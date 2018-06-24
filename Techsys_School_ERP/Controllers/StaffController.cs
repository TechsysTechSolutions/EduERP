using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
			staffEducationalDetail.Academic_Year = 2019;
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
														 Id = staffEduDetail.StaffDetail_Id,
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

	}
}