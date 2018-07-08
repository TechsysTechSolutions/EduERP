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

namespace Techsys_School_ERP.Controllers
{
    public class AttendanceController : CommonController
	{
		long[] nStudentIdArr;
		// GET: Attendance
		public ActionResult GetAttendanceForClass()
        {
			GetClass();
			GetTerm();
			return View();
			
        }
		#region StudentAttendance
		[HttpPost]
		public JsonResult GetAttendanceForClass(string class_Id ,string section_Id , string term_Id , string year)
		{
			List<Student> studentList = new List<Student>();
			int nNoOfTermDays;
			DataTable dtStudentAttendance = new DataTable();


			TempData["Class_Id_For_Attendance"] = class_Id;
			TempData.Keep("Class_Id_For_Attendance");

			TempData["Section_Id_For_Attendance"] = section_Id;
			TempData.Keep("Section_Id_For_Attendance");

			TempData["Term_Id_For_Attendance"] = term_Id;
			TempData.Keep("Term_Id_For_Attendance");


			TempData["Year_For_Attendance"] = year;
			TempData.Keep("Year_For_Attendance");

			int nYear = Convert.ToInt16(year);
			int nTermId = Convert.ToInt16(term_Id);
			int nClassId = Convert.ToInt16(class_Id);
			int nSectionId = Convert.ToInt16(section_Id);
		

			DateTime termStartDate;
			DateTime termEndDate;
			List<Attendance_ViewModel> attendance_ViewModelList = new List<Attendance_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				studentList = (from stu in dbcontext.Student
							   where  stu.Academic_Year == nYear && ( stu.Is_Deleted == false) && stu.Class_Id == nClassId && stu.Section_Id == nSectionId
							   select new
							   {
								   Id = stu.Student_Id,
								   Name = stu.First_Name + " " + stu.Last_Name ,
								   Roll_No = stu.Roll_No 
							   }).ToList().Select(x => new Student()
							   {
									Student_Id = x.Id,
									First_Name = x.Name,
									Roll_No = x.Roll_No
							   }).ToList();


			}

			int nStudentCount = studentList.Count;

			using (var dbcontext = new SchoolERPDBContext())
			{				
				termEndDate = dbcontext.Term.Where(x => x.Id == nTermId).ToList()[0].To_Date;
				termStartDate = dbcontext.Term.Where(x => x.Id == nTermId).ToList()[0].From_Date;
			}

			TimeSpan difference = termEndDate - termStartDate;
			int nDays = Convert.ToInt16(difference.TotalDays);
			nDays = nDays + 1;
			long nStudent_Id;

			PropertyInfo[] proeprties = typeof(Attendance_ViewModel).GetProperties();
			
