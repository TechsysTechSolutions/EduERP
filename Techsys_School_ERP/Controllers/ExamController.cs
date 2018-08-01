using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Techsys_School_ERP.DBAccess;
using Techsys_School_ERP.Model;
using System.Web.Security;
using System.Data.Entity;
using Techsys_School_ERP.Model.ViewModel;

namespace Techsys_School_ERP.Controllers
{
    public class ExamController : CommonController
    {
		// GET: Exam/ExamList
		[Authorize(Roles = "Admin")]
		public ActionResult ExamList()
        {
			List<ExamList_ViewModel> examListViewModel = new List<ExamList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				examListViewModel = (from usr in dbcontext.Users
										join
									exam in dbcontext.Exam on usr.Id equals exam.Created_By
										where exam.Is_Deleted == null || exam.Is_Deleted == false
										select new ExamList_ViewModel
										{
											Id =  exam.Id,
											Name = exam.Name,
											Academic_Year = usr.Academic_Year,
											User_Id = usr.User_Id,
											Is_Deleted = usr.Is_Deleted,
											Created_On = exam.Created_On,
											Created_By = exam.Created_By

										}).ToList();


			}
			return View(examListViewModel);
		}


		public ActionResult AddExam_TimeTable()
		{
			//GetSubjects();
			PopulateClassAndSection();
			GetExamList();
			return View();
		}


