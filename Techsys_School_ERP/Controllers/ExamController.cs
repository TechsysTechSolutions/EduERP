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

namespace Techsys_School_ERP.Controllers
{
    public class ExamController : Controller
    {
		// GET: Exam/ExamList
		[Authorize(Roles = "Admin")]
		public ActionResult ExamList()
        {
			List<ExamList_ViewModel> examListViewModel = new List<ExamList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				examListViewModel = (from usr in dbcontext.Users
										join
									exam in dbcontext.Exam on usr.Id equals exam.Created_By
										where exam.Is_Deleted == null || exam.Is_Deleted == false
										select new ExamList_ViewModel
										{
											Id =  exam.Id,
											Name = exam.Name,
											Academic_Year = usr.Academic_Year,
											User_Id = usr.User_Id,
											Is_Deleted = usr.Is_Deleted,
											Created_On = exam.Created_On,
											Created_By = exam.Created_By

										}).ToList();


			}
			return View(examListViewModel);
		}


		[HttpPost]
		public JsonResult CreateExam(string Exam_Name)
		{
			try
			{
				Exam newExam = new Exam();
				int nUser_Id;
				using (var dbcontext = new SchoolERPDBContext())
				{
					nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
				}
				using (var dbcontext = new SchoolERPDBContext())
				{
					newExam.Name = Exam_Name;
					newExam.Academic_Year = (DateTime.Now.Month <= 4) ? DateTime.Now.Year - 1 : DateTime.Now.Year;
					newExam.Is_Active = true;
					newExam.Created_By = nUser_Id;
					newExam.Created_On = DateTime.Now;
					if (dbcontext.User_Roles.Where(a => a.Name == Exam_Name.Trim().ToString() && a.Is_Deleted == false).Count() == 0)
					{
						dbcontext.Exam.Add(newExam);
						dbcontext.SaveChanges();
						return Json("OK", JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json("Exam Already Exists.", JsonRequestBehavior.AllowGet);
					}
				}

			}
			catch (Exception e)
			{
				return Json("Failure", JsonRequestBehavior.AllowGet);
			}


		}


		public async Task<ActionResult> Delete(int id)
		{
			using (var dbcontext = new SchoolERPDBContext())
			{


				Exam exam = await dbcontext.Exam.FindAsync(id);
				if (exam != null)
				{
					exam.Is_Deleted = true;
					dbcontext.Entry(exam).CurrentValues.SetValues(exam);
					dbcontext.Entry(exam).State = EntityState.Modified;
					await dbcontext.SaveChangesAsync();
				}
			}

			return RedirectToAction("ExamList");
		}
	}
}