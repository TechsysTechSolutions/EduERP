using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Techsys_School_ERP.DBAccess;
using Techsys_School_ERP.Model;
using Techsys_School_ERP.Model.ViewModel;

namespace Techsys_School_ERP.Controllers
{
    public class TimeTableController : CommonController
    {
        // GET: TimeTable
        public ActionResult Staff_Subject_Detail()
        {
			List<StaffSubjectList_ViewModel> staffSubjectItemListViewModel = new List<StaffSubjectList_ViewModel>();
			long nYear = GetAcademicYear();
			GetSubjects();
			PopulateClassAndSection();
			using (var dbcontext = new SchoolERPDBContext())
			{
				staffSubjectItemListViewModel = (from usr in dbcontext.Users
											  join cls in dbcontext.Class on usr.Id equals cls.Created_By
											  join sec in dbcontext.Section on cls.Id equals sec.Class_Id
											  join clsSubDetail in dbcontext.Subject_Class_Detail on sec.Id equals clsSubDetail.Section_Id
											  join Subject in dbcontext.Subject on clsSubDetail.Subject_Id equals Subject.Id											   
											  join staffSubDetail in dbcontext.Staff_Subject_Detail on clsSubDetail.Section_Id equals staffSubDetail.Section_Id
											  join staff in dbcontext.Staff on staffSubDetail.Staff_Id equals staff.Staff_Id
											  where (staffSubDetail.Is_Deleted == null || staffSubDetail.Is_Deleted == false) && staffSubDetail.Academic_Year == nYear
											  select new StaffSubjectList_ViewModel
											  {
												  Id =  staffSubDetail.Id,
												  Staff_Id = staffSubDetail.Staff_Id,
												  Staff_Name = staff.First_Name + " " +  staff.Middle_Name + " " + staff.Last_Name ,
												  Class_Id = staffSubDetail.Staff_Id,
												  Class_Name = cls.Name,
												  Section_Id = sec.Id,
												  Section_Name = sec.Name,
												  Updated_On = staffSubDetail.Updated_On,
												  Academic_Year = staffSubDetail.Academic_Year

											  }).ToList();



			}

			return View(staffSubjectItemListViewModel);
		}

		public JsonResult GetStaffList(string q)
		{
			return Json(SearchAndGetStaffList(q), JsonRequestBehavior.AllowGet);
			
		}

		[HttpPost]
		public ActionResult AddStaff_Subject_Detail(string[] Class_Ids, string[] Subject_Ids, string Staff_Id)
		{
			string[] nClassSectionIds = Class_Ids[0].Split(',');
			string[] nSubjectIds = Subject_Ids[0].Split(',');
			string sReturnText = string.Empty;
			int nClassIds;
			long nYear = GetAcademicYear();

			Staff_Subject_Detail newStaffSubjectDetail = new Staff_Subject_Detail();

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

								newStaffSubjectDetail.Class_Id = nClassIds;
								newStaffSubjectDetail.Subject_Id = Convert.ToInt16(nSubjectIds[nSubjectLoopCount]);
								newStaffSubjectDetail.Section_Id = Convert.ToInt16(nClassSectionIds[nClassSubLoopCount]);
								newStaffSubjectDetail.Staff_Id = Convert.ToInt16(Staff_Id);
								newStaffSubjectDetail.Academic_Year = GetAcademicYear();
								newStaffSubjectDetail.Is_Active = true;
								newStaffSubjectDetail.Created_By = nUser_Id;
								newStaffSubjectDetail.Created_On = DateTime.Now;

								if (dbcontext.Staff_Subject_Detail.Where(a => a.Class_Id == nClassIds && a.Subject_Id == newStaffSubjectDetail.Subject_Id && a.Section_Id == newStaffSubjectDetail.Section_Id && (a.Is_Deleted == false || a.Is_Deleted == null) && a.Academic_Year == nYear && a.Staff_Id == newStaffSubjectDetail.Staff_Id).Count() == 0)
								{
									dbcontext.Staff_Subject_Detail.Add(newStaffSubjectDetail);
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
									return Json("Already Exists", JsonRequestBehavior.AllowGet);
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


			return Json(sReturnText.ToString(), JsonRequestBehavior.AllowGet);
		}

		public ActionResult AddStaff_TimeTable()
		{

			return View();
		}

		[HttpPost]
		public JsonResult AddStaff_TimeTable(int Staff_Id, long Academic_Year)
			{
			List<Staff_TimeTable> Staff_TimeTableList_ViewModelobj = new List<Staff_TimeTable>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				using (var transaction = dbcontext.Database.BeginTransaction())
				{
					try
					{
						PropertyInfo[] proeprties = typeof(Staff_TimeTable).GetProperties();

											
					    if (dbcontext.Staff_TimeTable.Where(x => x.Staff_Id == Staff_Id && x.Academic_Year == Academic_Year && (x.Is_Deleted == null || x.Is_Deleted == false)).Count() == 0)
						{

							for (int nCount = 0; nCount <= 5; nCount++)
							{
								Staff_TimeTable staffTimeTableViewModel = new Staff_TimeTable();

								foreach (PropertyInfo property in proeprties)
								{


									if (property.Name == "Week")
									{
										if (nCount == 0)
										{
											staffTimeTableViewModel.Week = 1;
										}
										else if (nCount == 1)
										{
											staffTimeTableViewModel.Week = 2;
										}
										else if (nCount == 2)
										{
											staffTimeTableViewModel.Week = 3;
										}
										else if (nCount == 3)
										{

											staffTimeTableViewModel.Week = 4;

										}
										else if (nCount == 4)
										{

											staffTimeTableViewModel.Week = 5;
										}
										else if (nCount == 5)
										{

											staffTimeTableViewModel.Week = 6;

										}
										
									}
									//if (nCount == 0)
									//{

									//}
									//else
									//{
										
									//}

								}

								Staff_TimeTableList_ViewModelobj.Add(staffTimeTableViewModel);

							}
							return Json(Staff_TimeTableList_ViewModelobj.ToArray(), JsonRequestBehavior.AllowGet);
						}
						else
						{
							var existingstaffTimeTable = dbcontext.Staff_TimeTable.Where(x => x.Staff_Id == Staff_Id && x.Academic_Year == Academic_Year && (x.Is_Deleted == null || x.Is_Deleted == false)).ToList();
							return Json(existingstaffTimeTable.ToArray(), JsonRequestBehavior.AllowGet);

						}
					}
					catch (Exception ex)
					{
						return Json(ex.InnerException.Message.ToString(), JsonRequestBehavior.AllowGet);
					}
				}
			}
			}