			for (int nCount = 0; nCount <= studentList.Count; nCount++)
			{
				Attendance_ViewModel attendanceViewModel = new Attendance_ViewModel();
				int nValue;
				DateTime dDateToBeCompared;
				if (nCount > 0)
				{
					nStudent_Id = studentList[nCount - 1].Student_Id;
				}
				else
				{
					nStudent_Id = 0;
				}
				
				foreach (PropertyInfo property in proeprties)
				{
					string sDay = property.Name;

					if (property.Name == "Roll_No")
					{
						if (nCount == 0)
						{
							property.SetValue(attendanceViewModel, property.Name);
						}
						else
						{
							attendanceViewModel.Roll_No = studentList[nCount - 1].Roll_No;
						}
					}
					else if (property.Name == "Student_Name")
					{
						if (nCount == 0)
						{
							property.SetValue(attendanceViewModel, property.Name);
						}
						else
						{
							attendanceViewModel.Student_Name = studentList[nCount - 1].First_Name + " " + studentList[nCount - 1].Last_Name;
						}
					}
					else if (property.Name == "Student_Id")
					{
						nStudentIdArr = studentList.ToList().Select(l => l.Student_Id).Distinct().ToArray();
						if (nCount == 0)
						{
							property.SetValue(attendanceViewModel, "STUDENT ID");
						}
						else
						{
							attendanceViewModel.Student_Id = Convert.ToString(studentList[nCount - 1].Student_Id);
						}
						TempData["StudentIdArr"] = nStudentIdArr;
						TempData.Keep("StudentIdArr");			
						continue;
					}


					if (sDay.Contains("Day_"))
					{
						string sHolidayReason = string.Empty; ;
						sDay = sDay.Replace("Day_", "");
						dDateToBeCompared = termStartDate.AddDays(Convert.ToInt16(sDay)-1);

						if (nDays >= Convert.ToInt16(sDay))
						{
							if (nCount == 0)
							{
								property.SetValue(attendanceViewModel, dDateToBeCompared.Day + "-" + dDateToBeCompared.ToString("MMM").ToUpper());
								continue;
							}

							else
							{
								using (var dbcontext = new SchoolERPDBContext())
								{
									if (dbcontext.Holiday.Where((x => x.Holiday_Date == dDateToBeCompared  && x.Academic_Year == nYear && (x.Is_Deleted == false || x.Is_Deleted == null))).ToList().Count() > 0)
									{
										sHolidayReason = dbcontext.Holiday.Where((x => x.Holiday_Date == dDateToBeCompared && x.Academic_Year == nYear && (x.Is_Deleted == false || x.Is_Deleted == null))).ToList()[0].Name;

										sHolidayReason = "H" + "-" + sHolidayReason;
									}
									else
									{
										if (dbcontext.Attendance.Where(x => x.Leave_Date == dDateToBeCompared && x.Academic_Year == nYear && (x.Is_Deleted == false || x.Is_Deleted == null) && x.Student_Id == nStudent_Id).ToList().Count() > 0)
										{
											sHolidayReason = dbcontext.Attendance.Where(x => x.Leave_Date == dDateToBeCompared && x.Academic_Year == nYear && (x.Is_Deleted == false || x.Is_Deleted == null) && x.Student_Id == nStudent_Id).ToList()[0].Leave_Reason;
										}
									}
								}

							}							
							
								if (sHolidayReason == string.Empty)
								{
									property.SetValue(attendanceViewModel, "P");

								}
								else
								{
									property.SetValue(attendanceViewModel, sHolidayReason);
								}
							}						
					}					
				}
				attendance_ViewModelList.Add(attendanceViewModel);
			}		
			return Json(attendance_ViewModelList.ToArray(), JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		public JsonResult SaveAttendanceForClass(List<string[]> myData)
		{
			
			int nUser_Id;

			nStudentIdArr = (long[])TempData.Peek("StudentIdArr");

			string sReturnText = string.Empty;
			DateTime termEndDate;
			DateTime termStartDate;

			using (var dbcontext = new SchoolERPDBContext())
			{
				//nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
				nUser_Id = 4;
			}
			int nCount = myData.ToList().Count;
			int nTerm_Id = Convert.ToInt16(TempData.Peek("Term_Id_For_Attendance"));

			int nLoopCount = 0;

			using (var dbcontext = new SchoolERPDBContext())
			{
				termEndDate = dbcontext.Term.Where(x => x.Id == nTerm_Id).ToList()[0].To_Date;
				termStartDate = dbcontext.Term.Where(x => x.Id == nTerm_Id).ToList()[0].From_Date;
			}

			TimeSpan difference = termEndDate - termStartDate;
			int nDays = Convert.ToInt16(difference.TotalDays);
			nDays = nDays + 1;

			using (var dbcontext = new SchoolERPDBContext())
			{
				using (var transaction = dbcontext.Database.BeginTransaction())
				{
					try
					{
						for (int iAttendanceLoopCount = 1; iAttendanceLoopCount < myData.ToList().Count(); iAttendanceLoopCount++)
						{
							
							for (int nColumnCount = 3; nColumnCount < 3 + nDays; nColumnCount++)
							{
								Attendance newAttendance = new Attendance();
								newAttendance.Academic_Year = Convert.ToInt32(GetAcademicYear());
								if (Convert.ToString(myData[iAttendanceLoopCount][nColumnCount]) != "P" && Convert.ToString(myData[iAttendanceLoopCount][nColumnCount]) != "H" && Convert.ToString(myData[iAttendanceLoopCount][nColumnCount]) != "PH" && Convert.ToString(myData[iAttendanceLoopCount][nColumnCount]) != null)
								{
									newAttendance.Class_Id = Convert.ToInt16(TempData.Peek("Class_Id_For_Attendance"));
									newAttendance.Section_Id = Convert.ToInt16(TempData.Peek("Section_Id_For_Attendance"));
									newAttendance.Leave_Date = termStartDate.AddDays(nColumnCount - 3);
									newAttendance.Student_Id = nStudentIdArr[iAttendanceLoopCount - 1];
									newAttendance.Leave_Reason = myData[iAttendanceLoopCount][nColumnCount];
									newAttendance.Is_Active = true;
									//newAttendance.Is_Deleted = false;
									newAttendance.Term_Id = Convert.ToInt16(TempData.Peek("Term_Id_For_Attendance"));
									newAttendance.Created_By = 4;
									newAttendance.Created_On = DateTime.Now;
									newAttendance.Academic_Year = Convert.ToInt16(TempData.Peek("Year_For_Attendance"));

									if (dbcontext.Attendance.Where(a => a.Student_Id == newAttendance.Student_Id && a.Academic_Year == newAttendance.Academic_Year && a.Leave_Date == newAttendance.Leave_Date && (a.Is_Deleted == false || a.Is_Deleted == null)).Count() == 0)
									{

										dbcontext.Attendance.Add(newAttendance);
										dbcontext.SaveChanges();

									}
									else
									{
										var attendanceId = dbcontext.Attendance.Where(a => a.Student_Id == newAttendance.Student_Id && a.Academic_Year == newAttendance.Academic_Year && a.Leave_Date == newAttendance.Leave_Date && (a.Is_Deleted == false || a.Is_Deleted == null)).ToList()[0].Id;

										Attendance attendanceToBeUpdated = dbcontext.Attendance.Find(attendanceId);

										attendanceToBeUpdated.Leave_Reason = myData[iAttendanceLoopCount][nColumnCount];
										attendanceToBeUpdated.Is_Active = true;

										dbcontext.Entry(attendanceToBeUpdated).State = EntityState.Modified;
										dbcontext.SaveChanges();
									}
								}
								else
								{
									newAttendance.Leave_Date = termStartDate.AddDays(nColumnCount - 3);
									string LeaveReason = myData[iAttendanceLoopCount][nColumnCount];
									if (dbcontext.Attendance.Where(a => a.Student_Id == newAttendance.Student_Id && a.Academic_Year == newAttendance.Academic_Year && a.Leave_Date == newAttendance.Leave_Date && (a.Is_Deleted == false || a.Is_Deleted == null) && a.Leave_Reason != LeaveReason).Count() > 0)
									{
										var attendanceId = dbcontext.Attendance.Where(a => a.Student_Id == newAttendance.Student_Id && a.Academic_Year == newAttendance.Academic_Year && a.Leave_Date == newAttendance.Leave_Date && (a.Is_Deleted == false || a.Is_Deleted == null)).ToList()[0].Id;

										Attendance attendanceToBeUpdated = dbcontext.Attendance.Find(attendanceId);

										attendanceToBeUpdated.Leave_Reason = myData[iAttendanceLoopCount][nColumnCount];
										attendanceToBeUpdated.Is_Active = false;
										attendanceToBeUpdated.Is_Deleted = true;

										dbcontext.Entry(attendanceToBeUpdated).State = EntityState.Modified;
										dbcontext.SaveChanges();
									}
								}

								if (iAttendanceLoopCount == myData.ToList().Count() - 1 && nColumnCount == 3 + (nDays - 1 ))
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
					}

				}
				return Json(sReturnText, JsonRequestBehavior.AllowGet);
			}
		}


		public ArrayList DataTableToJsonObj(DataTable dt)
		{
			ArrayList myArrayList = new ArrayList();
			for (int i = 0; i <= dt.Rows.Count - 1; i++)
			{
				for (int j = 0; j <= dt.Columns.Count - 1; j++)
				{
					myArrayList.Add(dt.Rows[i][j].ToString());
				}
			}
			return myArrayList;
		}
	

		[HttpPost]
		public JsonResult FetchSectionIDForClass(string Class_Id)
		{
			List<Section> lstSection = GetSectionForClass(Class_Id);
			
			return Json(lstSection, JsonRequestBehavior.AllowGet);
			
		}


		public ActionResult HolidayList()
		{
			List<HolidayList_ViewModel> holidayListViewModel = new List<HolidayList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				holidayListViewModel  = (from usr in dbcontext.Users
										join hol in dbcontext.Holiday on usr.Id equals hol.Created_By
										where hol.Is_Deleted == null || hol.Is_Deleted == false 
										select  new HolidayList_ViewModel 
										{
											//Id = hol.Id,
											Name = hol.Name,
											From_Date = hol.From_Date,
											To_Date = hol.To_Date,
											Reason = hol.Reason,
											Academic_Year = hol.Academic_Year,
											User_Id = usr.User_Id,
											Created_On = usr.Created_On,
											Created_By = usr.Created_By
										}).ToList();

				
				

				if (holidayListViewModel.Count() == 0)
				{
					holidayListViewModel = null;
				}


			}
			return View(holidayListViewModel);
		}
		

		[HttpPost]
		public ActionResult AddHoliday(string Name, string From_Date , string To_Date , string Year)
		{
			DataTable dt = new DataTable();
				
			int nUser_Id;
			int nYear = Convert.ToInt16( Year);
			using (var dbcontext = new SchoolERPDBContext())
			{
				//nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
				nUser_Id = 5;
			}
			DateTime dtFromDate = DateTime.ParseExact(From_Date, "dd/MM/yyyy", null);
			DateTime dtToDate = DateTime.ParseExact(To_Date, "dd/MM/yyyy", null);
			TimeSpan ts = dtToDate - dtFromDate;
			int nDays = Convert.ToInt16(ts.TotalDays) + 1;

				using (var dbcontext = new SchoolERPDBContext())
				{
				using (var transaction = dbcontext.Database.BeginTransaction())
				{
					try
					{
						for (int nHolidayCount = 0; nHolidayCount < nDays; nHolidayCount++)
						{
							Holiday newHoliday = new Holiday();
							newHoliday.Name = Name;
							newHoliday.Academic_Year = nYear;
							newHoliday.Is_Active = true;
							newHoliday.Created_By = nUser_Id;
							newHoliday.Created_On = DateTime.Now;
							newHoliday.Holiday_Date = dtFromDate.AddDays(nHolidayCount);

							newHoliday.From_Date = dtFromDate;
							newHoliday.To_Date = dtToDate;

							//if (dbcontext.Holiday.Where(a => a.Name.Replace(" ", "").Trim().ToString() == Name.Replace(" ", "").Trim().ToString() && (a.Is_Deleted == false || a.Is_Deleted == null) && a.Academic_Year == nYear).Count() == 0)
							if (dbcontext.Holiday.Where(a => a.From_Date >= newHoliday.Holiday_Date && a.To_Date <= newHoliday.Holiday_Date && (a.Is_Deleted == false || a.Is_Deleted == null) && a.Academic_Year == nYear).Count() == 0)
							{
								dbcontext.Holiday.Add(newHoliday);
								dbcontext.SaveChanges();
								//return Json("OK", JsonRequestBehavior.AllowGet);
							}
							else
							{
								return Json("Holiday Already Exists.", JsonRequestBehavior.AllowGet);
							}

							if (nHolidayCount == (nDays - 1))
							{
								transaction.Commit();
							}
						}
					}
					catch (Exception e)
					{
						transaction.Rollback();
						return Json("Failure", JsonRequestBehavior.AllowGet);
					}
				}
					return Json("OK", JsonRequestBehavior.AllowGet);
				}

			
			

			//return View();
		}
		#endregion

		#region StaffAttendance
		public ActionResult AddStaffAttendance()
		{
			long nYear = GetAcademicYear();
			List<StaffAttendance_ViewModel> staffAttendanceListViewModel = new List<StaffAttendance_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				
				staffAttendanceListViewModel = (from staff in dbcontext.Staff
												join staffAttendance in dbcontext.Staff_Attendance on staff.Staff_Id equals staffAttendance.Staff_Id
												
												where staffAttendance.Is_Deleted == null || staffAttendance.Is_Deleted == false
												select new StaffAttendance_ViewModel
												{
													//Id = hol.Id,
													Id = staffAttendance.Id,
													From_Date = staffAttendance.From_Date,
													To_Date = staffAttendance.To_Date,
													Leave_Reason = staffAttendance.Reason,
													Leave_Days_Taken = 0,
													Academic_Year = nYear,
													Name = staff.First_Name + " " + staff.Last_Name,
													Employee_No = staff.Employee_Id,
													Created_On = staffAttendance.Created_On

												}).ToList();




				if (staffAttendanceListViewModel.Count() == 0)
				{
					staffAttendanceListViewModel = null;
				}


			}
			
			return View(staffAttendanceListViewModel);
			
		}

		public JsonResult GetStaffList(string q)
		{
			return Json(SearchAndGetStaffList(q), JsonRequestBehavior.AllowGet);
			//return Json(new { items = SearchAndGetStaffList(q) }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult AddStaffAttendance(string Staff_Id , string From_Date , string To_Date , string Reason)
		{
			DataTable dt = new DataTable();

			int nUser_Id;
			string sReturnText = string.Empty;
			long nYear = GetAcademicYear();
			using (var dbcontext = new SchoolERPDBContext())
			{
				//nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
				nUser_Id = 5;
			}
			DateTime dtFromDate = DateTime.ParseExact(From_Date, "dd/MM/yyyy", null);
			DateTime dtToDate = DateTime.ParseExact(To_Date, "dd/MM/yyyy", null);
			TimeSpan ts = dtToDate - dtFromDate;
			int nDays = Convert.ToInt16(ts.TotalDays) + 1;

			using (var dbcontext = new SchoolERPDBContext())
			{
				using (var transaction = dbcontext.Database.BeginTransaction())
				{
					try
					{
						for (int nHolidayCount = 0; nHolidayCount < nDays; nHolidayCount++)
						{
							Staff_Attendance newStaffAttendance = new Staff_Attendance();
							newStaffAttendance.Staff_Id = Convert.ToInt16(Staff_Id) ;
							
							newStaffAttendance.Academic_Year = nYear;
							newStaffAttendance.Is_Active = true;
							newStaffAttendance.Created_By = nUser_Id;
							newStaffAttendance.Created_On = DateTime.Now;
							newStaffAttendance.Leave_Date = dtFromDate.AddDays(nHolidayCount);

							newStaffAttendance.From_Date = dtFromDate;
							newStaffAttendance.To_Date = dtToDate;
							newStaffAttendance.Reason = Reason;

							//if (dbcontext.Holiday.Where(a => a.Name.Replace(" ", "").Trim().ToString() == Name.Replace(" ", "").Trim().ToString() && (a.Is_Deleted == false || a.Is_Deleted == null) && a.Academic_Year == nYear).Count() == 0)
							if (dbcontext.Staff_Attendance.Where(x=>x.Staff_Id == newStaffAttendance.Staff_Id && x.Leave_Date ==  newStaffAttendance.Leave_Date && (x.Is_Deleted == null || x.Is_Deleted == null) && x.Academic_Year == nYear).Count() == 0)
							//if (dbcontext.Holiday.Where(a => a.From_Date >= newStaffAttendance.Leave_Date && a.To_Date <= newStaffAttendance.Leave_Date && (a.Is_Deleted == false || a.Is_Deleted == null) && a.Academic_Year == nYear).Count() == 0)
							{	
								dbcontext.Staff_Attendance.Add(newStaffAttendance);
								dbcontext.SaveChanges();
								sReturnText = "OK";
								//return Json("OK", JsonRequestBehavior.AllowGet);
							}
							else
							{
								return Json("Holiday Already Exists.", JsonRequestBehavior.AllowGet);
							}

							if (nHolidayCount == (nDays - 1))
							{
								transaction.Commit();
							}
						}
					}
					catch (Exception e)
					{
						transaction.Rollback();
						return Json("Failure", JsonRequestBehavior.AllowGet);
					}
				}
				return Json(sReturnText, JsonRequestBehavior.AllowGet);
			}
		}
		#endregion



	}
}