		[HttpPost]
		public JsonResult AddExam_TimeTable(int Exam_Id , string[] Section_Ids, long Year)
		{
			List<Exam_TimeTableList_ViewModel> examTimeTableListViewModel = new List<Exam_TimeTableList_ViewModel>();

			List<Tuple<int, int>> subjectIdAndSectionIdList = new List<Tuple<int, int>>();

			//Tuple<int, int> subjectIdAndSectionIdList = new Tuple<int, int>;

			string[] nClassSectionIds = Section_Ids[0].Split(',');

			using (var dbcontext = new SchoolERPDBContext())
			{

				for (int nCount = 0; nCount < nClassSectionIds.Count(); nCount++)
			    {
					List<int> subjectIdList = new List<int>();
					int nSection_Id = Convert.ToInt32(nClassSectionIds[nCount]);
					var subList = (from subjectClsDetail in dbcontext.Subject_Class_Detail
									 where subjectClsDetail.Section_Id == nSection_Id && (subjectClsDetail.Is_Deleted == null || subjectClsDetail.Is_Deleted == false)
									 select new
									 {
										 Id = subjectClsDetail.Subject_Id
									 }).ToList().Select(x=> new Subject()
									 {
										 Id = x.Id
									 });

					foreach (var SubjectId in subList)
					{
						int nSubjectId = Convert.ToInt16(SubjectId.Id);
						//if (!subjectIdAndSectionIdList.ContainsKey(nSubjectId))
						//{
						subjectIdAndSectionIdList.Add(new Tuple<int, int>(nSubjectId, nSection_Id));
							//subjectIdAndSectionIdList.Add(nSubjectId,nSection_Id);
						//}
					}

				}

				foreach (var pair in subjectIdAndSectionIdList)
				{
					int nSectionId = pair.Item2;
					int nSubjectId = pair.Item1;
					string sExam_name = dbcontext.Exam.Where(x => x.Id == Exam_Id).FirstOrDefault().Name;
					if (dbcontext.Exam_TimeTable.Where(x => x.Exam_Id == Exam_Id && x.Academic_Year == Year && x.Section_Id == pair.Item2 && x.Subject_Id == pair.Item1 && (x.Is_Deleted == false || x.Is_Deleted == null)).Count() == 0)
					{
						Exam_TimeTableList_ViewModel examTimeTableViewModel = new Exam_TimeTableList_ViewModel();
						examTimeTableViewModel = (from subClassDetail in dbcontext.Subject_Class_Detail
												  join sec in dbcontext.Section on subClassDetail.Section_Id equals sec.Id
												  join cls in dbcontext.Class on sec.Class_Id equals cls.Id
												  join sub in dbcontext.Subject on subClassDetail.Subject_Id equals sub.Id

												  join user in dbcontext.Users on subClassDetail.Created_By equals user.Id

												  where (subClassDetail.Is_Deleted == null || subClassDetail.Is_Deleted == false)
												  && subClassDetail.Section_Id == pair.Item2 && (subClassDetail.Subject_Id == pair.Item1)

												  select new Exam_TimeTableList_ViewModel
												  {
													  Class_Id = cls.Id,
													  Section_Id = sec.Id,
													  Class_Name = cls.Name + " - " + sec.Name,
													  Subject_Id = sub.Id,
													  Academic_Year = Year,
													  Subject_Name = sub.Name,
													  Exam_Name = sExam_name,
													  Exam_Id = Exam_Id,
													  Exam_Date = null,
													  Exam_Session = 0


												  }).Distinct().ToList()[0];


						examTimeTableListViewModel.Add(examTimeTableViewModel);

					}
					else
					{

						Exam_TimeTableList_ViewModel examTimeTableViewModel = new Exam_TimeTableList_ViewModel();
						examTimeTableViewModel = (from examTimeTable in dbcontext.Exam_TimeTable
												  join sec in dbcontext.Section on examTimeTable.Section_Id equals sec.Id
												  join cls in dbcontext.Class on sec.Class_Id equals cls.Id
												  join sub in dbcontext.Subject on examTimeTable.Subject_Id equals sub.Id

												  join user in dbcontext.Users on examTimeTable.Created_By equals user.Id

												  where (examTimeTable.Is_Deleted == null || examTimeTable.Is_Deleted == false)
												  && examTimeTable.Section_Id == pair.Item2 && (examTimeTable.Subject_Id == pair.Item1)&&
												  examTimeTable.Exam_Id == Exam_Id && examTimeTable.Academic_Year == Year

												  select new Exam_TimeTableList_ViewModel
												  {
													  Class_Id = cls.Id,
													  Section_Id = sec.Id,
													  Class_Name = cls.Name + " - " + sec.Name,
													  Subject_Id = sub.Id,
													  Academic_Year = Year,
													  Subject_Name = sub.Name,
													  Exam_Name = sExam_name,
													  Exam_Id = Exam_Id,
													  Exam_Date = examTimeTable.Exam_Date == null ? "" : examTimeTable.Exam_Date.ToString(),
													  Exam_Session = examTimeTable.Exam_Session


												  }).Distinct().ToList()[0];


						examTimeTableListViewModel.Add(examTimeTableViewModel);

					}
				}
				
			}

			return Json(examTimeTableListViewModel.ToArray(), JsonRequestBehavior.AllowGet); ;

		}