		public ActionResult AddClass_TimeTable()
		{
			PopulateClassAndSection();
			return View();
		}

		
		public JsonResult GetSubjectList(string q)
		{
			return Json(SearchAndGetSubjectList(q) , JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetClassAndSectionList(string q)
		{
			return Json(SearchAndGetClassAndSectionList(q), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult SaveTimeTable_For_Class(string[][] data)
		{
			string sReturnText = string.Empty;

			using (var dbcontext = new SchoolERPDBContext())
			{
				using (var transaction = dbcontext.Database.BeginTransaction())
				{
					try
					{
						if (data.Length > 0)
						{
							int nSection_Id = Convert.ToInt32(data[0][8]);
							long nAcademic_Year = Convert.ToInt64(data[0][9]);

							for (int nRowCount = 0; nRowCount < data.Length; nRowCount++)
							{
								Class_TimeTable class_TimeTable = new Class_TimeTable();

								if (dbcontext.Class_TimeTable.Where(x => x.Section_Id == nSection_Id && x.Academic_Year == nAcademic_Year && (x.Is_Deleted == null || x.Is_Deleted == false) && x.Week == (nRowCount + 1)).Count() == 0)
									{
										class_TimeTable.Academic_Year = nAcademic_Year;
										class_TimeTable.Section_Id = nSection_Id;
										class_TimeTable.Class_Id = dbcontext.Section.Where(x => x.Id == nSection_Id).FirstOrDefault().Class_Id;
										class_TimeTable.Subject_Id_Period_1 = Convert.ToInt16(data[nRowCount][0]);
										class_TimeTable.Subject_Id_Period_2 = Convert.ToInt16(data[nRowCount][1]);
										class_TimeTable.Subject_Id_Period_3 = Convert.ToInt16(data[nRowCount][2]);
										class_TimeTable.Subject_Id_Period_4 = Convert.ToInt16(data[nRowCount][3]);
										class_TimeTable.Subject_Id_Period_5 = Convert.ToInt16(data[nRowCount][4]);
										class_TimeTable.Subject_Id_Period_6 = Convert.ToInt16(data[nRowCount][5]);
										class_TimeTable.Subject_Id_Period_7 = Convert.ToInt16(data[nRowCount][6]);
										class_TimeTable.Subject_Id_Period_8 = Convert.ToInt16(data[nRowCount][7]);
										class_TimeTable.Week = nRowCount + 1;
										class_TimeTable.Created_By = 5;
										class_TimeTable.Created_On = DateTime.Now;
										class_TimeTable.Is_Active = true;

										dbcontext.Class_TimeTable.Add(class_TimeTable);
										dbcontext.SaveChanges();

										
									}
									else
									{
										var class_TimeTable_Id_ToBeModified = dbcontext.Class_TimeTable.Where(x => x.Section_Id == nSection_Id && x.Academic_Year == nAcademic_Year && (x.Is_Deleted == null || x.Is_Deleted == false) && x.Week == (nRowCount + 1)).FirstOrDefault().Id;

										var Class_TimeTable_ToBeModified = dbcontext.Class_TimeTable.Find(class_TimeTable_Id_ToBeModified);
										Class_TimeTable_ToBeModified.Subject_Id_Period_1 = Convert.ToInt16(data[nRowCount][0]);
										Class_TimeTable_ToBeModified.Subject_Id_Period_2 = Convert.ToInt16(data[nRowCount][1]);
										Class_TimeTable_ToBeModified.Subject_Id_Period_3 = Convert.ToInt16(data[nRowCount][2]);
										Class_TimeTable_ToBeModified.Subject_Id_Period_4 = Convert.ToInt16(data[nRowCount][3]);
										Class_TimeTable_ToBeModified.Subject_Id_Period_5 = Convert.ToInt16(data[nRowCount][4]);
										Class_TimeTable_ToBeModified.Subject_Id_Period_6 = Convert.ToInt16(data[nRowCount][5]);
										Class_TimeTable_ToBeModified.Subject_Id_Period_7 = Convert.ToInt16(data[nRowCount][6]);
										Class_TimeTable_ToBeModified.Subject_Id_Period_8 = Convert.ToInt16(data[nRowCount][7]);
										Class_TimeTable_ToBeModified.Updated_By = 5;
										Class_TimeTable_ToBeModified.Updated_On = DateTime.Now;

										dbcontext.Entry(Class_TimeTable_ToBeModified).State = EntityState.Modified;
										dbcontext.SaveChanges();


									}
									if (nRowCount == (data.Length - 1))
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
		public ActionResult SaveTimeTable_For_Staff(string[][] data)
		{
			string sReturnText = string.Empty;

			using (var dbcontext = new SchoolERPDBContext())
			{
				using (var transaction = dbcontext.Database.BeginTransaction())
				{
					try
					{
						if (data.Length > 0)
						{
							int nStaff_Id = Convert.ToInt32(data[0][8]);
							long nAcademic_Year = Convert.ToInt64(data[0][9]);

							for (int nRowCount = 0; nRowCount < data.Length; nRowCount++)
							{
								Staff_TimeTable staff_TimeTable = new Staff_TimeTable();

								if (dbcontext.Staff_TimeTable.Where(x => x.Staff_Id == nStaff_Id && x.Academic_Year == nAcademic_Year && (x.Is_Deleted == null || x.Is_Deleted == false) && x.Week == (nRowCount + 1)).Count() == 0)
								{
									staff_TimeTable.Academic_Year = nAcademic_Year;
									staff_TimeTable.Staff_Id = nStaff_Id;
									//staff_TimeTable.Class_Id = dbcontext.Section.Where(x => x.Id == nSection_Id).FirstOrDefault().Class_Id;
									staff_TimeTable.Section_Id_Period1 = Convert.ToInt16(data[nRowCount][0]);
									staff_TimeTable.Section_Id_Period2 = Convert.ToInt16(data[nRowCount][1]);
									staff_TimeTable.Section_Id_Period3 = Convert.ToInt16(data[nRowCount][2]);
									staff_TimeTable.Section_Id_Period4 = Convert.ToInt16(data[nRowCount][3]);
									staff_TimeTable.Section_Id_Period5 = Convert.ToInt16(data[nRowCount][4]);
									staff_TimeTable.Section_Id_Period6 = Convert.ToInt16(data[nRowCount][5]);
									staff_TimeTable.Section_Id_Period7 = Convert.ToInt16(data[nRowCount][6]);
									staff_TimeTable.Section_Id_Period8 = Convert.ToInt16(data[nRowCount][7]);
									staff_TimeTable.Week = nRowCount + 1;
									staff_TimeTable.Created_By = 5;
									staff_TimeTable.Created_On = DateTime.Now;
									staff_TimeTable.Is_Active = true;

									dbcontext.Staff_TimeTable.Add(staff_TimeTable);
									dbcontext.SaveChanges();


								}
								else
								{
									var staff_TimeTable_Id_ToBeModified = dbcontext.Staff_TimeTable.Where(x => x.Staff_Id == nStaff_Id && x.Academic_Year == nAcademic_Year && (x.Is_Deleted == null || x.Is_Deleted == false) && x.Week == (nRowCount + 1)).FirstOrDefault().Id;

									var Staff_TimeTable_ToBeModified = dbcontext.Staff_TimeTable.Find(staff_TimeTable_Id_ToBeModified);
									Staff_TimeTable_ToBeModified.Section_Id_Period1 = Convert.ToInt16(data[nRowCount][0]);
									Staff_TimeTable_ToBeModified.Section_Id_Period2 = Convert.ToInt16(data[nRowCount][1]);
									Staff_TimeTable_ToBeModified.Section_Id_Period3 = Convert.ToInt16(data[nRowCount][2]);
									Staff_TimeTable_ToBeModified.Section_Id_Period4 = Convert.ToInt16(data[nRowCount][3]);
									Staff_TimeTable_ToBeModified.Section_Id_Period5 = Convert.ToInt16(data[nRowCount][4]);
									Staff_TimeTable_ToBeModified.Section_Id_Period6 = Convert.ToInt16(data[nRowCount][5]);
									Staff_TimeTable_ToBeModified.Section_Id_Period7 = Convert.ToInt16(data[nRowCount][6]);
									Staff_TimeTable_ToBeModified.Section_Id_Period8 = Convert.ToInt16(data[nRowCount][7]);
									Staff_TimeTable_ToBeModified.Updated_By = 5;
									Staff_TimeTable_ToBeModified.Updated_On = DateTime.Now;

									dbcontext.Entry(Staff_TimeTable_ToBeModified).State = EntityState.Modified;
									dbcontext.SaveChanges();


								}
								if (nRowCount == (data.Length - 1))
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

		public JsonResult FetchSubjectNameBasedOnId(string subjectId)
		{
			return Json(new { items = GetSubjectNameBasedOnId(subjectId) }, JsonRequestBehavior.AllowGet);

		}

		public JsonResult FetchSectionIdBasedOnId(string sectionId)
		{
			return Json(new { items = GetClassAndSectionNameBasedOnId(sectionId) }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult AddClass_TimeTable(int Section_Id, long Academic_Year)
		{
			//List<Class_TimeTableList_ViewModel> Class_TimeTableList_ViewModelobj = new List<Model.ViewModel.Class_TimeTableList_ViewModel>();
			List<Class_TimeTable> Class_TimeTableList_ViewModelobj = new List<Class_TimeTable>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				using (var transaction = dbcontext.Database.BeginTransaction())
				{
					try
					{
						PropertyInfo[] proeprties = typeof(Class_TimeTable).GetProperties();

											
					    if (dbcontext.Class_TimeTable.Where(x => x.Section_Id == Section_Id && x.Academic_Year == Academic_Year && (x.Is_Deleted == null || x.Is_Deleted == false)).Count() == 0)
						{

							for (int nCount = 0; nCount <= 5; nCount++)
							{
								Class_TimeTable classTimeTableViewModel = new Class_TimeTable();

								foreach (PropertyInfo property in proeprties)
								{


									if (property.Name == "Week")
									{
										if (nCount == 0)
										{
											classTimeTableViewModel.Week = 1;
										}
										else if (nCount == 1)
										{											
											classTimeTableViewModel.Week = 2;
										}
										else if (nCount == 2)
										{
											classTimeTableViewModel.Week = 3;
										}
										else if (nCount == 3)
										{
											
											classTimeTableViewModel.Week = 4;

										}
										else if (nCount == 4)
										{
											
											classTimeTableViewModel.Week = 5;
										}
										else if (nCount == 5)
										{
											
											classTimeTableViewModel.Week = 6;

										}
										
									}
									//if (nCount == 0)
									//{

									//}
									//else
									//{
										
									//}

								}

								Class_TimeTableList_ViewModelobj.Add(classTimeTableViewModel);

							}
							return Json(Class_TimeTableList_ViewModelobj.ToArray(), JsonRequestBehavior.AllowGet);
						}
						else
						{
							var existingClassTimeTable = dbcontext.Class_TimeTable.Where(x => x.Section_Id == Section_Id && x.Academic_Year == Academic_Year && (x.Is_Deleted == null || x.Is_Deleted == false)).ToList();
							return Json(existingClassTimeTable.ToArray(), JsonRequestBehavior.AllowGet);

						}
					}
					catch (Exception ex)
					{
						return Json(ex.InnerException.Message.ToString(), JsonRequestBehavior.AllowGet);
					}
				}
			}
			
		}
	}
}