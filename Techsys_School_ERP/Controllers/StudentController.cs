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
			long nStudent_Id;
			Student student = new Student();
			student.Academic_Year = GetAcademicYear();
			using (var dbcontext = new SchoolERPDBContext())
			{
				if (dbcontext.Student.ToList().Count == 0)
				{
					
					student.Roll_No = "1" + Convert.ToString(GetAcademicYear());
				}
				else
				{
					nStudent_Id = Convert.ToInt64(dbcontext.Student.Max(x => x.Student_Id) + 1);
					student.Roll_No = Convert.ToString(GetAcademicYear()) + " - " + (nStudent_Id + 1);
					
					TempData["Student_Id"] = nStudent_Id;
					TempData.Keep("Student_Id");
				}
			}
			return View(student);
        }


		public ActionResult CreateStudentOtherDetails()
		{
			GetCategory();
			GetOccupation();
			return View();
		}


		[HttpPost]

		public JsonResult CreateStudent(Student formData)
		
		{
			Student newStudentToBeAdded = new Student();
			newStudentToBeAdded = formData;
			newStudentToBeAdded.Student_Id = Convert.ToInt64(TempData.Peek("Student_Id"));
			
			
			newStudentToBeAdded.Is_Active = true;
			//newStudentToBeAdded.Created_By = GetLoggedInUserId();
			newStudentToBeAdded.Created_By = 4;
			newStudentToBeAdded.Created_On = DateTime.Now;
			string sEmail_Id = formData.Email_Id;
			string sReturn_Text = string.Empty;
			try
			{
				if (ModelState.IsValid)
				{
					using (var dbcontext = new SchoolERPDBContext())
					{
						if (dbcontext.Student.Any(o => o.Email_Id == sEmail_Id))
						{
							sReturn_Text = "Email Already Exists";
						}
						else
						{
							dbcontext.Student.Add(newStudentToBeAdded);
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
		public ActionResult CreateStudentOtherDetails(Student_Other_Details Student_Other_Detail)
		{
			string sReturn_Text = string.Empty;
			Student_Other_Details newStudentOtherDetailsToBeAdded = new Student_Other_Details();
			newStudentOtherDetailsToBeAdded = Student_Other_Detail;
			newStudentOtherDetailsToBeAdded.Is_Active = true;
			newStudentOtherDetailsToBeAdded.Created_By = "5";
			//newStudentOtherDetailsToBeAdded.Student_Id = 113;
			newStudentOtherDetailsToBeAdded.Student_Id =  Convert.ToInt64(TempData.Peek("Student_Id"));
			newStudentOtherDetailsToBeAdded.Created_On = DateTime.Now;

			try
			{
				using (var dbcontext = new SchoolERPDBContext())
				{
					dbcontext.Student_Other_Details.Add(newStudentOtherDetailsToBeAdded);
					dbcontext.SaveChanges();
					sReturn_Text = "Student Other Details Saved Successfully";
				}
			}
			catch (Exception ex)
			{
				sReturn_Text = "Error";
			}

			return Json(sReturn_Text, JsonRequestBehavior.AllowGet);
		}


		public ActionResult AddStudentSiblingAndPrevSchoolDetail()
		{
			return View();
		}

		[HttpPost]
		public JsonResult AddStudentPrevSchoolDetails(Student_Prev_School_Details stuPrevSchoolDetail)
		{
			stuPrevSchoolDetail.Created_On = DateTime.Now;
			stuPrevSchoolDetail.Academic_Year = GetAcademicYear();
			stuPrevSchoolDetail.Is_Active = true;
			List<Student_PrevSchoolList_ViewModel> addedStudentPrevSchoolDetails = new List<Student_PrevSchoolList_ViewModel>();

			addedStudentPrevSchoolDetails = GetStudentPrevSchoolDetails(stuPrevSchoolDetail);
			return Json(new { items = addedStudentPrevSchoolDetails }, JsonRequestBehavior.AllowGet);
		}

		public List<Student_PrevSchoolList_ViewModel>  getStudentPrevSchoolDetail ()
		{
			long nAcademicYear = GetAcademicYear();
			long nStudent_Id = Convert.ToInt64(TempData.Peek("Student_Id"));
			List<Student_PrevSchoolList_ViewModel> addedStudentPrevSchoolDetails = new List<Student_PrevSchoolList_ViewModel>();
				try
				{

					using (var dbcontext = new SchoolERPDBContext())
					{
						addedStudentPrevSchoolDetails = (from school in dbcontext.School
															join prevSchooldetails in dbcontext.Student_Prev_School_Detail on school.Id equals prevSchooldetails.School_Id
															where prevSchooldetails.Student_Id == nStudent_Id && prevSchooldetails.Academic_Year == nAcademicYear && prevSchooldetails.Is_Deleted == null || prevSchooldetails.Is_Deleted == false
															select new Student_PrevSchoolList_ViewModel
															{
																Id = prevSchooldetails.Student_PrevSchool_Id,
																Name = school.Name,
																From_Year = prevSchooldetails.From_Year,
																To_Year = prevSchooldetails.To_Year,
																Leaving_Reason = prevSchooldetails.Leaving_Reason,
																Updated_On = prevSchooldetails.Updated_On,
																Student_Id = prevSchooldetails.Student_Id,
																Comments = prevSchooldetails.Comments

						}).ToList();
					}
				}
				catch (Exception ex)
				{

				}

			return addedStudentPrevSchoolDetails;
		  }


		public List<Student_SiblingList_ViewModel> getStudenSiblingDetail()
		{
			long nAcademicYear = GetAcademicYear();
			long nStudent_Id = Convert.ToInt64(TempData.Peek("Student_Id"));
			List<Student_SiblingList_ViewModel> addedStudentSiblingDetails = new List<Student_SiblingList_ViewModel>();
			try
			{

				using (var dbcontext = new SchoolERPDBContext())
				{
					addedStudentSiblingDetails = (from stu in dbcontext.Student
													 join studentSiblingDetail in dbcontext.Student_Sibling_Detail on stu.Student_Id equals studentSiblingDetail.Sibling_Student_Id
													 join cls in dbcontext.Class on stu.Class_Id equals cls.Id
													 join sec in dbcontext.Section on stu.Section_Id equals sec.Id
													 where studentSiblingDetail.Student_Id == nStudent_Id && studentSiblingDetail.Academic_Year == nAcademicYear && studentSiblingDetail.Is_Deleted == null || studentSiblingDetail.Is_Deleted == false
													 select new Student_SiblingList_ViewModel
													 {

														 Sibling_Detail_Id = studentSiblingDetail.Sibling_Detail_Id,
														 Class = cls.Name ,
														 Section = sec.Name,
														 Roll_No = stu.Roll_No,
														 Student_Name = stu.First_Name + "  " + stu.Last_Name,
														 Updated_On = studentSiblingDetail.Updated_On,
														 Student_Id = studentSiblingDetail.Student_Id,
														 Sibling_Relation = studentSiblingDetail.Sibling_Relation
														

													 }).ToList();
				}
			}
			catch (Exception ex)
			{

			}

			return addedStudentSiblingDetails;
		}
		[HttpPost]
		public JsonResult GetStudentPrevSchoolDetailsOnPageLoad()
		{
			List<Student_PrevSchoolList_ViewModel> addedStudentPrevSchoolDetails = new List<Student_PrevSchoolList_ViewModel>();

			addedStudentPrevSchoolDetails = getStudentPrevSchoolDetail();

			return Json(new { items = addedStudentPrevSchoolDetails }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetStudentSiblingDetailsOnPageLoad()
		{
			List<Student_SiblingList_ViewModel> addedStudentSiblingDetails = new List<Student_SiblingList_ViewModel>();

			addedStudentSiblingDetails = getStudenSiblingDetail();		

			return Json(new { items = addedStudentSiblingDetails }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult DeleteStudentPrevSchoolDetail(string id)
		{
			int idTobemodified = Convert.ToInt16(id);
			List<Student_PrevSchoolList_ViewModel> modifiedStudentPrevSchoolDetails = new List<Student_PrevSchoolList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				var studentPrevSchoolDetailT0bedeleted = dbcontext.Student_Prev_School_Detail.Find(idTobemodified);
				studentPrevSchoolDetailT0bedeleted.Is_Deleted = true;
				studentPrevSchoolDetailT0bedeleted.Updated_On = DateTime.Now;
				studentPrevSchoolDetailT0bedeleted.Updated_By = 5;
				dbcontext.Entry(studentPrevSchoolDetailT0bedeleted).CurrentValues.SetValues(studentPrevSchoolDetailT0bedeleted);
				dbcontext.SaveChanges();
			}
			modifiedStudentPrevSchoolDetails = getStudentPrevSchoolDetail();
			return Json(new { items =  modifiedStudentPrevSchoolDetails}, JsonRequestBehavior.AllowGet);
		}


		public JsonResult DeleteStudentSiblingDetail(string id)
		{
			int idTobemodified = Convert.ToInt16(id);
			List<Student_SiblingList_ViewModel> modifiedStudentSiblingDetails = new List<Student_SiblingList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				var studentSiblingDetailTobedeleted = dbcontext.Student_Sibling_Detail.Find(idTobemodified);
				studentSiblingDetailTobedeleted.Is_Deleted = true;
				studentSiblingDetailTobedeleted.Updated_On = DateTime.Now;
				studentSiblingDetailTobedeleted.Updated_By = 5;
				dbcontext.Entry(studentSiblingDetailTobedeleted).CurrentValues.SetValues(studentSiblingDetailTobedeleted);
				dbcontext.SaveChanges();
			}
			modifiedStudentSiblingDetails = getStudenSiblingDetail();
			return Json(new { items = modifiedStudentSiblingDetails }, JsonRequestBehavior.AllowGet);
		}

		public List<Student_PrevSchoolList_ViewModel>  GetStudentPrevSchoolDetails(Student_Prev_School_Details stuPrevSchoolDetail)
			{
			long nAcademicYear = GetAcademicYear();
			long nStudent_Id = Convert.ToInt64(TempData.Peek("Student_Id"));
			stuPrevSchoolDetail.Student_Id = nStudent_Id;
			List<Student_PrevSchoolList_ViewModel> addedStudentPrevSchoolDetails = new List<Student_PrevSchoolList_ViewModel>();
			try
			{
				using (var dbcontext = new SchoolERPDBContext())
				{
					dbcontext.Student_Prev_School_Detail.Add(stuPrevSchoolDetail);
					dbcontext.SaveChanges();
				}

				using (var dbcontext = new SchoolERPDBContext())
				{
					addedStudentPrevSchoolDetails = (from school in dbcontext.School
													 join prevSchooldetails in dbcontext.Student_Prev_School_Detail on school.Id equals prevSchooldetails.School_Id
													 where prevSchooldetails.Student_Id == nStudent_Id && prevSchooldetails.Academic_Year == nAcademicYear && prevSchooldetails.Is_Deleted == null || prevSchooldetails.Is_Deleted == false
													 select new Student_PrevSchoolList_ViewModel
													 {
														 Id = prevSchooldetails.Student_PrevSchool_Id,
														 Name = school.Name,
														 From_Year = prevSchooldetails.From_Year,
														 To_Year = prevSchooldetails.To_Year,
														 Leaving_Reason = prevSchooldetails.Leaving_Reason,
														 Updated_On = prevSchooldetails.Updated_On,
														 Student_Id = prevSchooldetails.Student_Id,
														 Comments = prevSchooldetails.Comments

													 }).ToList();
				}
				}
			catch (Exception ex)
			{

			}

					return addedStudentPrevSchoolDetails;
			}


		public class Select2Model
		{
			public string id { get; set; }
			public string text { get; set; }
		}

		public ActionResult GetEmployeeList(string q)

		{	var list = new List<Select2Model>();

		List<School> schoolList = new List<School>();

			using (var dbcontext = new SchoolERPDBContext())
			{

				if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
				{

				schoolList =  (from school in dbcontext.School
								   where school.Name.Contains(q.ToLower())
								   select new
								   {
									   Text = school.Name,
									  Id = school.Id
								   }).ToList().
								   Select(x=> new School

					 {
						
						 Name = x.Text,
						 Id = x.Id

					}).ToList();

				}
			}


			for (int i = 0; i < schoolList.Count(); i++)
			{
				Select2Model oSelect2Model = new Select2Model();
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

		public ActionResult populateCountriesList(string sSearchTerm)
		{
			return Json(new { items = GetCountriesList(sSearchTerm) }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetStudentList(string q)

		{
			var list = new List<Select2Model>();

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
									  Text = stu.First_Name + " " +stu.Last_Name + " " +
									  Environment.NewLine
									  + "["+stu.Roll_No + "]"+ " - " + cls.Name + " " + sec.Name,
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
				Select2Model oSelect2Model = new Select2Model();
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

		public byte[] imageToByteArray(System.Drawing.Image imageIn)
		{
			MemoryStream ms = new MemoryStream();
			imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
			return ms.ToArray();
		}

		[HttpPost]
		public JsonResult Upload(HttpPostedFileBase file)
	{
			long nStudent_Id = Convert.ToInt64( TempData.Peek("Student_Id"));
			Student studentToBeModified = new Student();
			
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
						studentToBeModified = dbcontext.Student.Find(nStudent_Id);
						studentToBeModified.Photo = bytes;
						dbcontext.Entry(studentToBeModified).CurrentValues.SetValues(studentToBeModified);
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

	


		[HttpPost]
		public ActionResult AddStudentSiblingDetails(Student_Sibling_Detail stuSiblingDetail)
		{
			stuSiblingDetail.Created_On = DateTime.Now;
			stuSiblingDetail.Academic_Year = GetAcademicYear();

			stuSiblingDetail.Is_Active = true;
			List<Student_SiblingList_ViewModel> addedStudentSiblingDetails = new List<Student_SiblingList_ViewModel>();

			addedStudentSiblingDetails = GetStudentSiblingDetails(stuSiblingDetail);

			return Json(new { items = addedStudentSiblingDetails }, JsonRequestBehavior.AllowGet);


		}


		public List<Student_SiblingList_ViewModel> GetStudentSiblingDetails(Student_Sibling_Detail stuSiblingDetail)
			{

			long nYear = GetAcademicYear();
			long nStudent_Id = Convert.ToInt64(TempData.Peek("Student_Id"));
			stuSiblingDetail.Student_Id = nStudent_Id;
			List <Student_SiblingList_ViewModel> addedStudentSiblingDetails = new List<Student_SiblingList_ViewModel>();
			try
			{
				using (var dbcontext = new SchoolERPDBContext())
				{
					if (stuSiblingDetail != null)
					{

						dbcontext.Student_Sibling_Detail.Add(stuSiblingDetail);
						dbcontext.SaveChanges();
					}
				}

				using (var dbcontext = new SchoolERPDBContext())
				{
					addedStudentSiblingDetails = (from stu in dbcontext.Student
													 join studentSiblingDetail in dbcontext.Student_Sibling_Detail on stu.Student_Id equals studentSiblingDetail.Sibling_Student_Id
													 join cls in dbcontext.Class on stu.Class_Id equals cls.Id
													 join sec in dbcontext.Section on stu.Section_Id equals sec.Id
													 where studentSiblingDetail.Student_Id == nStudent_Id && studentSiblingDetail.Academic_Year == nYear && studentSiblingDetail.Is_Deleted == null || studentSiblingDetail.Is_Deleted == false
													 select new Student_SiblingList_ViewModel
													 {

														 Sibling_Detail_Id = studentSiblingDetail.Sibling_Detail_Id,
														 Class  = cls.Name,
														 Section = sec.Name,													 
														 Student_Name = stu.First_Name + "  " + stu.Last_Name,	
														 Roll_No = stu.Roll_No,													
														 Updated_On = studentSiblingDetail.Updated_On,
														 Student_Id = studentSiblingDetail.Student_Id,
														  Sibling_Relation = studentSiblingDetail.Sibling_Relation


}).ToList();
				}


			}
			catch (Exception ex)
			{

			}

			return addedStudentSiblingDetails;
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