		[HttpPost]
		public ActionResult SaveExamTimeTable_ForClass(string[][] exam_timetable)
		{
			string sReturnText = string.Empty;

			using (var dbcontext = new SchoolERPDBContext())
			{
				using (var transaction = dbcontext.Database.BeginTransaction())
				{
					try
					{
						if (exam_timetable.Length > 0)
						{
							

							for (int nRowCount = 0; nRowCount < exam_timetable.Length; nRowCount++)
							{
								Exam_TimeTable newexam_TimeTable = new Exam_TimeTable();
								int nSection_Id = Convert.ToInt32(exam_timetable[nRowCount][2]);
								long nAcademic_Year = Convert.ToInt64(exam_timetable[nRowCount][5]);
								int nClass_Id = Convert.ToInt32(exam_timetable[nRowCount][3]);
								int nExam_Id = Convert.ToInt32(exam_timetable[nRowCount][4]);
								int nSubject_Id = Convert.ToInt32(exam_timetable[nRowCount][1]);
								DateTime dtExamDate = Convert.ToDateTime(exam_timetable[nRowCount][0]);
								int nExam_Session = Convert.ToInt16(exam_timetable[nRowCount][6]);


								if (dbcontext.Exam_TimeTable.Where(x => x.Section_Id == nSection_Id && x.Academic_Year == nAcademic_Year && x.Exam_Id == nExam_Id && x.Subject_Id == nSubject_Id && (x.Is_Deleted == null || x.Is_Deleted == false)).Count() == 0)
								{
									newexam_TimeTable.Academic_Year = nAcademic_Year;
									newexam_TimeTable.Section_Id = nSection_Id;
									newexam_TimeTable.Class_Id = dbcontext.Section.Where(x => x.Id == nSection_Id).FirstOrDefault().Class_Id;
									newexam_TimeTable.Section_Id = nSection_Id;
									newexam_TimeTable.Exam_Id = nExam_Id;
									newexam_TimeTable.Exam_Date = dtExamDate;
									newexam_TimeTable.Subject_Id = nSubject_Id;
									newexam_TimeTable.Created_By = 5;
									newexam_TimeTable.Created_On = DateTime.Now;
									newexam_TimeTable.Is_Active = true;
									newexam_TimeTable.Exam_Session = nExam_Session;

									dbcontext.Exam_TimeTable.Add(newexam_TimeTable);
									dbcontext.SaveChanges();


								}
								else
								{
									var exam_TimeTable_Id_ToBeModified = dbcontext.Exam_TimeTable.Where(x => x.Section_Id == nSection_Id && x.Academic_Year == nAcademic_Year && x.Exam_Id == nExam_Id && x.Subject_Id == nSubject_Id && (x.Is_Deleted == null || x.Is_Deleted == false)).FirstOrDefault().Id;

									var exam_TimeTable_ToBeModified = dbcontext.Exam_TimeTable.Find(exam_TimeTable_Id_ToBeModified);
									exam_TimeTable_ToBeModified.Exam_Date = dtExamDate;
									exam_TimeTable_ToBeModified.Updated_By = 5;
									exam_TimeTable_ToBeModified.Updated_On = DateTime.Now;
									exam_TimeTable_ToBeModified.Exam_Session = nExam_Session;

									dbcontext.Entry(exam_TimeTable_ToBeModified).State = EntityState.Modified;
									dbcontext.SaveChanges();


								}
								if (nRowCount == (exam_timetable.Length - 1))
								{
									transaction.Commit();
									sReturnText = "OK";
								}
							}
						}
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						sReturnText = ex.InnerException.Message.ToString();
					}
				}
			}
			return Json(sReturnText, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		public JsonResult CreateExam(string Exam_Name)
		{
			try
			{
				Exam newExam = new Exam();
				int nUser_Id;
				using (var dbcontext = new SchoolERPDBContext())
				{
					nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
				}
				using (var dbcontext = new SchoolERPDBContext())
				{
					newExam.Name = Exam_Name;
					newExam.Academic_Year =  GetAcademicYear();
					newExam.Is_Active = true;
					newExam.Created_By = nUser_Id;
					newExam.Created_On = DateTime.Now;
					if (dbcontext.User_Roles.Where(a => a.Name == Exam_Name.Trim().ToString() && a.Is_Deleted == false).Count() == 0)
					{
						dbcontext.Exam.Add(newExam);
						dbcontext.SaveChanges();
						return Json("OK", JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json("Exam Already Exists.", JsonRequestBehavior.AllowGet);
					}
				}

			}
			catch (Exception e)
			{
				return Json("Failure", JsonRequestBehavior.AllowGet);
			}


		}


		public async Task<ActionResult> Delete(int id)
		{
			using (var dbcontext = new SchoolERPDBContext())
			{


				Exam exam = await dbcontext.Exam.FindAsync(id);
				if (exam != null)
				{
					exam.Is_Deleted = true;
					dbcontext.Entry(exam).CurrentValues.SetValues(exam);
					dbcontext.Entry(exam).State = EntityState.Modified;
					await dbcontext.SaveChangesAsync();
				}
			}

			return RedirectToAction("ExamList");
		}
	}
}