
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Techsys_School_ERP.DBAccess;
using Techsys_School_ERP.Model;

namespace Techsys_School_ERP.Controllers
{
    public class AssignmentController : CommonController
    {
        // GET: Assignment
        public ActionResult GetAssignmentList()
        {
            return View();
        }

		public ActionResult AddAssignmentForClassAndSection()
		{
			GetClass();
			GetSubjects();
			return View();
			
		}

		[HttpPost]
		public ActionResult AddAssignmentForClassAndSection(Assignment Assignment)
		{
			long nYear = GetAcademicYear();
			Assignment.Created_By = 5;
			Assignment.Created_On = DateTime.Now;
			string sReturn_Text = string.Empty;
			int nAssignment_Id;

			try

			{

				using (var dbcontext = new SchoolERPDBContext())
			{
				if (dbcontext.Assignment.ToList().Count == 0)
				{

						nAssignment_Id = 1;
				}
				else
				{
						nAssignment_Id = Convert.ToInt32(dbcontext.Assignment.Max(x => x.Id) + 1);
					

					TempData["Assignment_Id"] = nAssignment_Id;
					TempData.Keep("Assignment_Id");
				}
			}

		

				using (var dbcontext = new SchoolERPDBContext())
				{
					dbcontext.Assignment.Add(Assignment);
					dbcontext.SaveChanges();
					sReturn_Text = "OK";
				}
			}
			catch (Exception ex)
			{
				sReturn_Text = ex.InnerException.Message.ToString();
			}
				 return Json(sReturn_Text, JsonRequestBehavior.AllowGet); ;
		}

		[HttpPost]
		public JsonResult FetchSectionIDForClass(string Class_Id)
		{
			List<Section> lstSection = GetSectionForClass(Class_Id);

			return Json(lstSection, JsonRequestBehavior.AllowGet);

		}

		[HttpPost]
		public JsonResult Upload(HttpPostedFileBase file)

		{
			long nAssignment_Id = Convert.ToInt32(TempData.Peek("Assignment_Id"));
			Assignment assignmentToBeModified = new Assignment();

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
						assignmentToBeModified = dbcontext.Assignment.Find(nAssignment_Id);
						assignmentToBeModified.Photo = bytes;
						dbcontext.Entry(assignmentToBeModified).CurrentValues.SetValues(assignmentToBeModified);
						dbcontext.SaveChanges();
					}


				}
				return Json("Assignment Successfully Added", JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);

			}

			return null;

		}
	}
}