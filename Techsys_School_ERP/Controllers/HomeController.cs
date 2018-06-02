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

namespace Techsys_School_ERP.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
		//[Authorize]
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Index1()
		{
			return View();
		}

		[Authorize]
		public ActionResult AdminHomePage()
		{
			User usr = new Model.User();
			string sRole = string.Empty;

			using (var dbcontext = new SchoolERPDBContext())
			{
				if (TempData["sUserId"] != null)
				{
					var sUser_Id = Convert.ToString(TempData["sUserId"]);
					usr.Name = dbcontext.Users.Where(x => x.User_Id == sUser_Id).ToList()[0].Name;
					sRole = (from usrs in dbcontext.Users
									join rle in dbcontext.User_Roles on usrs.Role_Id equals rle.Id
								where usrs.User_Id == sUser_Id
							select rle.Name).ToList()[0];
				}
				

			}
			if (sRole == "Admin" || sRole == "SuperAdmin")
			{
				usr.Role_Id = 1;
			}
			else if (sRole == "Parent" || sRole == "Student")
			{
				usr.Role_Id = 2;
			}
			else
			{
				usr.Role_Id = 3;
			}
		
				return View("Index1",usr);
		}


		[HttpPost]
		public JsonResult Login(string User_Id, string Password)
		{
			string sReturnText; 

			using (var dbcontext = new SchoolERPDBContext())
			{
				if (dbcontext.Users.Any(o => o.User_Id == User_Id) )
				{
					if (dbcontext.Users.Any(o => o.Password == Password))
					{
						TempData["sUserId"] = User_Id;

						sReturnText = "Login Success";

						FormsAuthentication.SetAuthCookie(User_Id, false);

						return Json("Login Success", JsonRequestBehavior.AllowGet);
					}

					sReturnText = "Password is Wrong";
					return Json(sReturnText, JsonRequestBehavior.AllowGet);
				}
				else
				{
					sReturnText = "User Name is Wrong";
					return new JsonResult { Data = sReturnText.ToString(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
					//return Json(sReturnText, JsonRequestBehavior.AllowGet);

				}

			}
			
		}


		public ActionResult MultiSelectPOC()
		{

			using (var dbcontext = new SchoolERPDBContext())
			{
				var result = (from skills in dbcontext.Exam select skills).ToList();
				if (result != null)
				{
					ViewBag.mySkills = result.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}

		
			return View();
		}



	}
}