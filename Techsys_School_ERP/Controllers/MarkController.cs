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
using System.Web.Script.Serialization;
using System.Data;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Dynamic;

namespace Techsys_School_ERP.Controllers
{
    public class MarkController : CommonController
    {
		long[] nStudentIdArr;
		long[] nSubjectForClassArr;
		// GET: Mark
		public ActionResult Index()
        {
            return View();
        }

		public ActionResult GetMarksForClass()
		{
			GetClass();
			GetExamList();
			return View();

		}

		[HttpPost]
		public JsonResult FetchSectionIDForClass(string Class_Id)
		{
			List<Section> lstSection = GetSectionForClass(Class_Id);

			return Json(lstSection, JsonRequestBehavior.AllowGet);

		}

		
		public ActionResult ConfigureSubjectsForClass()
		{
			List<Section> lstsection = PopulateClassAndSection();
			if (lstsection != null)
			{
				ViewBag.classSectionList = lstsection.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
			}
			GetSubjects();
			
			return View(populateGrid());
		}

		[HttpPost]
		public JsonResult ConfigureSubjectsForClass(string[] Class_Ids , string[] Subject_Ids)
		{
			string[] nClassSectionIds = Class_Ids[0].Split(',');
			string[] nSubjectIds = Subject_Ids[0].Split(',');
			string sReturnText = string.Empty;
			int nClassIds;


			Subject_Class_Detail newSubjClassDetail = new Subject_Class_Detail();

			int nUser_Id;
			using (var dbcontext = new SchoolERPDBContext())
			{
				//nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
				nUser_Id = 4;
			}

			using (var dbcontext = new SchoolERPDBContext())
			{
				using (var transaction = dbcontext.Database.BeginTransaction())
				{
					try
					{
						for (int nClassSubLoopCount = 0; nClassSubLoopCount < nClassSectionIds.Count(); nClassSubLoopCount++)
						{
							int nSectionId = Convert.ToInt16(nClassSectionIds[nClassSubLoopCount]);


							for (int nSubjectLoopCount = 0; nSubjectLoopCount < nSubjectIds.Count(); nSubjectLoopCount++)
							{


								nClassIds = dbcontext.Section.Where(x => x.Id == nSectionId).ToList()[0].Class_Id;

								newSubjClassDetail.Class_Id = nClassIds;
								newSubjClassDetail.Subject_Id = Convert.ToInt16(nSubjectIds[nSubjectLoopCount]);
								newSubjClassDetail.Section_Id = Convert.ToInt16(nClassSectionIds[nClassSubLoopCount]);
								newSubjClassDetail.Academic_Year = (DateTime.Now.Month <= 4) ? DateTime.Now.Year - 1 : DateTime.Now.Year;
								newSubjClassDetail.Is_Active = true;
								newSubjClassDetail.Created_By = nUser_Id;
								newSubjClassDetail.Created_On = DateTime.Now;

								if (dbcontext.Subject_Class_Detail.Where(a => a.Class_Id == nClassIds && a.Subject_Id == newSubjClassDetail.Subject_Id && a.Section_Id == newSubjClassDetail.Section_Id && (a.Is_Deleted == false || a.Is_Deleted == null) && a.Academic_Year == newSubjClassDetail.Academic_Year).Count() == 0)
								{
									dbcontext.Subject_Class_Detail.Add(newSubjClassDetail);
									dbcontext.SaveChanges();
									//return Json("OK", JsonRequestBehavior.AllowGet);

									if (nClassSubLoopCount == nClassSectionIds.Count() - 1 && nSubjectLoopCount == nSubjectIds.Count() - 1)
									{
										transaction.Commit();
										sReturnText = "OK";
									}
								}
								else
								{
									return Json("Updated", JsonRequestBehavior.AllowGet);
								}



							}
						}
					}
					catch (Exception ex)
					{
						transaction.Rollback();
					}
							}
					
					}

			
			//List<Subject_ClassList_ViewModel> subjectClassListViewModel = new List<Subject_ClassList_ViewModel>();
			//using (var dbcontext = new SchoolERPDBContext())
			//{
			//	subjectClassListViewModel = (from usr in dbcontext.Users
			//						 join subjClassDetail in dbcontext.Subject_Class_Detail on usr.Id equals subjClassDetail.Created_By
			//						 join sub in dbcontext.Subject on subjClassDetail.Subject_Id equals sub.Id
			//						 join sec in dbcontext.Section on subjClassDetail.Section_Id equals sec.Id
			//						 join cls in dbcontext.Class on subjClassDetail.Class_Id equals cls.Id
			//						 where subjClassDetail.Is_Deleted == null || subjClassDetail.Is_Deleted == false
			//						 select new Subject_ClassList_ViewModel
			//						 {
			//							 Id = subjClassDetail.Id,
			//							 ClassSectionName = cls.Name + sec.Name ,
			//							 Subject_Id = subjClassDetail.Subject_Id,
			//							 Subject_Name = sub.Name,
			//							 Academic_Year = usr.Academic_Year,
			//							 User_Id = usr.User_Id,
			//							 Is_Deleted = usr.Is_Deleted,
			//							 Created_On = subjClassDetail.Created_On,
			//							 Created_By = subjClassDetail.Created_By

			//						 }).ToList();


			//}
			//return  Json(subjectClassListViewModel.ToArray(), JsonRequestBehavior.AllowGet);

			return Json(sReturnText.ToString(), JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		public List<Subject_ClassList_ViewModel> populateGrid()
		{
			List<Subject_ClassList_ViewModel> subjectClassListViewModel = new List<Subject_ClassList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				subjectClassListViewModel = (from usr in dbcontext.Users
											 join subjClassDetail in dbcontext.Subject_Class_Detail on usr.Id equals subjClassDetail.Created_By
											 join sub in dbcontext.Subject on subjClassDetail.Subject_Id equals sub.Id
											 join sec in dbcontext.Section on subjClassDetail.Section_Id equals sec.Id
											 join cls in dbcontext.Class on subjClassDetail.Class_Id equals cls.Id
											 where (subjClassDetail.Is_Deleted == null || subjClassDetail.Is_Deleted == false)
											 select new Subject_ClassList_ViewModel
											 {
												 Id = subjClassDetail.Id,
												 ClassSectionName = cls.Name + sec.Name,
												 Subject_Id = subjClassDetail.Subject_Id,
												 Subject_Name = sub.Name,
												 Academic_Year = usr.Academic_Year,
												 User_Id = usr.User_Id,
												 Is_Deleted = usr.Is_Deleted,
												 Created_On = subjClassDetail.Created_On,
												 Created_By = subjClassDetail.Created_By

											 }).ToList();


			}
			return subjectClassListViewModel;
		}

		[HttpPost]
		public JsonResult SaveMarksForClass(List<string[]> myData)
		{
			Mark newmarkToBeAdded = new Mark();
			string sReturnText = string.Empty;
			int nUser_Id;
			int[] nSubjectIdArr = (int[])TempData.Peek("SubjectId_For_Marks");

			nStudentIdArr = (long[])TempData.Peek("StudentIdArr_For_Marks");

			using (var dbcontext = new SchoolERPDBContext())
			{
				//nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
				nUser_Id = 4;
			}
			int nCount = myData.ToList().Count;

			int nLoopCount = 0;

			using (var dbcontext = new SchoolERPDBContext())
			{
				using (var transaction = dbcontext.Database.BeginTransaction())
				{
					try
					{
						for (int iMarkDetailLoopCount = 1; iMarkDetailLoopCount < myData.ToList().Count(); iMarkDetailLoopCount++)
						{
							Mark newMarkDetail = new Mark();
							newMarkDetail.Class_Id = Convert.ToInt16(TempData.Peek("Class_Id_For_Marks"));
							newMarkDetail.Section_Id = Convert.ToInt16(TempData.Peek("Section_Id_For_Marks"));
							newMarkDetail.Student_Id = nStudentIdArr[iMarkDetailLoopCount - 1];
							newMarkDetail.Is_Active = true;
							newMarkDetail.Is_Deleted = false;
							newMarkDetail.Exam_Id = Convert.ToInt16(TempData.Peek("Exam_Id_For_Marks"));
							newMarkDetail.Created_By = 4;
							newMarkDetail.Created_On = DateTime.Now;
							newMarkDetail.Academic_Year = Convert.ToInt16(TempData.Peek("Year_For_Marks"));
							for (int nSubjectCount = 0; nSubjectCount < nSubjectIdArr.Count() ; nSubjectCount++)
							{
							
								newMarkDetail.GetType().GetProperty("Subject_Id" + (nSubjectCount + 1)).SetValue(newMarkDetail, nSubjectIdArr[nSubjectCount], null);
								newMarkDetail.GetType().GetProperty("Mark" + (nSubjectCount + 1)).SetValue(newMarkDetail, Convert.ToInt16(myData[iMarkDetailLoopCount][nSubjectCount+2]), null);

							}

							if (dbcontext.Mark.Where(a => a.Student_Id == newMarkDetail.Student_Id && a.Academic_Year == newMarkDetail.Academic_Year && a.Exam_Id == newMarkDetail.Exam_Id && (a.Is_Deleted == false || a.Is_Deleted == null)).Count() == 0)
									{

										dbcontext.Mark.Add(newMarkDetail);
										dbcontext.SaveChanges();
								

									}
									else
									{
										var markId = dbcontext.Mark.Where(a => a.Student_Id == newMarkDetail.Student_Id && a.Academic_Year == newMarkDetail.Academic_Year && a.Exam_Id == newMarkDetail.Exam_Id && (a.Is_Deleted == false || a.Is_Deleted == null)).ToList()[0].Id;

										Mark markToBeUpdated = dbcontext.Mark.Find(markId);
										markToBeUpdated.Is_Active = true;

										markToBeUpdated.Mark1 = newMarkDetail.Mark1;
										markToBeUpdated.Mark2 = newMarkDetail.Mark2;
										markToBeUpdated.Mark3 = newMarkDetail.Mark3;
										markToBeUpdated.Mark4 = newMarkDetail.Mark4;
										markToBeUpdated.Mark5 = newMarkDetail.Mark5;
										markToBeUpdated.Mark6 = newMarkDetail.Mark6;
										markToBeUpdated.Mark7 = newMarkDetail.Mark7;
										markToBeUpdated.Mark8 = newMarkDetail.Mark8;
										markToBeUpdated.Mark9 = newMarkDetail.Mark9;
										markToBeUpdated.Mark10 = newMarkDetail.Mark10;

										dbcontext.Entry(markToBeUpdated).State = EntityState.Modified;
										dbcontext.SaveChanges();
									}

									if (iMarkDetailLoopCount == (myData.ToList().Count() - 1) )
									{
									transaction.Commit();
									sReturnText = "OK";
									}
						}



					}
					catch (Exception ex)
					{
						transaction.Rollback();
					}

				}
				return Json(sReturnText, JsonRequestBehavior.AllowGet);
			}

		}

		[HttpPost]
		public JsonResult GetMarkListForClass(string class_Id, string section_Id, string exam_Id, string year)
		{
			List<Student> studentList = new List<Student>();
			int nNoOfTermDays;
			DataTable dtStudentAttendance = new DataTable();

			//dynamic catList = new List<System.Dynamic.ExpandoObject>();


			TempData["Class_Id_For_Marks"] = class_Id;
			TempData.Keep("Class_Id_For_Marks");

			TempData["Section_Id_For_Marks"] = section_Id;
			TempData.Keep("Section_Id_For_Marks");

			TempData["Exam_Id_For_Marks"] = exam_Id;
			TempData.Keep("Exam_Id_For_Marks");


			TempData["Year_For_Marks"] = year;
			TempData.Keep("Year_For_Marks");

			int nYear = Convert.ToInt16(year);
			int nTermId = Convert.ToInt16(exam_Id);
			int nClassId = Convert.ToInt16(class_Id);
			int nSectionId = Convert.ToInt16(section_Id);
			int[] nSectionIdArr;


			List<MarkList_ViewModel> mark_ViewModelList = new List<MarkList_ViewModel>();
			List<Subject> classSubjectList = new List<Subject>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				studentList = (from stu in dbcontext.Student
							   where stu.Academic_Year == nYear && (stu.Is_Deleted == false) && stu.Class_Id == nClassId && stu.Section_Id == nSectionId
							   select new
							   {
								   Id = stu.Student_Id,
								   Name = stu.First_Name + " " + stu.Last_Name,
								   Roll_No = stu.Roll_No
							   }).ToList().Select(x => new Student()
							   {
								   Student_Id = x.Id,
								   First_Name = x.Name,
								   Roll_No = x.Roll_No
							   }).ToList();


			}

			TempData["StudentIdArr_For_Marks"] = studentList.ToList().Select(l => l.Student_Id).Distinct().ToArray();
			//TempData["StudentIdArr"] = nStudentIdArr;

			using (var dbcontext = new SchoolERPDBContext())
			{
				classSubjectList = (from clsSubDetail in dbcontext.Subject_Class_Detail join
								   sub in dbcontext.Subject on clsSubDetail.Subject_Id equals sub.Id
								   where clsSubDetail.Academic_Year == nYear && (clsSubDetail.Is_Deleted == false || clsSubDetail.Is_Deleted == null) && clsSubDetail.Class_Id == nClassId && clsSubDetail.Section_Id == nSectionId
								   select new
								   {
									  
									   Id = clsSubDetail.Subject_Id,
									   Name = sub.Name 
								   }).ToList().Select(x => new Subject()
								   {
									   Id = x.Id,
									   Name = x.Name
								   }).ToList();
			}

			TempData["SubjectId_For_Marks"] = classSubjectList.ToList().Select(l => l.Id).Distinct().ToArray();
			//TempData["StudentIdArr"] = nSectionIdArr;

			PropertyInfo[] properties = typeof(MarkList_ViewModel).GetProperties();

			for (int nCount = 0; nCount <= studentList.Count; nCount++)
			{
				MarkList_ViewModel markViewModel = new MarkList_ViewModel();
				
				int nPropertyCount = 0;
				foreach (PropertyInfo property in properties)
				{
					
					if (nCount == 0)
					{

						if (property.Name.Contains("Subject_Id") && nPropertyCount < classSubjectList.Count())
						{
							int nSubjectId = Convert.ToInt16(property.Name.Replace("Subject_Id", ""));

							property.SetValue(markViewModel, classSubjectList[nPropertyCount].Name);
							nPropertyCount++;


						}
						else if (property.Name == "Student_Name")
						{
							property.SetValue(markViewModel, property.Name);
						}
						else if (property.Name == "Total")
						{
							property.SetValue(markViewModel, property.Name);
						}
						else if (property.Name == "Average")
						{
							property.SetValue(markViewModel, property.Name);
						}



					}
					else
					{
						if (property.Name == "Student_Name")
						{
							markViewModel.Student_Name = studentList[nCount - 1].First_Name + " " + studentList[nCount - 1].Last_Name;
						}
						else if (property.Name == "Student_Id")
						{
							nStudentIdArr = studentList.ToList().Select(l => l.Student_Id).Distinct().ToArray();
							TempData["StudentIdArr"] = nStudentIdArr;
							TempData.Keep("StudentIdArr");
							continue;
						}

						using (var dbcontext = new SchoolERPDBContext())
						{
							long nStudentId = studentList[nCount - 1].Student_Id;
							if (dbcontext.Mark.Where(a => a.Student_Id == nStudentId  && a.Academic_Year == nYear && a.Exam_Id == nTermId && (a.Is_Deleted == false || a.Is_Deleted == null)).Count() > 0)
							{
								Mark existingMark = dbcontext.Mark.Where(a => a.Student_Id == nStudentId && a.Academic_Year == nYear && a.Exam_Id == nTermId && (a.Is_Deleted == false || a.Is_Deleted == null)).ToList()[0];
								markViewModel.Subject_Id1 = Convert.ToString(existingMark.Mark1);
								markViewModel.Subject_Id2 = Convert.ToString(existingMark.Mark2);
								markViewModel.Subject_Id3 = Convert.ToString(existingMark.Mark3);
								markViewModel.Subject_Id4 = Convert.ToString(existingMark.Mark4);
								markViewModel.Subject_Id5 = Convert.ToString(existingMark.Mark5);
								markViewModel.Subject_Id6 = Convert.ToString(existingMark.Mark6);
								markViewModel.Subject_Id7 = Convert.ToString(existingMark.Mark7);
								markViewModel.Subject_Id8 = Convert.ToString(existingMark.Mark8);
								markViewModel.Subject_Id9 = Convert.ToString(existingMark.Mark9);
								markViewModel.Subject_Id10 = Convert.ToString(existingMark.Mark10);


							}
						}
					}
				}
				mark_ViewModelList.Add(markViewModel);
			}

			

			
			return Json(mark_ViewModelList.ToArray(), JsonRequestBehavior.AllowGet);
		}

		public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
		{
			// ExpandoObject supports IDictionary so we can extend it like this
			var expandoDict = expando as IDictionary<string, object>;
			if (expandoDict.ContainsKey(propertyName))
				expandoDict[propertyName] = propertyValue;
			else
				expandoDict.Add(propertyName, propertyValue);
		}
	}
}