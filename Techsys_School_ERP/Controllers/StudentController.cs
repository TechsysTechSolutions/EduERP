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
					student.Student_Id = nStudent_Id;
					
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
			GetSecondLanguage();
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
			string sPassword = CreateRandomPassword(6);
			try
			{
				if (ModelState.IsValid)
				{
					using (var dbcontext = new SchoolERPDBContext())
					{
						//if (dbcontext.Student.Any(o => o.Email_Id == sEmail_Id))
						//{
						//	sReturn_Text = "Email Already Exists";
						//}
						//else
						//{
							dbcontext.Student.Add(newStudentToBeAdded);
							dbcontext.SaveChanges();
							

						MailController oMailController = new MailController();
						oMailController.SendMail(newStudentToBeAdded.Email_Id, newStudentToBeAdded.First_Name.Substring(0, 4) + newStudentToBeAdded.Student_Id , sPassword );

						//To create the UserId in the User Table for Login 

						//}
					}

					using (var dbcontext = new SchoolERPDBContext())
					{
						User newUserToBeAdded = new User();
						newUserToBeAdded.Name = newStudentToBeAdded.First_Name + " " + newStudentToBeAdded.Last_Name;
						newUserToBeAdded.EmailConfirmed = true;
						newUserToBeAdded.Created_By = 5;
						newUserToBeAdded.User_Id = newStudentToBeAdded.First_Name.Substring(0, 4) + newStudentToBeAdded.Student_Id;
						newUserToBeAdded.Password = sPassword;
						newUserToBeAdded.Is_Active = true;
						newUserToBeAdded.Academic_Year = newStudentToBeAdded.Academic_Year;
						newUserToBeAdded.Role_Id = Convert.ToInt16(Role_Type.Student);
						sReturn_Text = "SuccessFully Added";
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
			newStudentOtherDetailsToBeAdded.Mother_Occupation_Id = Student_Other_Detail.Mother_Occupation_Id;
			newStudentOtherDetailsToBeAdded.Father_Occupation_Id = Student_Other_Detail.Father_Occupation_Id;
			newStudentOtherDetailsToBeAdded.Is_Active = true;
			newStudentOtherDetailsToBeAdded.Created_By = 5;
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

		public ActionResult AddOrUploadStudentRelatedDocuments()
		{
			long nStudent_Id = Convert.ToInt64(TempData.Peek("Student_Id"));


			using (var dbcontext = new SchoolERPDBContext())
			{
				if (dbcontext.Student_Document.Where(x => x.Student_Id == nStudent_Id).ToList().Count() > 0)
				{
					return RedirectToAction("EditUploadedStudentRelatedDocuments");
					
				}
				else
				{
					return View();
				}
			}
					
		}

		public ActionResult EditUploadedStudentRelatedDocuments()
		{

			Student_Document studentDocumentToBeEdited = new Student_Document();

			long nStudent_Id = Convert.ToInt64(TempData.Peek("Student_Id"));


			using (var dbcontext = new SchoolERPDBContext())
			{
				if (dbcontext.Student_Document.Where(x => x.Student_Id == nStudent_Id).ToList().Count() > 0)
				{

					var studentDocumentId = dbcontext.Student_Document.Where(x => x.Student_Id == nStudent_Id).FirstOrDefault().Id;
					var studentDocumentToEdit = dbcontext.Student_Document.Find(studentDocumentId);


					if (studentDocumentToEdit.document1 != null)
					{
						ViewBag.Document1 = ConvertByteStreamToString(studentDocumentToEdit.document1);
					}
					else
					{
						ViewBag.Document1 = null;

					}

					if (studentDocumentToEdit.document2 != null)
					{
						ViewBag.Document2 = ConvertByteStreamToString(studentDocumentToEdit.document2);
					}
					else
					{
						ViewBag.Document2 = null;

					}

					if (studentDocumentToEdit.document3 != null)
					{
						ViewBag.Document3 = ConvertByteStreamToString(studentDocumentToEdit.document3);
					}

					if (studentDocumentToEdit.document4 != null)
					{
						ViewBag.Document4 = ConvertByteStreamToString(studentDocumentToEdit.document4);
					}

					if (studentDocumentToEdit.document5 != null)
					{
						ViewBag.Document5 = ConvertByteStreamToString(studentDocumentToEdit.document5);
					}

					if (studentDocumentToEdit.document6 != null)
					{
						ViewBag.Document6 = ConvertByteStreamToString(studentDocumentToEdit.document6);
					}
					return View(studentDocumentToBeEdited);
				}
				else
				{
					return View();
				}


			}

			

			
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


		public ActionResult populateHostelRoomList(string sSearchTerm)
		{
			return Json(SearchandGetHostelRoomList(sSearchTerm), JsonRequestBehavior.AllowGet);
		}


		public ActionResult populateVehicleList(string sSearchTerm)
		{
			return Json(SearchandGetVehicleList(sSearchTerm), JsonRequestBehavior.AllowGet);
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



		public JsonResult FetchHostelRoomBasedOnId(int nHostelRoomId)
		{	
			return Json(GetHostelRoomBasedOnId(nHostelRoomId), JsonRequestBehavior.AllowGet);
		}


		public JsonResult FetchVehicleNoBasedOnId(int nVehicleNo)
		{			
			return Json(GetTransportBasedOnId(nVehicleNo), JsonRequestBehavior.AllowGet);
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
		public JsonResult Upload(HttpPostedFileBase file, long? student_Id)
	{
			//long nStudent_Id = Convert.ToInt64(TempData.Peek("Student_Id"));
			long nStudent_Id = Convert.ToInt64(student_Id);
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
				return Json("OK", JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);

			}
	
			//return null;

		}


		[HttpPost]
		public JsonResult UploadStudentDocuments(HttpPostedFileBase file1 , HttpPostedFileBase file2 , HttpPostedFileBase file3 , HttpPostedFileBase file4, HttpPostedFileBase file5, HttpPostedFileBase file6)
		{
			//long nStudent_Id = 
			//	long nStudent_Id = Convert.ToInt64(TempData.Peek("Student_Id"));
			long nStudent_Id = Convert.ToInt64(TempData.Peek("Student_Id")); ;
			Student_Document StudentDocumentToBeAddededOrUpdated = new Student_Document();

			HttpPostedFileBase[] fileBaseArr = new HttpPostedFileBase[6];

			

		    

			//byte[] bytes1, bytes2 , bytes3, bytes4, bytes5, bytes6;

			try
			{
				
					
					using (var dbcontext = new SchoolERPDBContext())
					{
					
					if (file1 != null)
					{
						StudentDocumentToBeAddededOrUpdated.document1 = ConvertFiletoBYteStream(file1);
					}
					else
					{
					}
					if (file2 != null)
					{
						StudentDocumentToBeAddededOrUpdated.document2 = ConvertFiletoBYteStream(file2);
					}
					if (file3 != null)
					{
						StudentDocumentToBeAddededOrUpdated.document3 = ConvertFiletoBYteStream(file3);
					}
					if (file4 != null)
					{
						StudentDocumentToBeAddededOrUpdated.document4 = ConvertFiletoBYteStream(file4);
					}
					if (file5 != null)
					{
						StudentDocumentToBeAddededOrUpdated.document5 = ConvertFiletoBYteStream(file5);
					}
					if (file6 != null)
					{
						StudentDocumentToBeAddededOrUpdated.document6 = ConvertFiletoBYteStream(file6);
					}

					if (dbcontext.Student_Document.Where(x => x.Student_Id == nStudent_Id).ToList().Count() > 0)
					{
						var studentDocumentId = dbcontext.Student_Document.Where(x=>x.Student_Id == nStudent_Id).FirstOrDefault().Id;
						var studentDocToBeModified = dbcontext.Student_Document.Find(studentDocumentId);
						studentDocToBeModified.document1 = StudentDocumentToBeAddededOrUpdated.document1 == null ? studentDocToBeModified.document1 : StudentDocumentToBeAddededOrUpdated.document1;
						studentDocToBeModified.document2 = StudentDocumentToBeAddededOrUpdated.document2 == null ? studentDocToBeModified.document2 : StudentDocumentToBeAddededOrUpdated.document2;
						studentDocToBeModified.document3 = StudentDocumentToBeAddededOrUpdated.document3 == null ? studentDocToBeModified.document3 : StudentDocumentToBeAddededOrUpdated.document3;
						studentDocToBeModified.document4 = StudentDocumentToBeAddededOrUpdated.document4 == null ? studentDocToBeModified.document4 : StudentDocumentToBeAddededOrUpdated.document4;
						studentDocToBeModified.document5 = StudentDocumentToBeAddededOrUpdated.document5 == null ? studentDocToBeModified.document5 : StudentDocumentToBeAddededOrUpdated.document5;
						studentDocToBeModified.document6 = StudentDocumentToBeAddededOrUpdated.document6 == null ? studentDocToBeModified.document6 : StudentDocumentToBeAddededOrUpdated.document6;

						dbcontext.Entry(studentDocToBeModified).CurrentValues.SetValues(studentDocToBeModified);
						dbcontext.SaveChanges();
					}
					else
					{
						StudentDocumentToBeAddededOrUpdated.Student_Id = nStudent_Id;
						dbcontext.Student_Document.Add(StudentDocumentToBeAddededOrUpdated);
						dbcontext.SaveChanges();
					}
						
						//
					}


				
				return Json("Student Documents Successfully Uploaded", JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);

			}

			

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


		#region StudentList

		public ActionResult StudentList()
		{
			long nYear = GetAcademicYear();
			List<Student_ViewModel> studentVieModelList = new List<Student_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				studentVieModelList = (from stu in dbcontext.Student 
									   join cls in dbcontext.Class on stu.Class_Id equals cls.Id
									   join sec in dbcontext.Section on stu.Section_Id equals sec.Id
									   where  stu.Academic_Year == sec.Academic_Year &&  stu.Is_Deleted == false &&  stu.Academic_Year == nYear
									   select new Student_ViewModel
									   {

										 Id = stu.Student_Id,
										 Name = stu.First_Name + " " + stu.Last_Name + " " + stu.Middle_Name,
										 ClassAndSection = cls.Name + " - " + sec.Name ,
										 Emergency_ContactNo = stu.Phone_No1  ,
										 Academic_Year = nYear,
										 Created_On = stu.Created_On



									   }).OrderBy(x => x.Created_On).ToList();
			}
			return View(studentVieModelList);
		}
		#endregion

		#region DeleteStudent

		public ActionResult Delete(long id)
		{
			using (var dbcontext = new SchoolERPDBContext())
			{
				var studenToBeDeleted = dbcontext.Student.Find(id);
				studenToBeDeleted.Is_Deleted = true;
				studenToBeDeleted.Is_Active = true;
				studenToBeDeleted.Updated_By = 5;
				studenToBeDeleted.Updated_On = DateTime.Now;

				dbcontext.Entry(studenToBeDeleted).State = EntityState.Modified;
				dbcontext.SaveChanges();

			}
				return RedirectToAction("StudentList");
		}

		#endregion

		#region EditStudent

		public ActionResult Edit(long? id)
		{

			Student studentToBeEdited = new Student();
			GetBloodGroup();
			GetGender();
			
			GetCountries();
			GetClass();

			using (var dbcontext = new SchoolERPDBContext())
			{
				var studentEdited = dbcontext.Student.Find(id);
				GetSectionForClass(Convert.ToString(studentEdited.Class_Id));
				GetStatesForCountry(Convert.ToString(studentEdited.Country_Id));
				GetCitiesForState(Convert.ToString(studentEdited.State_Id));
				studentToBeEdited.Student_Id = Convert.ToInt64(id);
				

				studentToBeEdited = studentEdited;

				byte[] byteData = studentEdited.Photo;
				string imgDataURL = string.Empty;

				if (byteData != null)
				{
					//Convert byte arry to base64string   
					string imreBase64Data = Convert.ToBase64String(byteData);
					imgDataURL  = string.Format("data:image/png;base64,{0}", imreBase64Data);
					//Passing image data in viewbag to view  
				
				}
				else
				{
					imgDataURL = string.Empty;
				}

				ViewBag.ImageData = imgDataURL;


			}

			return View(studentToBeEdited);
		}

		
		public JsonResult FetchCountryNameBasedOnId(string countryId)
		{
			return Json(new { items = GetCountryNameBasedOnId(countryId) }, JsonRequestBehavior.AllowGet);

		}


		[HttpPost]
		public ActionResult EditStudent(Student student)
		{
			string sReturnText = string.Empty;
			try
			{
				if (ModelState.IsValid)
				{
					using (var dbcontext = new SchoolERPDBContext())
					{
						Student studentToBeUpdated = dbcontext.Student.Find(student.Student_Id);

						studentToBeUpdated.First_Name = student.First_Name;
						studentToBeUpdated.Last_Name = student.Last_Name;
						studentToBeUpdated.Middle_Name = student.Middle_Name;
						studentToBeUpdated.Father_Name = student.Father_Name;
						studentToBeUpdated.Mother_Name = student.Mother_Name;
						studentToBeUpdated.Aadhar_No = student.Aadhar_No;
						studentToBeUpdated.Address_Line1 = student.Address_Line1;
						studentToBeUpdated.Address_Line2 = student.Address_Line2;
						studentToBeUpdated.Blood_Group_Id = student.Blood_Group_Id;
						studentToBeUpdated.Email_Id = student.Email_Id;
						studentToBeUpdated.DOB = student.DOB;
						studentToBeUpdated.Enrollment_Date = student.Enrollment_Date;
						studentToBeUpdated.City_Id = student.City_Id;
						studentToBeUpdated.State_Id = student.State_Id;
						studentToBeUpdated.Country_Id = student.Country_Id;
						studentToBeUpdated.Blood_Group_Id = student.Blood_Group_Id;
						studentToBeUpdated.Is_HostelStudent = student.Is_HostelStudent;
						studentToBeUpdated.Phone_No1 = student.Phone_No1;
						studentToBeUpdated.Phone_No2 = student.Phone_No2;
						studentToBeUpdated.Section_Id = student.Section_Id;
						studentToBeUpdated.Class_Id = student.Class_Id;
						studentToBeUpdated.Gender_Id = student.Gender_Id;
						studentToBeUpdated.Pincode = student.Pincode;
						studentToBeUpdated.Updated_On = DateTime.Now;
						studentToBeUpdated.Updated_By = 5;
					

						if (student.Email_Id.Trim() != studentToBeUpdated.Email_Id.Trim())
						{
							if (!CheckIfEmailAlreadyExists(student.Email_Id, true, false))
							{

								dbcontext.Entry(studentToBeUpdated).State = EntityState.Modified;
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
							dbcontext.Entry(studentToBeUpdated).State = EntityState.Modified;
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

		public ActionResult EditStudentOtherDetails(string sStudent_Id)
		{
			long nStudent_Id = Convert.ToInt64(sStudent_Id);
			Student_Other_Details student_OtherDetails_ToBeEdited = new Student_Other_Details();
			GetCategory();
			GetSecondLanguage();
			GetOccupation();

			using (	var dbcontext = new SchoolERPDBContext())
			{
				if (dbcontext.Student_Other_Details.Where(x => x.Student_Id == nStudent_Id).Count() == 1)
				{
					var id = dbcontext.Student_Other_Details.Where(x => x.Student_Id == nStudent_Id).FirstOrDefault().StudentDetail_Id;
					var studen_OtherDetailsTotEdit = dbcontext.Student_Other_Details.Find(id);
					//GetSectionForClass(Convert.ToString(studentEdited.Class_Id));
					//GetStatesForCountry(Convert.ToString(studentEdited.Country_Id));
					//GetCitiesForState(Convert.ToString(studentEdited.State_Id));
					student_OtherDetails_ToBeEdited = studen_OtherDetailsTotEdit;
				}
				else
				{
					student_OtherDetails_ToBeEdited.Student_Id = nStudent_Id;
				}

			}

			return View(student_OtherDetails_ToBeEdited);
		//	return View();
		}

		public ActionResult EditAndSaveStudentOtherDetails(Student_Other_Details Student_Other_Detail)
		{
			long nStudent_Id = Convert.ToInt64(Student_Other_Detail.Student_Id);
			Student_Other_Details student_OtherDetails_ToBeEdited = new Student_Other_Details();
			string sReturn_Text = string.Empty;

			try
			{
				using (var dbcontext = new SchoolERPDBContext())
				{
					//var id = dbcontext.Student_Other_Details.Where(x => x.Student_Id == nStudent_Id).FirstOrDefault().StudentDetail_Id;
					if (dbcontext.Student_Other_Details.Where(x => x.Student_Id == nStudent_Id).Count() == 1)
					{

						var studen_OtherDetailsTotEdit = dbcontext.Student_Other_Details.Find(Student_Other_Detail.StudentDetail_Id);

						studen_OtherDetailsTotEdit.Identification_Mark1 = Student_Other_Detail.Identification_Mark1;
						studen_OtherDetailsTotEdit.Identification_Mark2 = Student_Other_Detail.Identification_Mark2;
						studen_OtherDetailsTotEdit.Is_Allergic = Student_Other_Detail.Is_Allergic;
						studen_OtherDetailsTotEdit.Allergy_Details = Student_Other_Detail.Allergy_Details;
						studen_OtherDetailsTotEdit.Father_Occupation_Id = Student_Other_Detail.Father_Occupation_Id;
						studen_OtherDetailsTotEdit.Father_Designation = Student_Other_Detail.Father_Designation;
						studen_OtherDetailsTotEdit.Father_Company_Name = Student_Other_Detail.Father_Company_Name;
						studen_OtherDetailsTotEdit.Father_Office_Address = Student_Other_Detail.Father_Office_Address;
						studen_OtherDetailsTotEdit.Father_Annual_Income = Student_Other_Detail.Father_Annual_Income;
						studen_OtherDetailsTotEdit.Mother_Occupation_Id = Student_Other_Detail.Mother_Occupation_Id;
						studen_OtherDetailsTotEdit.Mother_Designation = Student_Other_Detail.Mother_Designation;
						studen_OtherDetailsTotEdit.Mother_Company_Name = Student_Other_Detail.Mother_Company_Name;
						studen_OtherDetailsTotEdit.Mother_Office_Address = Student_Other_Detail.Mother_Office_Address;
						studen_OtherDetailsTotEdit.Mother_Annual_Income = Student_Other_Detail.Mother_Annual_Income;
						studen_OtherDetailsTotEdit.Medical_History_Details = Student_Other_Detail.Medical_History_Details;
						studen_OtherDetailsTotEdit.Second_Language_Opted_Id = Student_Other_Detail.Second_Language_Opted_Id;
						studen_OtherDetailsTotEdit.Updated_On = DateTime.Now;


						dbcontext.Entry(studen_OtherDetailsTotEdit).State = EntityState.Modified;
						dbcontext.SaveChanges();
						
					}
					else
					{
						Student_Other_Detail.Created_On = DateTime.Now;
						Student_Other_Detail.Created_By = 5;
						dbcontext.Student_Other_Details.Add(Student_Other_Detail);
						dbcontext.SaveChanges();
					}

					TempData["Student_Id"] = nStudent_Id;
					TempData.Keep("Student_Id");
					sReturn_Text = "OK";

				}
			}
			catch (Exception ex)
			{
				sReturn_Text = ex.InnerException.Message.ToString();
			}

			return Json(sReturn_Text, JsonRequestBehavior.AllowGet);
			//	return View();
		}
		#endregion




	}


}