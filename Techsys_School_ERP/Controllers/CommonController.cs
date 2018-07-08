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

namespace Techsys_School_ERP.Controllers
{
	public class CommonController : Controller
	{
		// GET: Common
		public ActionResult Index()
		{
			return View();
		}


		public ActionResult SearchAndGetSchoolList(string q)
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
			return Json(new { items = list }, JsonRequestBehavior.AllowGet);
		}
		public ActionResult SearchAndGetStudentList(string q)
		{
			var list = new List<SearchList>();

			List<Student> schoolList = new List<Student>();

			using (var dbcontext = new SchoolERPDBContext())
			{

				if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
				{
					string newLine = ((char)13).ToString() + ((char)10).ToString(); ;

					schoolList = (from stu in dbcontext.Student
								  join cls in dbcontext.Class on stu.Class_Id equals cls.Id
								  join sec in dbcontext.Section on stu.Section_Id equals sec.Id
								  where stu.First_Name.Contains(q.ToLower())
								  select new
								  {
									  Text = stu.First_Name + " " + stu.Last_Name + " " +
									  Environment.NewLine
									  + "[" + stu.Roll_No + "]" + " - " + cls.Name + " " + sec.Name,
									  Id = stu.Student_Id
								  }).ToList().
								   Select(x => new Student

								   {

									   First_Name = x.Text,
									   Student_Id = x.Id

								   }).ToList();


				}
			}


			for (int i = 0; i < schoolList.Count(); i++)
			{
				SearchList oSelect2Model = new SearchList();
				oSelect2Model.text = schoolList[i].First_Name;
				oSelect2Model.id = schoolList[i].Student_Id.ToString();

				list.Add(oSelect2Model);


			}


			if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
			{
				list = list.Where(x => x.text.ToLower().Contains(q.ToLower())).ToList();
			}
			//return Json(new { data = schoolList }, JsonRequestBehavior.AllowGet);
			return Json(new { items = list }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult SearchAndGetStaffList(string q)
		{
			var list = new List<SearchList>();

			List<Staff> staffList = new List<Staff>();

			using (var dbcontext = new SchoolERPDBContext())
			{

				if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
				{
					string newLine = ((char)13).ToString() + ((char)10).ToString(); ;

					staffList = (from staff in dbcontext.Staff
								 where staff.First_Name.Contains(q.ToLower())
								  select new
								  {
									  Text = staff.First_Name + " " + staff.Last_Name + " " +
									  Environment.NewLine
									  + "[" + staff.Employee_Id + "]" ,
									  Id = staff.Staff_Id
								  }).ToList().
								   Select(x => new Staff

								   {

									   First_Name = x.Text,
									   Staff_Id = x.Id

								   }).ToList();


				}
			}


			for (int i = 0; i < staffList.Count(); i++)
			{
				SearchList oSelect2Model = new SearchList();
				oSelect2Model.text = staffList[i].First_Name;
				oSelect2Model.id = staffList[i].Staff_Id.ToString();
				list.Add(oSelect2Model);
			}


			if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
			{
				list = list.Where(x => x.text.ToLower().Contains(q.ToLower())).ToList();
			}

			//return Json(new { items = list }, JsonRequestBehavior.AllowGet);
			return Json( list , JsonRequestBehavior.AllowGet);
		}

		public void GetClass()
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var clsList = (from cls in dbcontext.Class select cls).ToList();
				if (clsList != null)
				{
					ViewBag.classList = clsList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}
		}

		public void GetDesignation()
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var designationList = (from desgnList in dbcontext.Designation select desgnList).ToList();
				if (designationList != null)
				{
					ViewBag.designationList = designationList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}
		}

