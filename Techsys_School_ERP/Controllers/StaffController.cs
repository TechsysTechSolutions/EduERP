//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Web;
//using System.Web.Mvc;
//using Techsys_School_ERP.DBAccess;
//using Techsys_School_ERP.Model;
//using Techsys_School_ERP.Model.ViewModel;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Techsys_School_ERP.DBAccess;
using Techsys_School_ERP.Model;
using Techsys_School_ERP.Model.ViewModel;

namespace Techsys_School_ERP.Controllers
{
    public class StaffController : CommonController
	{
        // GET: Staff
        public ActionResult CreateTeachingStaff()
        {
			GetBloodGroup();
			GetGender();
			GetCountries();
		
			long nStaff_Id ;
			Staff staff = new Staff();
			staff.Academic_Year = GetAcademicYear();
			using (var dbcontext = new SchoolERPDBContext())
			{
				if (dbcontext.Staff.ToList().Count == 0)
				{

					staff.Employee_Id =  Convert.ToString(GetAcademicYear()) + " - " + "1";
					nStaff_Id = 1;
				}
				else
				{
					nStaff_Id = Convert.ToInt64(dbcontext.Staff.Max(x => x.Staff_Id) + 1);
					staff.Employee_Id = Convert.ToString(GetAcademicYear()) + " - " + (nStaff_Id + 1);					
				}

				TempData["Staff_Id"] = nStaff_Id;
				TempData.Keep("Staff_Id");
			}
			return View(staff);
		}


