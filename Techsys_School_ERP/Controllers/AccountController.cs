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
	public class AccountController : Controller
	{
		// GET: Account
		[Authorize(Roles = "Admin")]
		public ActionResult RoleList()
		{
			List<User_Role> usrRoleList = new List<User_Role>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				usrRoleList = dbcontext.User_Roles.Where(x => x.Is_Deleted == null).ToList();
			}
			return View(usrRoleList);
		}

		// GET: Account/UserList
		[Authorize(Roles = "Admin")]
		public ActionResult UserList()
		{
			//List<User> usrList = new List<User>();
			//using (var dbcontext = new SchoolERPDBContext())
			//{
			//	usrList = dbcontext.Users.Where(x => x.Is_Deleted == null).ToList();
			//}
			//return View(usrList);
			List<UserList_ViewModel> usrRoleListViewModel = new List<UserList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				usrRoleListViewModel = (from usr in dbcontext.Users
										join
		   usrrole in dbcontext.User_Roles on usr.Role_Id equals usrrole.Id
										where usr.Is_Deleted == null || usr.Is_Deleted == false
										select new UserList_ViewModel
										{
											Name = usr.Name,
											Academic_Year = usr.Academic_Year,
											User_Id = usr.User_Id,
											Role_Name = usrrole.Name,
											Is_Deleted = usr.Is_Deleted,
											Updated_By = usr.Updated_By,
											Updated_On = usr.Updated_On

										}).ToList();


			}
			return View(usrRoleListViewModel);
		}


		// GET: Account/CreateUser
		[Authorize]
		public ActionResult CreateUser()
		{
			
			return View();
		}




		[HttpPost]
		public JsonResult CreateRole(string Role_Name)
		{
			try
			{
				User_Role newUserRole = new User_Role();
				int nUser_Id;
				using (var dbcontext = new SchoolERPDBContext())
				{
					nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
				}
				using (var dbcontext = new SchoolERPDBContext())
				{
					newUserRole.Name = Role_Name;
					newUserRole.Academic_Year = (DateTime.Now.Month <= 4) ? DateTime.Now.Year - 1 : DateTime.Now.Year;
					newUserRole.Is_Active = true;
					newUserRole.Created_By = nUser_Id;
					newUserRole.Created_On = DateTime.Now;
					if (dbcontext.User_Roles.Where(a => a.Name == Role_Name.Trim().ToString()  && a.Is_Deleted == false ).Count() == 0)
					{
						dbcontext.User_Roles.Add(newUserRole);
						dbcontext.SaveChanges();
						return Json("OK", JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json("Role Already Exists.", JsonRequestBehavior.AllowGet);
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


				User_Role userRole = await dbcontext.User_Roles.FindAsync(id);
				if (userRole != null)
				{
					userRole.Is_Deleted = true;
					dbcontext.Entry(userRole).CurrentValues.SetValues(userRole);
					dbcontext.Entry(userRole).State = EntityState.Modified;
					await dbcontext.SaveChangesAsync();
				}
			}

			return RedirectToAction("RoleList");
		}


	}

}