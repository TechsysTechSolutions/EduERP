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
				return cityList;


			}


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
	}
}