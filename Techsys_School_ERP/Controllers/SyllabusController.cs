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
using System.IO;

namespace Techsys_School_ERP.Controllers
{
    public class SyllabusController : CommonController
    {
        // GET: Syllabus
        public ActionResult SyllabusList()
        {
			
			return View(getSyllabusList(GetAcademicYear()));
		//	return View();
        }


		public List<SyllabusList_ViewModel> getSyllabusList(long nyear)
		{
			long nYear = GetAcademicYear();
			List<SyllabusList_ViewModel> syllabusVieModelList = new List<SyllabusList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				syllabusVieModelList = (from syllabus in dbcontext.Syllabus
										join cls in dbcontext.Class on syllabus.Class_Id equals cls.Id
										join sec in dbcontext.Section on syllabus.Section_Id equals sec.Id
										join term in dbcontext.Term on syllabus.Term_Id equals term.Id
										join sub in dbcontext.Subject on syllabus.Subject_Id equals sub.Id
										where syllabus.Is_Deleted == false && syllabus.Academic_Year == nYear
										select new SyllabusList_ViewModel
										{

											Id = syllabus.Id,
											Subject_Id = syllabus.Subject_Id,
											Class_Id = syllabus.Class_Id,
											Section_Id = syllabus.Section_Id,
											Class_Name = cls.Name + " - " + sec.Name,
											Subject_Name = sub.Name,
											Term_Id = term.Id,
											Term_Name = term.Name,
											Syllabus_Text = syllabus.Syllabus_Text,
											Academic_Year = nYear,
											Created_On = syllabus.Created_On



										}).OrderBy(x => x.Created_On).ToList();
			}

			return syllabusVieModelList;
		}

		public ActionResult AddSyllabus()
		{
			GetTerm();
			PopulateClassAndSection();
			GetSubjects();
			return View();
		}
    }
}