		public ActionResult GetSchoolList(string q)
		{
			var list = new List<SearchList>();

			List<School> schoolList = new List<School>();

			using (var dbcontext = new SchoolERPDBContext())
			{

				if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
				{

					schoolList = (from school in dbcontext.School
								  where school.Name.Contains(q.ToLower())
								  select new
								  {
									  Text = school.Name,
									  Id = school.Id
								  }).ToList().
									   Select(x => new School

									   {

										   Name = x.Text,
										   Id = x.Id

									   }).ToList();

				}
			}


			for (int i = 0; i < schoolList.Count(); i++)
			{
				SearchList oSelect2Model = new SearchList();
				oSelect2Model.text = schoolList[i].Name;
				oSelect2Model.id = schoolList[i].Id.ToString();

				list.Add(oSelect2Model);


			}


			if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
			{
				list = list.Where(x => x.text.ToLower().Contains(q.ToLower())).ToList();
			}
			//return Json(new { data = schoolList }, JsonRequestBehavior.AllowGet);
			return Json(new { items = list }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult CreateStaff(Staff formData)

		{

			Staff newStaffToBeAdded = new Staff();
			int nStaff_Type_Id;
			newStaffToBeAdded = formData;
			//newStaffToBeAdded.Staff_Id = Convert.ToInt32(TempData.Peek("Staff_Id"));
			newStaffToBeAdded.Staff_Id = 3;
			newStaffToBeAdded.Is_Active = true;
			newStaffToBeAdded.Academic_Year = GetAcademicYear();
			//newStudentToBeAdded.Created_By = GetLoggedInUserId();
			newStaffToBeAdded.Created_By = 4;

			using (var dbcontext = new SchoolERPDBContext())
			{
				nStaff_Type_Id = (dbcontext.Staff_Type.Where(x => x.Name == "Teaching").FirstOrDefault()).Id;
			}
			newStaffToBeAdded.Staff_Type_Id = nStaff_Type_Id;
		    newStaffToBeAdded.Created_On = DateTime.Now;
			string sEmail_Id = formData.Email_Id;
			string sReturn_Text = string.Empty;
			try
			{
				if (ModelState.IsValid)
				{
					using (var dbcontext = new SchoolERPDBContext())
					{
						if (dbcontext.Staff.Any(o => o.Email_Id == sEmail_Id))
						{
							sReturn_Text = "Email Already Exists";
						}
						else
						{
							dbcontext.Staff.Add(newStaffToBeAdded);
							dbcontext.SaveChanges();
							sReturn_Text = "SuccessFully Added";
						}
					}
					//return Json(sReturn_Text, JsonRequestBehavior.AllowGet);
				}
				else
				{
					foreach (ModelState modelState in ViewData.ModelState.Values)
					{
						foreach (ModelError error in modelState.Errors)
						{
							sReturn_Text = error.ErrorMessage.ToString();
							break;
						}

						if (sReturn_Text != string.Empty)
						{
							break;
						}

					}


				}
			}
			catch (Exception ex)
			{
				sReturn_Text = "Error";
				//return Json(sReturn_Text, JsonRequestBehavior.AllowGet);
			}

			return Json(sReturn_Text, JsonRequestBehavior.AllowGet);

		}

		[HttpPost]
		public ActionResult EditTeachingStaff(Staff staff)
		{
			string sReturnText = string.Empty;
			try
			{
				if (ModelState.IsValid)
				{
					using (var dbcontext = new SchoolERPDBContext())
					{
						Staff staffToBeUpdated = dbcontext.Staff.Find(staff.Staff_Id);

						staffToBeUpdated.First_Name = staff.First_Name;
						staffToBeUpdated.Employee_Id = staff.Employee_Id;
						staffToBeUpdated.Date_Of_Joining = staff.Date_Of_Joining;
						staffToBeUpdated.Last_Name = staff.Last_Name;
						staffToBeUpdated.Middle_Name = staff.Middle_Name;
						staffToBeUpdated.Father_Name = staff.Father_Name;
						staffToBeUpdated.Is_Married = staff.Is_Married;
						staffToBeUpdated.Aadhar_Number = staff.Aadhar_Number;
						staffToBeUpdated.Address_Line1 = staff.Address_Line1;
						staffToBeUpdated.Address_Line2 = staff.Address_Line2;
						staffToBeUpdated.Blood_Group_Id = staff.Blood_Group_Id;
						staffToBeUpdated.Mobile_No = staff.Mobile_No;
						staffToBeUpdated.Alt_Mobile_No = staff.Alt_Mobile_No;
						staffToBeUpdated.Email_Id = staff.Email_Id;
						staffToBeUpdated.DOB = staff.DOB;
						staffToBeUpdated.Academic_Year = GetAcademicYear();
						staffToBeUpdated.City_Id = staff.City_Id;
						staffToBeUpdated.State_Id = staff.State_Id;
						staffToBeUpdated.Country_Id = staff.Country_Id;
						staffToBeUpdated.Blood_Group_Id = staff.Blood_Group_Id;
						staffToBeUpdated.Experience_in_Years = staff.Experience_in_Years;
						staffToBeUpdated.Handling_Subjects = staff.Handling_Subjects;
						//staffToBeUpdated.Is_HostelStudent = staff.Is_HostelStudent;
						//staffToBeUpdated.Phone_No1 = staff.Phone_No1;
						//staffToBeUpdated.Phone_No2 = staff.Phone_No2;
						//staffToBeUpdated.Section_Id = staff.Section_Id;
						//staffToBeUpdated.Class_Id = staff.Class_Id;
						staffToBeUpdated.Gender_Id = staff.Gender_Id;
						staffToBeUpdated.PinCode = staff.PinCode;
						staffToBeUpdated.Updated_On = DateTime.Now;
						staffToBeUpdated.Updated_By = 5;


						if (staff.Email_Id.Trim() != staffToBeUpdated.Email_Id.Trim())
						{
							if (!CheckIfEmailAlreadyExists(staff.Email_Id, true, false))
							{

								dbcontext.Entry(staffToBeUpdated).State = EntityState.Modified;
								dbcontext.SaveChanges();
								sReturnText = "OK";
							}
							else
							{
								sReturnText = "Email Already Exists";
							}
						}
						else
						{
							dbcontext.Entry(staffToBeUpdated).State = EntityState.Modified;
							dbcontext.SaveChanges();
							sReturnText = "OK";
						}
					}
				}
			}
			catch (Exception ex)
			{
				sReturnText = ex.InnerException.Message.ToString();
			}

			return Json(sReturnText, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Upload(HttpPostedFileBase file)
		{
			long nStaff_Id = Convert.ToInt64(TempData.Peek("Staff_Id"));
			Staff staffToBeModified = new Staff();

			byte[] bytes;
			try
			{
				if (file != null)
				{
					using (BinaryReader br = new BinaryReader(file.InputStream))
					{
						bytes = br.ReadBytes(file.ContentLength);
					}


					using (var dbcontext = new SchoolERPDBContext())
					{
						staffToBeModified = dbcontext.Staff.Find(nStaff_Id);
						staffToBeModified.Photo = bytes;
						dbcontext.Entry(staffToBeModified).CurrentValues.SetValues(staffToBeModified);
						dbcontext.SaveChanges();
					}


				}
				return Json("Student Details Successfully Added", JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);

			}

			return null;

		}

		public ActionResult AddStaffEducationalAndWorkExpDetails()
		{

			GetSubjects();
			GetDesignation();
			return View();
		}

		public ActionResult AddStaffHandlingSubjectsAndUploadNecessaryDocuments()
		{
			return View();
		}

		public ActionResult GetInstitutionList(string q)

		{
			var list = new List<SearchList>();

			List<Institution> institutionList = new List<Institution>();

			using (var dbcontext = new SchoolERPDBContext())
			{

				if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
				{

					institutionList = (from inst in dbcontext.Institution
								  where inst.Name.Contains(q.ToLower())
								  select new
								  {
									  Text = inst.Name,
									  Id = inst.Id
								  }).ToList().
								Select(x => new Institution

									   {

										   Name = x.Text,
										   Id = x.Id

									   }).ToList();

				}
			}


			for (int i = 0; i < institutionList.Count(); i++)
			{
				SearchList oSearchList = new SearchList();
				oSearchList.text = institutionList[i].Name;
				oSearchList.id = institutionList[i].Id.ToString();

				list.Add(oSearchList);


			}


			if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
			{
				list = list.Where(x => x.text.ToLower().Contains(q.ToLower())).ToList();
			}
			//return Json(new { data = schoolList }, JsonRequestBehavior.AllowGet);
			return Json(new { items = list }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetQualificationList(string q)
		{
			var list = new List<SearchList>();

			List<Qualification> qualificationList = new List<Qualification>();

			using (var dbcontext = new SchoolERPDBContext())
			{

				if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
				{

					qualificationList = (from qual in dbcontext.Qualification
									   where qual.Name.Contains(q.ToLower())
									   select new
									   {
										   Text = qual.Name,
										   Id = qual.Id
									   }).ToList().
								Select(x => new Qualification

								{

									Name = x.Text,
									Id = x.Id

								}).ToList();

				}
			}


			for (int i = 0; i < qualificationList.Count(); i++)
			{
				SearchList oSearchList = new SearchList();
				oSearchList.text = qualificationList[i].Name;
				oSearchList.id = qualificationList[i].Id.ToString();

				list.Add(oSearchList);


			}


			if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
			{
				list = list.Where(x => x.text.ToLower().Contains(q.ToLower())).ToList();
			}
			//return Json(new { data = schoolList }, JsonRequestBehavior.AllowGet);
			return Json(new { items = list }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetSpecializationList(string q)
		{
			var list = new List<SearchList>();

			List<Specialization> specializationList = new List<Specialization>();

			using (var dbcontext = new SchoolERPDBContext())
			{

				if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
				{

					specializationList = (from specialization in dbcontext.Specialization
										 where specialization.Name.Contains(q.ToLower())
										 select new
										 {
											 Text = specialization.Name,
											 Id = specialization.Id
										 }).ToList().
								Select(x => new Specialization

								{

									Name = x.Text,
									Id = x.Id

								}).ToList();

				}
			}


			for (int i = 0; i < specializationList.Count(); i++)
			{
				SearchList oSearchList = new SearchList();
				oSearchList.text = specializationList[i].Name;
				oSearchList.id = specializationList[i].Id.ToString();

				list.Add(oSearchList);


			}


			if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
			{
				list = list.Where(x => x.text.ToLower().Contains(q.ToLower())).ToList();
			}
			//return Json(new { data = schoolList }, JsonRequestBehavior.AllowGet);
			return Json(new { items = list }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult CreateNonTeachingStaff()
		{
			return View();
		}


		#region DeleteStudent

		public ActionResult Delete(long id)
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var staffToBeDeleted = dbcontext.Staff.Find(id);
				staffToBeDeleted.Is_Deleted = true;
				staffToBeDeleted.Is_Active = true;
				staffToBeDeleted.Updated_By = 5;
				staffToBeDeleted.Updated_On = DateTime.Now;

				dbcontext.Entry(staffToBeDeleted).State = EntityState.Modified;
				dbcontext.SaveChanges();

			}
			return RedirectToAction("StaffList");
		}

		#endregion

		[HttpPost]
		public JsonResult FetchStateForCountry(string Country_Id)
		{
			List<State> lstState = GetStatesForCountry(Country_Id);

			return Json(lstState, JsonRequestBehavior.AllowGet);

		}

		public ActionResult populateCountriesList(string sSearchTerm)
		{
			return Json(new { items = GetCountriesList(sSearchTerm) }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GetStaffEducationalDetailsOnPageLoad()
		{
			List<StaffEduList_ViewModel> addedStaffEducationalDetails = new List<StaffEduList_ViewModel>();

			addedStaffEducationalDetails = GetStaffEducationalDetails();

			return Json(new { items = addedStaffEducationalDetails }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GetStaffWorkExpOnPageLoad()
		{
			List<StaffWorkExp_ViewModel> addedStaffWorkExpDetails = new List<StaffWorkExp_ViewModel>();

			addedStaffWorkExpDetails = GetStaffWorkExpDetails();

			return Json(new { items = addedStaffWorkExpDetails }, JsonRequestBehavior.AllowGet);

		}


		[HttpPost]
		public JsonResult FetchCitiesForState(string State_Id)
		{
			List<City> lstCity = GetCitiesForState(State_Id);

			return Json(lstCity, JsonRequestBehavior.AllowGet);

		}

		[HttpPost]
		public JsonResult AddStaffEducationalQualification(Staff_Educational_Details staffEducationalDetail)
		{
			staffEducationalDetail.Created_On = DateTime.Now;
			staffEducationalDetail.Created_By = 5;
			staffEducationalDetail.Academic_Year = GetAcademicYear();
			staffEducationalDetail.Is_Active = true;
			staffEducationalDetail.Staff_Id = Convert.ToInt16(TempData.Peek("Staff_Id"));
			List<StaffEduList_ViewModel> addedStudentPrevSchoolDetails = new List<StaffEduList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				dbcontext.Staff_Educational_Details.Add(staffEducationalDetail);
				dbcontext.SaveChanges();

			}

			addedStudentPrevSchoolDetails = GetStaffEducationalDetails();
			return Json(new { items = addedStudentPrevSchoolDetails }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult AddStaffWorkExpDetail(Staff_Exp_Details Staff_Exp_Details)
		{
			Staff_Exp_Details.Created_On = DateTime.Now;
			Staff_Exp_Details.Created_By = 5;
			Staff_Exp_Details.Academic_Year = GetAcademicYear();
			Staff_Exp_Details.Is_Active = true;
			//Staff_Exp_Details.Staff_Id = 5;
			Staff_Exp_Details.Staff_Id = Convert.ToInt16(TempData.Peek("Staff_Id"));
		
			List<StaffWorkExp_ViewModel> addedStaffWorkExpDetails = new List<StaffWorkExp_ViewModel>();
			
			
			using (var dbcontext = new SchoolERPDBContext())
			{
				dbcontext.Staff_Exp_Details.Add(Staff_Exp_Details);
				dbcontext.SaveChanges();
			}
			addedStaffWorkExpDetails = GetStaffWorkExpDetails();
			return Json(new { items = addedStaffWorkExpDetails }, JsonRequestBehavior.AllowGet);
		}

		public List<StaffEduList_ViewModel> GetStaffEducationalDetails()
		{
			List<StaffEduList_ViewModel> addedStaffEducationalDetails = new List<StaffEduList_ViewModel>();
			try
			{
				long nStaff_Id = Convert.ToInt64(TempData.Peek("Staff_Id"));
				long nAcademicYear = GetAcademicYear();
				
				using (var dbcontext = new SchoolERPDBContext())
				{
					addedStaffEducationalDetails = (from inst in dbcontext.Institution
													 join staffEduDetail in dbcontext.Staff_Educational_Details on inst.Id equals staffEduDetail.Institution_Id
													 join qual in dbcontext.Qualification on staffEduDetail.Qualification_Id equals qual.Id
													 join specialization in dbcontext.Specialization on staffEduDetail.Specialization_Id equals specialization.Id
													 where staffEduDetail.Staff_Id == nStaff_Id && staffEduDetail.Academic_Year == nAcademicYear && staffEduDetail.Is_Deleted == null || staffEduDetail.Is_Deleted == false
													 select new StaffEduList_ViewModel
													 {
														 Id = staffEduDetail.Id,
														 Institution = inst.Name,
														 Qualification = qual.Name,
														Specialization = specialization.Name,
														 Year_Of_Passing = staffEduDetail.Year_Of_Passing

													 }).ToList();
				}
			}
			catch (Exception ex)
			{
			}

			return addedStaffEducationalDetails;
		}

		[HttpPost]
		public JsonResult DeleteStaffEducationDetail(string id)
		{
			int idTobemodified = Convert.ToInt16(id);
			List<StaffEduList_ViewModel> modifiedStaffEduList = new List<StaffEduList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				var studentEducationDetailToBedeleted = dbcontext.Staff_Educational_Details.Find(idTobemodified);
				studentEducationDetailToBedeleted.Is_Deleted = true;
				studentEducationDetailToBedeleted.Updated_On = DateTime.Now;
				studentEducationDetailToBedeleted.Updated_By = 5;
				dbcontext.Entry(studentEducationDetailToBedeleted).CurrentValues.SetValues(studentEducationDetailToBedeleted);
				dbcontext.SaveChanges();
			}
			modifiedStaffEduList = GetStaffEducationalDetails();
			return Json(new { items = modifiedStaffEduList }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult DeleteStaffWorkExpDetail(string id)
		{
			int idTobemodified = Convert.ToInt16(id);
			List<StaffWorkExp_ViewModel> modifiedStaffWorkExp = new List<StaffWorkExp_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				var staffWorkExpDetailTobedeleted = dbcontext.Staff_Exp_Details.Find(idTobemodified);
				staffWorkExpDetailTobedeleted.Is_Deleted = true;
				staffWorkExpDetailTobedeleted.Updated_On = DateTime.Now;
				staffWorkExpDetailTobedeleted.Updated_By = 5;
				dbcontext.Entry(staffWorkExpDetailTobedeleted).CurrentValues.SetValues(staffWorkExpDetailTobedeleted);
				dbcontext.SaveChanges();
			}
			modifiedStaffWorkExp = GetStaffWorkExpDetails();
			return Json(new { items = modifiedStaffWorkExp }, JsonRequestBehavior.AllowGet);
		}

		public List<StaffWorkExp_ViewModel> GetStaffWorkExpDetails()
		{
			List<StaffWorkExp_ViewModel> addedStaffWorkExpDetails = new List<StaffWorkExp_ViewModel>();
			try
			{
				long nStaff_Id = Convert.ToInt64(TempData.Peek("Staff_Id"));
				//long nStaff_Id = 5;
				long nAcademicYear = GetAcademicYear();

				using (var dbcontext = new SchoolERPDBContext())
				{
					addedStaffWorkExpDetails = (from school in dbcontext.School
												join staffWorkExpDetail in dbcontext.Staff_Exp_Details on school.Id equals staffWorkExpDetail.School_Id
												join desgn in dbcontext.Designation on staffWorkExpDetail.Designation_Id equals desgn.Id
												//join sub in dbcontext.Subject on staffWorkExpDetail.Subject_Id equals specialization.Id
												where staffWorkExpDetail.Staff_Id == nStaff_Id && staffWorkExpDetail.Academic_Year == nAcademicYear && (staffWorkExpDetail.Is_Deleted == null || staffWorkExpDetail.Is_Deleted == false)
												select new StaffWorkExp_ViewModel
												{
													Id = staffWorkExpDetail.Id,
													Institution = school.Name,
													Designation = desgn.Name,
													Subject = staffWorkExpDetail.Subject_Id,
													Year_Of_Passing = staffWorkExpDetail.From_Year

												}).ToList();
				}
			}
			catch (Exception ex)
			{
			}

		//	return Json(new { data = addedStaffWorkExpDetails }, JsonRequestBehavior.AllowGet);

		return addedStaffWorkExpDetails;
		}

		public ActionResult AddOrEditStaffSalaryDetail()
		{
			long nStaff_Id = Convert.ToInt64(TempData.Peek("Staff_Id"));
			long nAcademic_Year = GetAcademicYear();


			using (var dbcontext = new SchoolERPDBContext())
			{
				if (dbcontext.Staff_Salary_Detail.Where(x => x.Staff_Id == nStaff_Id && x.Academic_Year == nAcademic_Year && (x.Is_Deleted == null || x.Is_Deleted ==false)) .ToList().Count() == 0)
				{
					return RedirectToAction("AddStaffSalaryDetail");

				}
				else
				{
					return RedirectToAction("EditStaffSalaryDetail");
				}
			}

		}

		public ActionResult AddStaffSalaryDetail()
		{
			StaffSalary_ViewModel staffSalary_ViewModel = new StaffSalary_ViewModel();
			long nStaff_Id = Convert.ToInt64(TempData.Peek("Staff_Id")); ;
			using (var dbcontext = new SchoolERPDBContext())
			{
				var existingStaffDetail = dbcontext.Staff.Find(nStaff_Id);

				staffSalary_ViewModel.Staff_Name = existingStaffDetail.First_Name + " " + existingStaffDetail.Last_Name;
				staffSalary_ViewModel.Employee_No = existingStaffDetail.Employee_Id;
				staffSalary_ViewModel.Academic_Year = GetAcademicYear();
				staffSalary_ViewModel.Staff_Id = Convert.ToInt32(nStaff_Id);
			}
			return View(staffSalary_ViewModel);
		}

		[HttpPost]
		public ActionResult EditStaffSalaryDetail(Staff_Salary_Detail staffSalaryDetail)
		{

			StaffSalary_ViewModel staffSalary_ViewModel = new StaffSalary_ViewModel();
			string sReturnText = string.Empty;
			try
			{
				long nStaff_Id = Convert.ToInt64(TempData.Peek("Staff_Id")); ;
				long nAcademicYear = GetAcademicYear();
				using (var dbcontext = new SchoolERPDBContext())
				{
					var existingStaffDetail = dbcontext.Staff.Find(nStaff_Id);

					staffSalary_ViewModel.Staff_Name = existingStaffDetail.First_Name + " " + existingStaffDetail.Last_Name;
					staffSalary_ViewModel.Employee_No = existingStaffDetail.Employee_Id;
					staffSalary_ViewModel.Academic_Year = GetAcademicYear();

					if (dbcontext.Staff_Salary_Detail.Where(x => x.Staff_Id == nStaff_Id && x.Academic_Year == nAcademicYear && (x.Is_Deleted == false || x.Is_Deleted == null)).ToList().Count() > 0)
					{
						var staffSalaryDetailId_ToBeEdited = dbcontext.Staff_Salary_Detail.Where(x => x.Staff_Id == nStaff_Id && x.Academic_Year == nAcademicYear && (x.Is_Deleted == false || x.Is_Deleted == null)).FirstOrDefault().Id;
						var staffSalaryDetailToBeEdited = dbcontext.Staff_Salary_Detail.Find(staffSalaryDetailId_ToBeEdited);
						staffSalaryDetailToBeEdited.Bank_Account_No = staffSalaryDetail.Bank_Account_No;
						staffSalaryDetailToBeEdited.Bank_AddressLine1 = staffSalaryDetail.Bank_AddressLine1;
						staffSalaryDetailToBeEdited.Bank_AddressLine2 = staffSalaryDetail.Bank_AddressLine2;
						staffSalaryDetailToBeEdited.Bank_Name = staffSalaryDetail.Bank_Name;
						staffSalaryDetailToBeEdited.Basic = staffSalaryDetail.Basic;
						staffSalaryDetailToBeEdited.Branch_Name = staffSalaryDetail.Branch_Name;
						staffSalaryDetailToBeEdited.City = staffSalaryDetail.City;
						staffSalaryDetailToBeEdited.Conveyance = staffSalaryDetail.Conveyance;
						staffSalaryDetailToBeEdited.DA = staffSalaryDetail.DA;
						staffSalaryDetailToBeEdited.ESIC = staffSalaryDetail.ESIC;
						staffSalaryDetailToBeEdited.Gross_Salary = staffSalaryDetail.Gross_Salary;
						staffSalaryDetailToBeEdited.HRA = staffSalaryDetail.HRA;
						staffSalaryDetailToBeEdited.LTA = staffSalaryDetail.LTA;
						staffSalaryDetailToBeEdited.Medical = staffSalaryDetail.Medical;
						staffSalaryDetailToBeEdited.Net_Salary = staffSalaryDetail.Net_Salary;
						staffSalaryDetailToBeEdited.Other = staffSalaryDetail.Other;
						staffSalaryDetailToBeEdited.Other_Deductions = staffSalaryDetail.Other_Deductions;
						staffSalaryDetailToBeEdited.PAN_No = staffSalaryDetail.PAN_No;
						staffSalaryDetailToBeEdited.PF_Account_No = staffSalaryDetail.PF_Account_No;
						staffSalaryDetailToBeEdited.Professional_Tax = staffSalaryDetail.Professional_Tax;
						staffSalaryDetailToBeEdited.Provident_Fund = staffSalaryDetail.Provident_Fund;
						dbcontext.Entry(staffSalaryDetailToBeEdited).State = EntityState.Modified;
						dbcontext.SaveChanges();

					}
					else
					{
						staffSalaryDetail.Created_By = 5;
						staffSalaryDetail.Created_On = DateTime.Now;
						dbcontext.Staff_Salary_Detail.Add(staffSalaryDetail);
						dbcontext.SaveChanges();


					}
					sReturnText = "OK";
				}
			}
			catch (Exception ex)
			{
				sReturnText = ex.InnerException.Message.ToString();
			}

				return Json(sReturnText.ToString(), JsonRequestBehavior.AllowGet);
			

		}

		public ActionResult EditStaffSalaryDetail()
		{
			StaffSalary_ViewModel staffSalary_ViewModel = new StaffSalary_ViewModel();
			long nStaff_Id = Convert.ToInt64(TempData.Peek("Staff_Id")); ;
			long nAcademicYear = GetAcademicYear();
			using (var dbcontext = new SchoolERPDBContext())
			{
				var existingStaffDetail = dbcontext.Staff.Find(nStaff_Id);

				staffSalary_ViewModel.Staff_Name = existingStaffDetail.First_Name + " " + existingStaffDetail.Last_Name;
				staffSalary_ViewModel.Employee_No = existingStaffDetail.Employee_Id;
				staffSalary_ViewModel.Academic_Year = GetAcademicYear();

				if (dbcontext.Staff_Salary_Detail.Where(x => x.Staff_Id == nStaff_Id && x.Academic_Year == nAcademicYear && ( x.Is_Deleted == false || x.Is_Deleted == null)).ToList().Count() > 0)
				{
					var staffSalaryDetailToBeEdited = dbcontext.Staff_Salary_Detail.Where(x => x.Staff_Id == nStaff_Id && x.Academic_Year == nAcademicYear && (x.Is_Deleted == false || x.Is_Deleted == null)).FirstOrDefault();
					staffSalary_ViewModel.Bank_Account_No = staffSalaryDetailToBeEdited.Bank_Account_No;
					staffSalary_ViewModel.Bank_AddressLine1 = staffSalaryDetailToBeEdited.Bank_AddressLine1;
					staffSalary_ViewModel.Bank_AddressLine2 = staffSalaryDetailToBeEdited.Bank_AddressLine2;
					staffSalary_ViewModel.Bank_Name = staffSalaryDetailToBeEdited.Bank_Name;
					staffSalary_ViewModel.Basic = staffSalaryDetailToBeEdited.Basic;
					staffSalary_ViewModel.Branch_Name = staffSalaryDetailToBeEdited.Branch_Name;
					staffSalary_ViewModel.City = staffSalaryDetailToBeEdited.City;
					staffSalary_ViewModel.Conveyance = staffSalaryDetailToBeEdited.Conveyance;
					staffSalary_ViewModel.DA = staffSalaryDetailToBeEdited.DA;
					staffSalary_ViewModel.ESIC = staffSalaryDetailToBeEdited.ESIC;
					staffSalary_ViewModel.Gross_Salary = staffSalaryDetailToBeEdited.Gross_Salary;
					staffSalary_ViewModel.HRA = staffSalaryDetailToBeEdited.HRA;
					staffSalary_ViewModel.LTA = staffSalaryDetailToBeEdited.LTA;
					staffSalary_ViewModel.Medical = staffSalaryDetailToBeEdited.Medical;
					staffSalary_ViewModel.Net_Salary = staffSalaryDetailToBeEdited.Net_Salary;
					staffSalary_ViewModel.Other = staffSalaryDetailToBeEdited.Other;
					staffSalary_ViewModel.Other_Deductions = staffSalaryDetailToBeEdited.Other_Deductions;
					staffSalary_ViewModel.PAN_No = staffSalaryDetailToBeEdited.PAN_No;
					staffSalary_ViewModel.PF_Account_No = staffSalaryDetailToBeEdited.PF_Account_No;
					staffSalary_ViewModel.Professional_Tax = staffSalaryDetailToBeEdited.Professional_Tax;
					staffSalary_ViewModel.Provident_Fund = staffSalaryDetailToBeEdited.Provident_Fund;
				
				}
			}
			return View(staffSalary_ViewModel);
		}

		[HttpPost]
		public JsonResult AddStaffSalaryDetail(Staff_Salary_Detail staffSalaryDetail)
		{
			staffSalaryDetail.Academic_Year = GetAcademicYear();
			staffSalaryDetail.Created_By = 5;
			staffSalaryDetail.Created_On = DateTime.Now;
			staffSalaryDetail.Is_Active = true;
			string sReturnText = string.Empty;
			try
			{
				using (var dbcontext = new SchoolERPDBContext())
				{
					dbcontext.Staff_Salary_Detail.Add(staffSalaryDetail);
					dbcontext.SaveChanges();
					sReturnText = "OK";
				}
			}
			catch (Exception ex)
			{
				sReturnText = ex.Message.ToString();
			}
			return Json(sReturnText.ToString(), JsonRequestBehavior.AllowGet);
		}

		public ActionResult GenerateSalarySlipsForStaff()
		{

			return null;

		}

		public ActionResult StaffList()
		{
			long nYear = GetAcademicYear();
			List<Staff_ViewModel> staffVieModelList = new List<Staff_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				staffVieModelList = (from staff in dbcontext.Staff
									    where staff.Academic_Year == nYear && (staff.Is_Deleted == false || staff.Is_Deleted == null)
									   select new Staff_ViewModel
									   {

										   Id = staff.Staff_Id,
										   Name = staff.First_Name + " " + staff.Last_Name + " " + staff.Middle_Name,
										   Employee_No = staff.Employee_Id,
										   Emergency_ContactNo = staff.Mobile_No,
										   Academic_Year = nYear,
										   Created_On = staff.Created_On



									   }).OrderBy(x => x.Created_On).ToList();
			}
			return View(staffVieModelList);
		}

		#region SalarySlipGeneration
		public ActionResult GenerateSalarySlips()
		{
			
		
			
			int nStaffCount = 0;
			long nYear = GetAcademicYear();
			using (var dbcontext = new SchoolERPDBContext())
			{
				nStaffCount = dbcontext.Staff.Where(x => x.Academic_Year == nYear && (x.Is_Deleted == null || x.Is_Deleted == false)).Count();

				List<Staff_Salary_Detail> staffSalaryDetailList = new List<Staff_Salary_Detail>();

				staffSalaryDetailList = dbcontext.Staff_Salary_Detail.Where(x => x.Academic_Year == nYear && ( x.Is_Deleted == false)).ToList();


				for (int nLoopCount = 0; nLoopCount < staffSalaryDetailList.Count(); nLoopCount++)
				{
					Staff_Salary_Detail staffSalaryDetail = new Staff_Salary_Detail();
					staffSalaryDetail = dbcontext.Staff_Salary_Detail.ToList()[nLoopCount];


					Staff staff = new Staff();
					staff = dbcontext.Staff.Where(x => x.Staff_Id == staffSalaryDetail.Staff_Id).FirstOrDefault();

					int nNo_Of_Leaves_Taken = dbcontext.Staff_Attendance.Where(x => x.Staff_Id == staffSalaryDetail.Staff_Id && x.Academic_Year == nYear).Count();

					System.IO.FileStream fs;
					Document pdfDoc;
					pdfDoc = new Document(PageSize.A2, 0f, 0f, 80f, 30f);

					string severFilePath = Server.MapPath("~/views//billing//");

					if (!Directory.Exists(severFilePath))
					{ // if it doesn't exist, create

						System.IO.Directory.CreateDirectory(severFilePath);
					}

					fs = new FileStream(severFilePath + "//" + "" + "_" + "SalarySlip"+ staff.Staff_Id + "_" + "" + "_Triplicate" + ".pdf", FileMode.Create);
					PdfWriter writer = PdfWriter.GetInstance(pdfDoc, fs);

					writer.CloseStream = false;


					iTextSharp.text.Font NormalFont = FontFactory.GetFont("Arial", 12, BaseColor.BLUE);
					Paragraph paragraph = new Paragraph("                                                                                          INVOICE                                                                                               (Original)");
					pdfDoc.Open();
				//	pdfDoc.Add(paragraph);

					/********************************************************************************************************************************************************************************
					 *                                                                              SALARY SLIP DESIGN 
					 * *******************************************************************************************************************************************************************************/
					PdfPTable headingTable = new PdfPTable(1);

					PdfPCell headingTableCell = new PdfPCell(new Phrase("SALARY SLIP FOR THE MONTH" + "- Month_Name" + " - " + Convert.ToString(GetAcademicYear()), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					headingTableCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
					headingTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
					headingTableCell.PaddingTop = 45f;
					headingTableCell.PaddingBottom = 45f;
					headingTable.AddCell(headingTableCell);

					/*****************************************************************************************LOGO DESIGN *******************************************************************************/
					PdfPTable logoTable = new PdfPTable(2);


					string startupPath = AppDomain.CurrentDomain.BaseDirectory;

					string targetPath = startupPath + "Content\\images\\imageupload_-_Copy.png";



					iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(targetPath);
					//Resize image depend upon your need
					jpg.ScaleToFit(200f, 60f);
					//Give space before image
					jpg.SpacingBefore = 20f;
					//jpg.SpacingBefore = 150f;
					//Give some space after the image
					jpg.SpacingAfter = 20f;
					jpg.Alignment = Element.ALIGN_CENTER;


					PdfPCell cellImage = new PdfPCell(jpg);
					cellImage.PaddingLeft = 2f;
					//cellImage.Border = iTextSharp.text.Rectangle.NO_BORDER;


					PdfPCell logoCell1 = new PdfPCell();

					logoTable.AddCell(cellImage);
					logoTable.AddCell(logoCell1);

					/*****************************************************************************************LOGO DESIGN *******************************************************************************/

					PdfPTable staffDetailTable = new PdfPTable(4);
					float[] staffDetailTablewidths = new float[] { 200f,150f, 200f,150f };
					staffDetailTable.SetWidths(staffDetailTablewidths);
					/****************************************************FIRST TABLE (EMPLOYEE DETAILS ********************************************************************************************************************************/
					PdfPCell staffDetailTableCell11 = new PdfPCell(new Phrase("STAFF NAME   ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffDetailTableCell11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffDetailTableCell11.PaddingTop = 15f;
					staffDetailTableCell11.PaddingBottom = 15f;

					PdfPCell staffDetailTableCell11a = new PdfPCell(new Phrase( staff.First_Name + " " + staff.Last_Name, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffDetailTableCell11a.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffDetailTableCell11a.PaddingTop = 15f;
					staffDetailTableCell11a.PaddingBottom = 15f;

					PdfPCell staffDetailTableCell12 = new PdfPCell(new Phrase("EMPLOYEE NO  ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffDetailTableCell12.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffDetailTableCell12.PaddingTop = 15f;
					staffDetailTableCell12.PaddingBottom = 15f;

					PdfPCell staffDetailTableCell12a = new PdfPCell(new Phrase( staff.Employee_Id, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffDetailTableCell12a.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffDetailTableCell12a.PaddingTop = 15f;
					staffDetailTableCell12a.PaddingBottom = 15f;

					PdfPCell staffDetailTableCell21 = new PdfPCell(new Phrase("PF ACCOUNT NO " , new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffDetailTableCell21.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffDetailTableCell21.PaddingTop = 15f;
					staffDetailTableCell21.PaddingBottom = 15f;

					PdfPCell staffDetailTableCell21a = new PdfPCell(new Phrase(staffSalaryDetail.PF_Account_No, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffDetailTableCell21a.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffDetailTableCell21a.PaddingTop = 15f;
					staffDetailTableCell21a.PaddingBottom = 15f;

					PdfPCell staffDetailTableCell22 = new PdfPCell(new Phrase("PAN NO "  , new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffDetailTableCell22.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffDetailTableCell22.PaddingTop = 15f;
					staffDetailTableCell22.PaddingBottom = 15f;

					PdfPCell staffDetailTableCell22a = new PdfPCell(new Phrase(staffSalaryDetail.PAN_No, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffDetailTableCell22a.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffDetailTableCell22a.PaddingTop = 15f;
					staffDetailTableCell22a.PaddingBottom = 15f;

					PdfPCell staffDetailTableCell31 = new PdfPCell(new Phrase("NO OF LEAVES  ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffDetailTableCell31.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell21.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
					staffDetailTableCell31.PaddingTop = 15f;
					staffDetailTableCell31.PaddingBottom = 15f;

					PdfPCell staffDetailTableCell31a = new PdfPCell(new Phrase(Convert.ToString(nNo_Of_Leaves_Taken), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffDetailTableCell31a.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell21.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
					staffDetailTableCell31a.PaddingTop = 15f;
					staffDetailTableCell31a.PaddingBottom = 15f;

					PdfPCell staffDetailTableCell32 = new PdfPCell(new Phrase("WORKING DAYS" , new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffDetailTableCell32.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
					staffDetailTableCell32.PaddingTop = 15f;
					staffDetailTableCell32.PaddingBottom = 15f;

					PdfPCell staffDetailTableCell32a = new PdfPCell(new Phrase( "5 ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffDetailTableCell32a.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
					staffDetailTableCell32a.PaddingTop = 15f;
					staffDetailTableCell32a.PaddingBottom = 15f;

					staffDetailTable.AddCell(staffDetailTableCell11);
					staffDetailTable.AddCell(staffDetailTableCell11a);
					staffDetailTable.AddCell(staffDetailTableCell12);
					staffDetailTable.AddCell(staffDetailTableCell12a);
					staffDetailTable.AddCell(staffDetailTableCell21);
					staffDetailTable.AddCell(staffDetailTableCell21a);
					staffDetailTable.AddCell(staffDetailTableCell22);
					staffDetailTable.AddCell(staffDetailTableCell22a);
					staffDetailTable.AddCell(staffDetailTableCell31);
					staffDetailTable.AddCell(staffDetailTableCell31a);
					staffDetailTable.AddCell(staffDetailTableCell32);
					staffDetailTable.AddCell(staffDetailTableCell32a);


					PdfPTable bankAccountDetailTable = new PdfPTable(2);

					PdfPCell bankAccountDetailTable11 = new PdfPCell(new Phrase("BANK ACCOUNT NO"+ "\n" + "\n" + staffSalaryDetailList[0].Bank_Account_No, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					bankAccountDetailTable11.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
					//staffDetailTableCell22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
					bankAccountDetailTable11.PaddingTop = 15f;
					bankAccountDetailTable11.PaddingBottom = 15f;

					PdfPCell bankAccountDetailTable12 = new PdfPCell(new Phrase(staffSalaryDetailList[0].Bank_Name + "hciohOCOZGCHozOC" + "\n" + staffSalaryDetailList[0].Branch_Name + "\n" + staffSalaryDetailList[0].Bank_AddressLine1 + " - " + staffSalaryDetailList[0].City, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					bankAccountDetailTable12.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
					//staffDetailTableCell22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
					bankAccountDetailTable12.PaddingTop = 15f;
					bankAccountDetailTable12.PaddingBottom = 15f;

					//Phrase BankNamePhrase = new Phrase("\n" + staffSalaryDetailList[0].Bank_Account_No, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black)));
					//bankAccountDetailTable11.AddElement(BankNamePhrase);

					bankAccountDetailTable.AddCell(bankAccountDetailTable11);
					bankAccountDetailTable.AddCell(bankAccountDetailTable12);




				/*******************************************************************SALARY COMPONENT TABLE ****************************************************************************************/
				PdfPTable staffSalaryComponentTable = new PdfPTable(3);

					float[] widths = new float[] { 40f, 400f ,100f};
					staffSalaryComponentTable.SetWidths(widths);
					PdfPCell staffSalaryComponentTableCell11 = new PdfPCell(new Phrase("S.NO", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffSalaryComponentTableCell11.Width = 20f;
					//staffSalaryComponentTableCell11.Border = iTextSharp.text.Rectangle.top;
					staffSalaryComponentTableCell11.PaddingTop = 55f;
					staffSalaryComponentTableCell11.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell12 = new PdfPCell(new Phrase("SALARY COMPONENT", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell12.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell12.PaddingTop = 55f;
					staffSalaryComponentTableCell12.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell13 = new PdfPCell(new Phrase("AMOUNT", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell13.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell13.PaddingTop = 55f;
					staffSalaryComponentTableCell13.PaddingBottom = 15f;

					//BASIC 
					PdfPCell staffSalaryComponentTableCell21 = new PdfPCell(new Phrase("1.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell21.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell21.PaddingTop = 15f;
					staffSalaryComponentTableCell21.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell22 = new PdfPCell(new Phrase("Basic", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell22.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell22.PaddingTop = 15f;
					staffSalaryComponentTableCell22.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell23 = new PdfPCell(new Phrase(Convert.ToString(staffSalaryDetail.Basic), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell23.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell23.PaddingTop = 15f;
					staffSalaryComponentTableCell23.PaddingBottom = 15f;

					//DA  
					PdfPCell staffSalaryComponentTableCell31 = new PdfPCell(new Phrase("2.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell31.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell31.PaddingTop = 15f;
					staffSalaryComponentTableCell31.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell32 = new PdfPCell(new Phrase("Dearness Allowance(DA)", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell32.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell32.PaddingTop = 15f;
					staffSalaryComponentTableCell32.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell33 = new PdfPCell(new Phrase(Convert.ToString(staffSalaryDetail.DA), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell33.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell33.PaddingTop = 15f;
					staffSalaryComponentTableCell33.PaddingBottom = 15f;


					//Medical Allowance
					PdfPCell staffSalaryComponentTableCell41 = new PdfPCell(new Phrase("3.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell41.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell41.PaddingTop = 15f;
					staffSalaryComponentTableCell41.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell42 = new PdfPCell(new Phrase("Medical Allowance", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell42.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell42.PaddingTop = 15f;
					staffSalaryComponentTableCell42.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell43 = new PdfPCell(new Phrase(Convert.ToString(staffSalaryDetail.Medical), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell43.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell43.PaddingTop = 15f;
					staffSalaryComponentTableCell43.PaddingBottom = 15f;

					//Conveyance
					PdfPCell staffSalaryComponentTableCell51 = new PdfPCell(new Phrase("4.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell51.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell51.PaddingTop = 15f;
					staffSalaryComponentTableCell51.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell52 = new PdfPCell(new Phrase("Conveyance", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell52.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell52.PaddingTop = 15f;
					staffSalaryComponentTableCell52.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell53 = new PdfPCell(new Phrase(Convert.ToString(staffSalaryDetail.Conveyance), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell53.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell53.PaddingTop = 15f;
					staffSalaryComponentTableCell53.PaddingBottom = 15f;

					//HRA
					PdfPCell staffSalaryComponentTableCell61 = new PdfPCell(new Phrase("5.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell61.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell61.PaddingTop = 15f;
					staffSalaryComponentTableCell61.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell62 = new PdfPCell(new Phrase("HRA", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell62.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell62.PaddingTop = 15f;
					staffSalaryComponentTableCell62.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell63 = new PdfPCell(new Phrase(Convert.ToString(staffSalaryDetail.HRA), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell63.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell63.PaddingTop = 15f;
					staffSalaryComponentTableCell63.PaddingBottom = 15f;

					//LTA
					PdfPCell staffSalaryComponentTableCell71 = new PdfPCell(new Phrase("6.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell71.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell71.PaddingTop = 15f;
					staffSalaryComponentTableCell71.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell72 = new PdfPCell(new Phrase("LTA", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell72.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell72.PaddingTop = 15f;
					staffSalaryComponentTableCell72.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell73 = new PdfPCell(new Phrase(Convert.ToString(staffSalaryDetail.LTA), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell73.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell73.PaddingTop = 15f;
					staffSalaryComponentTableCell73.PaddingBottom = 15f;

					//Other
					PdfPCell staffSalaryComponentTableCell81 = new PdfPCell(new Phrase("7.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell81.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell81.PaddingTop = 15f;
					staffSalaryComponentTableCell81.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell82 = new PdfPCell(new Phrase("Other", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell82.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell82.PaddingTop = 15f;
					staffSalaryComponentTableCell82.PaddingBottom = 15f;

					PdfPCell staffSalaryComponentTableCell83 = new PdfPCell(new Phrase(Convert.ToString(staffSalaryDetail.Other), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffSalaryComponentTableCell83.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffSalaryComponentTableCell83.PaddingTop = 15f;
					staffSalaryComponentTableCell83.PaddingBottom = 15f;


					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell11);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell12);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell13);

					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell21);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell22);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell23);

					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell31);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell32);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell33);

					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell41);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell42);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell43);

					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell51);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell52);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell53);

					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell61);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell62);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell63);

					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell71);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell72);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell73);

					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell81);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell82);
					staffSalaryComponentTable.AddCell(staffSalaryComponentTableCell83);

					PdfPTable deductionHeadingTable = new PdfPTable(1);
					PdfPCell deductionHeadingTableCell = new PdfPCell(new Phrase(Convert.ToString("DEDUCTIONS"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					deductionHeadingTableCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					deductionHeadingTableCell.PaddingTop = 15f;
					deductionHeadingTableCell.PaddingBottom = 15f;

					deductionHeadingTable.AddCell(deductionHeadingTableCell);

					/*******************************************************************DEDUCTION COMPONENT TABLE ****************************************************************************************/
					PdfPTable staffdeductionComponentTable = new PdfPTable(3);

					staffdeductionComponentTable.SetWidths(widths);


					//Provident Fund
					PdfPCell staffdeductionComponentTableCell11 = new PdfPCell(new Phrase("8", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffdeductionComponentTableCell11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffdeductionComponentTableCell11.PaddingTop = 15f;
					staffdeductionComponentTableCell11.PaddingBottom = 15f;

					PdfPCell staffdeductionComponentTableCell12 = new PdfPCell(new Phrase("Provident Fund", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffdeductionComponentTableCell12.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffdeductionComponentTableCell12.PaddingTop = 15f;
					staffdeductionComponentTableCell12.PaddingBottom = 15f;

					PdfPCell staffdeductionComponentTableCell13 = new PdfPCell(new Phrase(Convert.ToString(staffSalaryDetail.Provident_Fund), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffdeductionComponentTableCell13.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffdeductionComponentTableCell13.PaddingTop = 15f;
					staffdeductionComponentTableCell13.PaddingBottom = 15f;


					//ESIC
					PdfPCell staffdeductionComponentTableCell21 = new PdfPCell(new Phrase("9", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffdeductionComponentTableCell21.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffdeductionComponentTableCell21.PaddingTop = 15f;
					staffdeductionComponentTableCell21.PaddingBottom = 15f;

					PdfPCell staffdeductionComponentTableCell22 = new PdfPCell(new Phrase("ESIC", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffdeductionComponentTableCell22.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffdeductionComponentTableCell22.PaddingTop = 15f;
					staffdeductionComponentTableCell22.PaddingBottom = 15f;

					PdfPCell staffdeductionComponentTableCell23 = new PdfPCell(new Phrase(Convert.ToString(staffSalaryDetail.ESIC), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffdeductionComponentTableCell23.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffdeductionComponentTableCell23.PaddingTop = 15f;
					staffdeductionComponentTableCell23.PaddingBottom = 15f;

					//Professional Tax
					PdfPCell staffdeductionComponentTableCell31 = new PdfPCell(new Phrase("10", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffdeductionComponentTableCell31.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffdeductionComponentTableCell31.PaddingTop = 15f;
					staffdeductionComponentTableCell31.PaddingBottom = 15f;

					PdfPCell staffdeductionComponentTableCell32 = new PdfPCell(new Phrase("Professional Tax", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffdeductionComponentTableCell32.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffdeductionComponentTableCell32.PaddingTop = 15f;
					staffdeductionComponentTableCell32.PaddingBottom = 15f;

					PdfPCell staffdeductionComponentTableCell33 = new PdfPCell(new Phrase(Convert.ToString(staffSalaryDetail.Professional_Tax), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffdeductionComponentTableCell33.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffdeductionComponentTableCell33.PaddingTop = 15f;
					staffdeductionComponentTableCell33.PaddingBottom = 15f;

					//Professional Tax
					PdfPCell staffdeductionComponentTableCell41 = new PdfPCell(new Phrase("11", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffdeductionComponentTableCell41.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffdeductionComponentTableCell41.PaddingTop = 15f;
					staffdeductionComponentTableCell41.PaddingBottom = 15f;

					PdfPCell staffdeductionComponentTableCell42 = new PdfPCell(new Phrase("Other Deductions", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffdeductionComponentTableCell42.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffdeductionComponentTableCell42.PaddingTop = 15f;
					staffdeductionComponentTableCell42.PaddingBottom = 15f;

					PdfPCell staffdeductionComponentTableCell43 = new PdfPCell(new Phrase(Convert.ToString(staffSalaryDetail.Other_Deductions), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffdeductionComponentTableCell43.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					staffdeductionComponentTableCell43.PaddingTop = 15f;
					staffdeductionComponentTableCell43.PaddingBottom = 15f;


					staffdeductionComponentTable.AddCell(staffdeductionComponentTableCell11);
					staffdeductionComponentTable.AddCell(staffdeductionComponentTableCell12);
					staffdeductionComponentTable.AddCell(staffdeductionComponentTableCell13);

					staffdeductionComponentTable.AddCell(staffdeductionComponentTableCell21);
					staffdeductionComponentTable.AddCell(staffdeductionComponentTableCell22);
					staffdeductionComponentTable.AddCell(staffdeductionComponentTableCell23);

					staffdeductionComponentTable.AddCell(staffdeductionComponentTableCell31);
					staffdeductionComponentTable.AddCell(staffdeductionComponentTableCell32);
					staffdeductionComponentTable.AddCell(staffdeductionComponentTableCell33);

					staffdeductionComponentTable.AddCell(staffdeductionComponentTableCell41);
					staffdeductionComponentTable.AddCell(staffdeductionComponentTableCell42);
					staffdeductionComponentTable.AddCell(staffdeductionComponentTableCell43);

					/*******************************************************************DEDUCTION COMPONENT TABLE ENDS  ****************************************************************************************/

					/*******************************************************************GROSS & NET SALARY TABLE   ****************************************************************************************/

					PdfPTable staffGrossAndNetSalaryTable = new PdfPTable(2);

					float[] width = new float[] { 440f, 100f };
					staffGrossAndNetSalaryTable.SetWidths(width);

					PdfPCell staffGrossAndNetSalaryTableCell11 = new PdfPCell(new Phrase(Convert.ToString("GROSS SALARY"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffGrossAndNetSalaryTableCell11.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					staffGrossAndNetSalaryTableCell11.PaddingTop = 15f;
					staffGrossAndNetSalaryTableCell11.PaddingBottom = 15f;

					PdfPCell staffGrossAndNetSalaryTableCell12 = new PdfPCell(new Phrase(Convert.ToString(staffSalaryDetail.Gross_Salary), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffGrossAndNetSalaryTableCell12.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					staffGrossAndNetSalaryTableCell12.PaddingTop = 15f;
					staffGrossAndNetSalaryTableCell12.PaddingBottom = 15f;

					PdfPCell staffGrossAndNetSalaryTableCell21 = new PdfPCell(new Phrase(Convert.ToString("NET SALARY"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffGrossAndNetSalaryTableCell21.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					staffGrossAndNetSalaryTableCell21.PaddingTop = 15f;
					staffGrossAndNetSalaryTableCell21.PaddingBottom = 15f;

					PdfPCell staffGrossAndNetSalaryTableCell22 = new PdfPCell(new Phrase(Convert.ToString(staffSalaryDetail.Net_Salary), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					staffGrossAndNetSalaryTableCell22.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					staffGrossAndNetSalaryTableCell22.PaddingTop = 15f;
					staffGrossAndNetSalaryTableCell22.PaddingBottom = 15f;

					staffGrossAndNetSalaryTable.AddCell(staffGrossAndNetSalaryTableCell11);
					staffGrossAndNetSalaryTable.AddCell(staffGrossAndNetSalaryTableCell12);
					staffGrossAndNetSalaryTable.AddCell(staffGrossAndNetSalaryTableCell21);
					staffGrossAndNetSalaryTable.AddCell(staffGrossAndNetSalaryTableCell22);

					/*******************************************************************GROSS & NET SALARY TABLE ENDS   ****************************************************************************************/

					PdfPTable signatureTable = new PdfPTable(1);
					PdfPCell signatureTableCell11 = new PdfPCell(new Phrase(Convert.ToString("AUTHORISED SIGNATURE"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					signatureTableCell11.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					signatureTableCell11.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
					signatureTableCell11.PaddingTop = 65f;
					signatureTableCell11.PaddingBottom = 15f;

					signatureTable.AddCell(signatureTableCell11);

					pdfDoc.Add(headingTable);
					pdfDoc.Add(logoTable);
					pdfDoc.Add(staffDetailTable);
					pdfDoc.Add(bankAccountDetailTable);
					pdfDoc.Add(staffSalaryComponentTable);
					pdfDoc.Add(deductionHeadingTable);
					pdfDoc.Add(staffdeductionComponentTable);
					pdfDoc.Add(staffGrossAndNetSalaryTable);
					pdfDoc.Add(signatureTable);


					pdfDoc.Close();
					// Close the writer instance
					writer.Close();
					// Always close open filehandles explicity
					fs.Close();
				}
			}
			   

			/********************************************************************************************************************************************************************************
			 * *******************************************************************************************************************************************************************************/
			
			return View();
		}
		#endregion

		#region EditTeachingStaff

		public ActionResult EditTeachingStaff(long? id)
		{

			Staff staffToBeEdited = new Staff();
			GetBloodGroup();
			GetGender();

			GetCountries();
			GetClass();

			using (var dbcontext = new SchoolERPDBContext())
			{
				var staffEdited = dbcontext.Staff.Find(id);
				//GetSectionForClass(Convert.ToString(staffEdited.Class_Id));
				GetStatesForCountry(Convert.ToString(staffEdited.Country_Id));
				GetCitiesForState(Convert.ToString(staffEdited.State_Id));
				staffToBeEdited.Staff_Id = Convert.ToInt32(id);


				TempData["Staff_Id"] = Convert.ToInt32(id); 
				TempData.Keep("Staff_Id");


				staffToBeEdited = staffEdited;

				byte[] byteData = staffEdited.Photo;
				//Convert byte arry to base64string   
				string imreBase64Data = Convert.ToBase64String(byteData);
				string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
				//Passing image data in viewbag to view  
				ViewBag.ImageData = imgDataURL;


			}

			return View(staffToBeEdited);
		}

		public JsonResult FetchCountryNameBasedOnId(string countryId)
		{
			return Json(new { items = GetCountryNameBasedOnId(countryId) }, JsonRequestBehavior.AllowGet);

		}

		#endregion

		#region SalarySlips

		public List<StaffSalarySlip_ViewModel> GetStaffMonthlySalary(int nYear, int nMonth)
			{
			string sYear = Convert.ToString(nYear);
		//DateTime dtFromSalaryCutOffDate = DateTime.Now.AddMonths(-1);
		//DateTime dtToSalaryCutOffDate = DateTime.Now.AddMonths(0);
		var dtFromSalaryCutOffDate = new DateTime(nYear, nMonth, 1);
		var dtToSalaryCutOffDate = dtFromSalaryCutOffDate.AddMonths(1).AddDays(-1);
		TimeSpan ts = dtToSalaryCutOffDate - dtFromSalaryCutOffDate;
		int nDays = ts.Days;
		List<StaffSalarySlip_ViewModel> staffMonthlySalaryModelList = new List<StaffSalarySlip_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				//if (dbcontext.Staff_MonthlySalary_Details.Where(x => x.academic_year == nYear && x.salary_month == nMonth).Count() == 0)
				//{
					staffMonthlySalaryModelList = (from staff in dbcontext.Staff
												   join staff_salary_Detail in dbcontext.Staff_Salary_Detail on staff.Staff_Id equals staff_salary_Detail.Staff_Id
												   // join  in dbcontext.Staff_Attendance on staff.Staff_Id equals staff_attendance.Staff_Id
												   where staff.Academic_Year == nYear && (staff.Is_Deleted == false || staff.Is_Deleted == null)
												   select new StaffSalarySlip_ViewModel
												   {

													   Id = staff.Staff_Id,
													   Name = staff.First_Name + " " + staff.Last_Name + " " + staff.Middle_Name,
													   staff_id = staff.Staff_Id,
													   no_of_leaves = dbcontext.Staff_Attendance.Where(x => x.Staff_Id == staff.Staff_Id && staff.Academic_Year == nYear && x.Leave_Date >= dtFromSalaryCutOffDate && x.Leave_Date <= dtToSalaryCutOffDate).ToList().Count(),
													   leaves_remaining = 5 - dbcontext.Staff_Attendance.Where(x => x.Staff_Id == staff.Staff_Id && staff.Academic_Year == nYear).ToList().Count(),
													   Created_On = staff.Created_On,
													   salary_deduction = (5 - dbcontext.Staff_Attendance.Where(x => x.Staff_Id == staff.Staff_Id && staff.Academic_Year == nYear).ToList().Count() < 0) ? (dbcontext.Staff_Attendance.Where(x => x.Staff_Id == staff.Staff_Id && staff.Academic_Year == nYear && x.Leave_Date >= dtFromSalaryCutOffDate && x.Leave_Date <= dtToSalaryCutOffDate).ToList().Count() * staff_salary_Detail.Net_Salary) / (nDays) : 0,
													   gross_salary = staff_salary_Detail.Net_Salary - ((20 - dbcontext.Staff_Attendance.Where(x => x.Staff_Id == staff.Staff_Id && staff.Academic_Year == nYear).ToList().Count() < 0) ? (dbcontext.Staff_Attendance.Where(x => x.Staff_Id == staff.Staff_Id && staff.Academic_Year == nYear && x.Leave_Date >= dtFromSalaryCutOffDate && x.Leave_Date <= dtToSalaryCutOffDate).ToList().Count() * staff_salary_Detail.Net_Salary) / (nDays) : 0),
													   salary_Month_name = dbcontext.Month.Where(x => x.Id == nMonth).FirstOrDefault().Name + " - " + sYear,
													   salary_month = nMonth ,
													   academic_year = nYear


												}).OrderBy(x => x.Created_On).ToList();


					foreach (var staffMonthlySalary in staffMonthlySalaryModelList)
					{
						if (dbcontext.Staff_MonthlySalary_Details.Where(x => x.academic_year == nYear && x.salary_month == nMonth && x.staff_id == staffMonthlySalary.staff_id && (x.Is_Deleted == null && x.Is_Deleted == false)).Count() == 0)
						{
							Staff_MonthlySalary_Details staffMonthlySalaryDetails = new Staff_MonthlySalary_Details();

						staffMonthlySalaryDetails.academic_year = staffMonthlySalary.academic_year;
						staffMonthlySalaryDetails.staff_id = staffMonthlySalary.staff_id;
						staffMonthlySalaryDetails.salary_month = staffMonthlySalary.salary_month;
						staffMonthlySalaryDetails.no_of_leaves = staffMonthlySalary.no_of_leaves;
						staffMonthlySalaryDetails.salary_deduction = Convert.ToDecimal(staffMonthlySalary.salary_deduction);
						staffMonthlySalaryDetails.leaves_remaining = staffMonthlySalary.leaves_remaining;
						staffMonthlySalaryDetails.gross_salary = Convert.ToDecimal(staffMonthlySalary.gross_salary);
						staffMonthlySalaryDetails.Is_Active = true;
						staffMonthlySalaryDetails.Created_By = 5;
						staffMonthlySalaryDetails.Created_On = DateTime.Now;

						dbcontext.Staff_MonthlySalary_Details.Add(staffMonthlySalaryDetails);
						dbcontext.SaveChanges();
					}



					}
				//}
				//else
				//{
				//	staffMonthlySalaryModelList = (from staff_monthlySalaryDetail in dbcontext.Staff_MonthlySalary_Details
				//								   join staff in dbcontext.Staff on staff_monthlySalaryDetail.staff_id equals staff.Staff_Id
				//								   where staff_monthlySalaryDetail.academic_year == nYear && staff_monthlySalaryDetail.salary_month == nMonth
				//								   select new StaffSalarySlip_ViewModel
				//								   {
				//									   Id = staff_monthlySalaryDetail.staff_id,
				//									   Name = staff.First_Name + " " + staff.Last_Name + " " + staff.Middle_Name,
				//									   staff_id = staff_monthlySalaryDetail.staff_id,
				//									   no_of_leaves = staff_monthlySalaryDetail.no_of_leaves,
				//									   leaves_remaining = staff_monthlySalaryDetail.leaves_remaining,
				//									   Created_On = DateTime.Now,
				//									   salary_deduction = staff_monthlySalaryDetail.salary_deduction,
				//									   gross_salary = staff_monthlySalaryDetail.gross_salary,
				//									   salary_month = staff_monthlySalaryDetail.salary_month,
				//									   salary_Month_name = dbcontext.Month.Where(x => x.Id == nMonth).FirstOrDefault().Name + " - " + sYear,
				//									   academic_year = staff_monthlySalaryDetail.academic_year



				//								   }).ToList();
				//}

				
			}

			return staffMonthlySalaryModelList;
		}

		[HttpPost]
		public ActionResult GetMonthlySalaryForStaff(int nYear , int nMonth)
		{
			
			return Json(GetStaffMonthlySalary(nYear , nMonth), JsonRequestBehavior.AllowGet);

			//	return staffMonthlySalaryModelList;
		}

		public ActionResult GetStaffListForGeneratingMonthlySalary()
		{
			int nYear = Convert.ToInt32(GetAcademicYear());
			string sYear = Convert.ToString(GetAcademicYear());
			int nMonth = DateTime.Now.Month;
			nMonth = nMonth - 1;
			DateTime dtFromSalaryCutOffDate = DateTime.Now.AddMonths(-1);
			DateTime dtToSalaryCutOffDate = DateTime.Now.AddMonths(0);

		

			GetMonth();
			List<StaffSalarySlip_ViewModel> staffMonthlySalaryModelList = new List<StaffSalarySlip_ViewModel>();
			staffMonthlySalaryModelList = GetStaffMonthlySalary(nYear , nMonth);
			return View(staffMonthlySalaryModelList);
			//return View();
		}

		[HttpPost]
		public ActionResult GetStaffListForGeneratingMonthlySalary(int nYear, int nMonth)
		{
			//int nYear = Convert.ToInt32(GetAcademicYear());
			//string sYear = Convert.ToString(GetAcademicYear());
			//int nMonth = DateTime.Now.Month;
			nMonth = nMonth - 1;
			DateTime dtFromSalaryCutOffDate = DateTime.Now.AddMonths(-1);
			DateTime dtToSalaryCutOffDate = DateTime.Now.AddMonths(0);



			GetMonth();
			List<StaffSalarySlip_ViewModel> staffMonthlySalaryModelList = new List<StaffSalarySlip_ViewModel>();
			//staffMonthlySalaryModelList = GetMonthlySalaryForStaff(nYear, nMonth);

			return Json( GetMonthlySalaryForStaff(nYear, nMonth), JsonRequestBehavior.AllowGet);
			//return View(staffMonthlySalaryModelList);
			//return View();
		}

		[HttpPost]
		public ActionResult GetNoOfWorkingDaysinMonth(string Year, string Month_Id)
		{
			int nMonth_Id = Convert.ToInt16(Month_Id);
			int nYear = Convert.ToInt16(Year);
			var startDate = new DateTime(nYear, nMonth_Id, 1);
			var endDate = startDate.AddMonths(1).AddDays(-1);
			TimeSpan ts = (endDate - startDate);
			int ntotNoOfDays = ts.Days;
			int nnoOfWorkingDays;
			using (var dbcontext = new SchoolERPDBContext())
			{
				nnoOfWorkingDays = ntotNoOfDays - dbcontext.Holiday.Where(x => x.Holiday_Date >= startDate && x.Holiday_Date <= endDate).Count();
			}
			return Json(nnoOfWorkingDays, JsonRequestBehavior.AllowGet);
		}
		#endregion

	}
}