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
    public class StudentController : CommonController
	{
        // GET: Student
        public ActionResult CreateStudent()
        {
			GetBloodGroup();
			GetGender();
			GetCountries();
			GetClass();
            return View();
        }


		[HttpPost]

	   public JsonResult CreateStudent(HttpPostedFileBase file, Student model)
		//public JsonResult CreateStudent(Student model, HttpPostedFileBase postedFile)
		{
			return null;
		}



		[HttpPost]
		public JsonResult FetchSectionIDForClass(string Class_Id)
		{
			List<Section> lstSection = GetSectionForClass(Class_Id);

			return Json(lstSection, JsonRequestBehavior.AllowGet);

		}

		public ActionResult JQGrid()
		{
			return View();
		}

		

	    [HttpPost]
		public JsonResult FetchStateForCountry(string Country_Id)
		{
			List<State> lstState = GetStatesForCountry(Country_Id);

			return Json(lstState, JsonRequestBehavior.AllowGet);

		}


		

		[HttpPost]
		public JsonResult FetchCitiesForState(string State_Id)
		{
			List<City> lstCity = GetCitiesForState(State_Id);

			return Json(lstCity, JsonRequestBehavior.AllowGet);

		}

		[HttpPost]
		public ActionResult InsertProduct1(HttpPostedFileBase helpSectionImages)
		{

			//string fileName = file.FileName;
			//file.SaveAs(Server.MapPath("~/Content/Images"));
			//return Json("Complete!");

			if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
			{
				var pic = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];

				using (var dbcontext = new SchoolERPDBContext())
				{
					//byte[] imgbyte = imageToByteArray(pic);
					Image newImage = new Image();
					//newImage.Name =(byte[]) pic;
					//dbcontext.Image.Add(newImage);
					//dbcontext.SaveChanges();
				}
			}

			

				return null;
		}


		public ActionResult Index1()
		{
			return View();
		}

		[HttpPost]
		public ActionResult UploadFiles()
		{
			// Checking no of files injected in Request object  
			if (Request.Files.Count > 0)
			{
				try
				{
					//  Get all files from Request object  
					HttpFileCollectionBase files = Request.Files;
					for (int i = 0; i < files.Count; i++)
					{
						//string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
						//string filename = Path.GetFileName(Request.Files[i].FileName);  

						HttpPostedFileBase file = files[i];
						string fname;

						// Checking for Internet Explorer  
						if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
						{
							string[] testfiles = file.FileName.Split(new char[] { '\\' });
							fname = testfiles[testfiles.Length - 1];
						}
						else
						{
							fname = file.FileName;
						}

						// Get the complete folder path and store the file inside it.  
						fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
						file.SaveAs(fname);
					}
					// Returns message that successfully uploaded  
					return Json("File Uploaded Successfully!");
				}
				catch (Exception ex)
				{
					return Json("Error occurred. Error details: " + ex.Message);
				}
			}
			else
			{
				return Json("No files selected.");
			}
		}

		public byte[] imageToByteArray(System.Drawing.Image imageIn)
		{
			MemoryStream ms = new MemoryStream();
			imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
			return ms.ToArray();
		}

		public ActionResult AddStudentSiblingAndPrevSchoolDetail()
		{
			return View();


		}

		[HttpPost]
		public JsonResult AutoComplete(string prefix)
		{
			List<City> lstExam = new List<City>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				var ex = (from customer in dbcontext.City
			

							select new City

							{
					Id = customer.Id,
                                Name = customer.Name

						   }).ToList();

				lstExam = ex.ToList();


			};
			

			return Json(lstExam);
		}

		[HttpPost]
		public JsonResult Index(string Prefix)
		{
			//Note : you can bind same list from database  
			List<Exam> ObjList = new List<Exam>()
			{

				new Exam {Id=1,Name="Latur" },
				new Exam {Id=2,Name="Mumbai" },
				new Exam {Id=3,Name="Pune" },
				new Exam {Id=4,Name="Delhi" },
				new Exam {Id=5,Name="Dehradun" },
				new Exam {Id=6,Name="Noida" },
				new Exam {Id=7,Name="New Delhi" }

		};
			//Searching records from list using LINQ query  
			var CityList = (from N in ObjList
							where N.Name.StartsWith(Prefix)
							select new {N.Id, N.Name });
			return Json(CityList, JsonRequestBehavior.AllowGet);
		}




	}
}