		public void GetCategory()
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var categoryList = (from ctgList in dbcontext.Category select ctgList).ToList();
				if (categoryList != null)
				{
					ViewBag.categoryList = categoryList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}
		}


		public int GetLoggedInUserId()
		{
			int nUser_Id;
			using (var dbcontext = new SchoolERPDBContext())
			{
				nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
			}

			return nUser_Id;
		}

		public void GetStaffType()
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var staffList = (from stffList in dbcontext.Staff_Type select stffList).ToList();
				if (staffList != null)
				{
					ViewBag.staffList = staffList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}
		}

		public List<AutoComplete_ViewModel> GetCountryNameBasedOnId(string sCountry_Id)
		{
			int nCountry_Id = Convert.ToInt16(sCountry_Id);
			var list = new List<AutoComplete_ViewModel>();

			List<Country> countryList = new List<Country>();

			using (var dbcontext = new SchoolERPDBContext())
			{

				if (!(string.IsNullOrEmpty(sCountry_Id)))
				{

					countryList = (from country in dbcontext.Country
								//  where country.Name.Contains(q.ToLower())
								   where country.Id == nCountry_Id
								   select new
								   {
									   Text = country.Name,
									   Id = country.Id
								   }).ToList().
								   Select(x => new Country

								   {

									   Name = x.Text,
									   Id = x.Id

								   }).ToList();

				}
			}


			for (int i = 0; i < countryList.Count(); i++)
			{
				AutoComplete_ViewModel oAutoCompleteViewModel = new AutoComplete_ViewModel();
				oAutoCompleteViewModel.text = countryList[i].Name;
				oAutoCompleteViewModel.id = countryList[i].Id.ToString();

				list.Add(oAutoCompleteViewModel);


			}


			//if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
			//{
			//	list = list.Where(x => x.text.ToLower().Contains(q.ToLower())).ToList();
			//}

			return list;
		}


		public List<AutoComplete_ViewModel> GetCountriesList(string q)
		{
			var list = new List<AutoComplete_ViewModel>();

			List<Country> countryList = new List<Country>();

			using (var dbcontext = new SchoolERPDBContext())
			{

				if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
				{

					countryList = (from country in dbcontext.Country
								  where country.Name.Contains(q.ToLower())
								  select new
								  {
									  Text = country.Name,
									  Id = country.Id
								  }).ToList().
								   Select(x => new Country

								   {

									   Name = x.Text,
									   Id = x.Id

								   }).ToList();

				}
			}


			for (int i = 0; i < countryList.Count(); i++)
			{
				AutoComplete_ViewModel oAutoCompleteViewModel = new AutoComplete_ViewModel();
				oAutoCompleteViewModel.text = countryList[i].Name;
				oAutoCompleteViewModel.id = countryList[i].Id.ToString();

				list.Add(oAutoCompleteViewModel);


			}


			if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
			{
				list = list.Where(x => x.text.ToLower().Contains(q.ToLower())).ToList();
			}

			return list;
			
		}

		public long GetAcademicYear()
		{
			long nAcademicYear = (DateTime.Now.Month <= 4) ? DateTime.Now.Year : DateTime.Now.Year + 1;

			return nAcademicYear;
		}

		public void GetExamList()
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var exmList = (from exm in dbcontext.Exam select exm).ToList();
				if (exmList != null)
				{
					ViewBag.examList = exmList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}
		}

		public void GetTerm()
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var termList = (from term in dbcontext.Term select term).ToList();
				if (termList != null)
				{
					ViewBag.termList = termList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}
		}

		public void GetSubjects()
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var subjList = (from subj in dbcontext.Subject select subj).ToList();
				if (subjList != null)
				{
					ViewBag.subjectList = subjList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}
		}

		public void GetBloodGroup()
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var bloodGroupList = (from bldGrp in dbcontext.Blood_Group select bldGrp).ToList();
				if (bloodGroupList != null)
				{
					ViewBag.bloodGroupList = bloodGroupList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}
		}
		public void GetOccupation()
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var occupationList = (from occupation in dbcontext.Occupation select occupation).ToList();
				if (occupationList != null)
				{
					ViewBag.occupationList = occupationList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}
		}


		public void GetGender()
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var genderList = (from gndr in dbcontext.Gender select gndr).ToList();
				if (genderList != null)
				{
					ViewBag.genderList = genderList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}
		}

		public void GetCountries()
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var countriesList = (from country in dbcontext.Country select country).ToList();
				if (countriesList != null)
				{
					ViewBag.countriesList = countriesList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}
		}

		public void GetFrequency()
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var freqList = (from freq in dbcontext.Frequency select freq).ToList();
				if (freqList != null)
				{
					ViewBag.freqList = freqList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}
		}

		public List<State> GetStatesForCountry(string sCountry_Id)
		{
			List<State> stateList = new List<State>();
			int nCountry_Id = Convert.ToInt16(sCountry_Id);
			using (var dbcontext = new SchoolERPDBContext())
			{

				var state = (from st in dbcontext.State
							 join country in dbcontext.Country on st.Country_Id equals country.Id
							 where (st.Country_Id == nCountry_Id)
							 select new
							 {
								 Id = st.Id,
								 Name = st.Name
							 }).ToList()
							   .Select(x => new State()
							   {
								   Id = x.Id,
								   Name = x.Name,
							   });

				stateList = state.ToList();
				ViewBag.stateList = stateList;
				return stateList;
			}
		}


		public List<City> GetCitiesForState(string sState_Id)
		{
			List<City> cityList = new List<City>();
			int nState_Id = Convert.ToInt16(sState_Id);
			using (var dbcontext = new SchoolERPDBContext())
			{

				var city = (from st in dbcontext.State
							 join cty in dbcontext.City on st.Id equals cty.State_Id
							 where (cty.State_Id == nState_Id)
							 select new
							 {
								 Id = cty.Id,
								 Name = cty.Name
							 }).ToList()
							   .Select(x => new City()
							   {
								   Id = x.Id,
								   Name = x.Name,
							   });

				cityList = city.ToList();
				ViewBag.cityList = cityList;
				return cityList;


			}


		}


		public bool CheckIfEmailAlreadyExists(string Email_Id, bool? Is_Student, bool? Is_Staff)
		{
			bool bEmailIdExists = false;
			
				using (var dbcontext = new SchoolERPDBContext())
				{
					if (Is_Student == true)
					{
						bEmailIdExists = dbcontext.Student.Any(x => x.Email_Id == Email_Id);
					}
					else if (Is_Staff == true)
					{
						bEmailIdExists = dbcontext.Staff.Any(x => x.Email_Id == Email_Id);
					}
				}
			
			return bEmailIdExists;
		}


		public List<Section> GetSectionForClass(string sClass_Id)
		{
			List<Section> sectionList = new List<Section>();
			int nClass_Id = Convert.ToInt16(sClass_Id);
			using (var dbcontext = new SchoolERPDBContext())
			{

				var section = (from usr in dbcontext.Users
							   join cls in dbcontext.Class on usr.Id equals cls.Created_By
							   join sec in dbcontext.Section on cls.Id equals sec.Class_Id
							   where (sec.Is_Deleted == null || sec.Is_Deleted == false) && sec.Class_Id == nClass_Id
							   select new
							   {
								   Id = sec.Id,
								   Name = sec.Name
							   }).ToList()
							   .Select(x => new Section()
							   {
								   Id = x.Id,
								   Name = x.Name,
							   });

				sectionList = section.ToList();
				ViewBag.SectionListForClass = sectionList;
				return sectionList;


			}

		}

		

		public List<Section> PopulateClassAndSection()
		{
			List<Section> sectionList = new List<Section>();

			using (var dbcontext = new SchoolERPDBContext())
			{

				var section = (from usr in dbcontext.Users
							   join cls in dbcontext.Class on usr.Id equals cls.Created_By
							   join sec in dbcontext.Section on cls.Id equals sec.Class_Id
							   where (sec.Is_Deleted == null || sec.Is_Deleted == false)
							   select new
							   {
								   Id = sec.Id,
								   Name = cls.Name + " - " + sec.Name
							   }).ToList()
							   .Select(x => new Section()
							   {
								   Id = x.Id,
								   Name = x.Name,
							   }).OrderBy(x=>x.Name);

				sectionList = section.ToList();
				return sectionList;
			}
		}


		public string GetClassANdSectionForStudent(long nStudent_id)
		{
			string sClassAndSectionName = string.Empty;
			//long nStudent_Id = Convert.ToInt64(sStudent_id);

			using (var dbcontext = new SchoolERPDBContext())
			{
				sClassAndSectionName = (from stu in dbcontext.Student
											   join cls in dbcontext.Class on stu.Class_Id equals cls.Id
											   join sec in dbcontext.Section on stu.Section_Id equals sec.Id
											   where stu.Student_Id == nStudent_id
										select new
											   {
												   Text =  cls.Name + " " + sec.Name
											   }).ToList()[0].Text;

			}

			return sClassAndSectionName;
		}

		public List<FeeConfiguration_ViewModel> GetFeeStructureForStudent(string Student_Id, string Frequency)
		{
			Dictionary<string, decimal> feeStructure = new Dictionary<string, decimal>();
			List<FeeConfiguration_ViewModel> feeConfigList = new List<FeeConfiguration_ViewModel>();
			long nStudent_Id = Convert.ToInt64(Student_Id);
			int nFrequency = Convert.ToInt32(Frequency);
			long nAcademicYear = GetAcademicYear();
			using (var dbcontext = new SchoolERPDBContext())
			{
				feeConfigList = (from stu in dbcontext.Student
								 join feeConfig in dbcontext.Fee_Configuration on stu.Class_Id equals feeConfig.Class_Id
								 join fee in dbcontext.Fee on feeConfig.Fee_Id equals fee.Id
								 where (feeConfig.Is_Deleted == null || feeConfig.Is_Deleted == false) && stu.Student_Id == nStudent_Id && feeConfig.Academic_Year == nAcademicYear
								 && feeConfig.Frequency == nFrequency
								 select new
								 {
									 Name = fee.Name,
									 Amount = feeConfig.Amount,
									 Total = feeConfig.Total
									 

									 // Total = fc.Total
								 }).ToList().Select(x => new FeeConfiguration_ViewModel()
								 {
									 Name = x.Name,
									 Amount = x.Amount,
									 Total = x.Total
									
									 
								 }).ToList();

				if (feeConfigList.Count() > 0)
				{
					if (nFrequency == 1 || nFrequency == 4)
					{
						feeConfigList[0].Next_Due_Date = "NA";
					}
					else
					{
						//int ntempFreq = nFrequency + 1;
						feeConfigList[0].Next_Due_Date = dbcontext.Term_Fee_Date.Where(x => x.Term_Id == nFrequency).FirstOrDefault().Fee_Pay_CutOff_Date.ToString();
					}

					//feeConfigList[0].Late_Fees = dbcontext.Fee_Payment.Where(x => x.Student_id == nStudent_Id && x.Academic_Year == nAcademicYear && x.Frequency == nFrequency).FirstOrDefault().Late_fees;
				}

			}

			return feeConfigList;
		}
	}
}