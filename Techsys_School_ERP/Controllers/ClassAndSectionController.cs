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
    public class ClassAndSectionController : Controller
    {
		#region Class
		// GET: ClassAndSection
		public ActionResult ClassList()
        {
			List<ClassList_ViewModel> classListViewModel = new List<ClassList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				classListViewModel = (from usr in dbcontext.Users
									join cls in dbcontext.Class on usr.Id equals cls.Created_By
									where cls.Is_Deleted == null || cls.Is_Deleted == false
									select new ClassList_ViewModel
									{
										Id = cls.Id,
										Name = cls.Name,
										Academic_Year = usr.Academic_Year,
										User_Id = usr.User_Id,
										Created_On = cls.Created_On,
										Created_By = cls.Created_By

									}).ToList();


			}
			using (var dbcontext = new SchoolERPDBContext())
			{
				var result = (from skills in dbcontext.Exam select skills).ToList();
				if (result != null)
				{
					ViewBag.mySkills = result.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
					return View(classListViewModel);
				}

			}
			return View(classListViewModel);
		}


	

		[HttpPost]
		public JsonResult CreateClass(string Class_Name)
		{
			try
			{
				Class newClass = new Class();
				int nUser_Id;
				using (var dbcontext = new SchoolERPDBContext())
				{
					nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
				}
				using (var dbcontext = new SchoolERPDBContext())
				{
					newClass.Name = Class_Name;
					newClass.Academic_Year = (DateTime.Now.Month <= 4) ? DateTime.Now.Year - 1 : DateTime.Now.Year;
					newClass.Is_Active = true;
					newClass.Created_By = nUser_Id;
					newClass.Created_On = DateTime.Now;
					if (dbcontext.User_Roles.Where(a => a.Name == Class_Name.Trim().ToString() && a.Is_Deleted == false).Count() == 0)
					{
						dbcontext.Class.Add(newClass);
						dbcontext.SaveChanges();
						return Json("OK", JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json("Class Already Exists.", JsonRequestBehavior.AllowGet);
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


				Class cls = await dbcontext.Class.FindAsync(id);
				if (cls != null)
				{
					cls.Is_Deleted = true;
					dbcontext.Entry(cls).CurrentValues.SetValues(cls);
					dbcontext.Entry(cls).State = EntityState.Modified;
					await dbcontext.SaveChangesAsync();
				}
			}

			return RedirectToAction("ClassList");
		}

	#endregion

	#region Section
	// GET: ClassAndSection
	public ActionResult SectionList()
	{
		List<SectionList_ViewModel> sectionListViewModel = new List<SectionList_ViewModel>();
		using (var dbcontext = new SchoolERPDBContext())
		{
			sectionListViewModel = (from usr in dbcontext.Users
									join cls in dbcontext.Class on  usr.Id equals cls.Created_By
								  join sec in dbcontext.Section on cls.Id equals sec.Class_Id
								  where sec.Is_Deleted == null || sec.Is_Deleted == false
								  orderby sec.Class_Id,sec.Name
								  select new SectionList_ViewModel
								  {
									  Id = sec.Id,
									  Name = cls.Name +" - "+ sec.Name  ,
									  Academic_Year = usr.Academic_Year,									  
									  Created_On = sec.Created_On,
									  User_Id = usr.User_Id

								  }).ToList();


		}

			using (var dbcontext = new SchoolERPDBContext())
			{
				var clsList = (from cls in dbcontext.Class select cls).ToList();
				if (clsList != null)
				{
					ViewBag.classList = clsList.Select(N => new SelectListItem { Text = N.Name, Value = N.Id.ToString() });
				}
			}


			return View(sectionListViewModel);
	}

		[HttpPost]
		public JsonResult CreateSection(string Class_Ids ,string Section_Name)
		{
			string[] ClassIdStrArr = Class_Ids.Split(',');
			Section newSection = new Section();
			int nUser_Id;
			string sReturnText = string.Empty;
			using (var dbcontext = new SchoolERPDBContext())
			{
				nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
			}
			int nCount = ClassIdStrArr.ToList().Count;
			int nLoopCount = 0;

			using (var dbcontext = new SchoolERPDBContext())
			{
				using (var transaction = dbcontext.Database.BeginTransaction())
				{
					try
					{
						foreach (string ClassId in ClassIdStrArr)
						{
							newSection.Class_Id = Convert.ToInt32(ClassId);
							newSection.Name = Section_Name;
							newSection.Academic_Year = (DateTime.Now.Month <= 4) ? DateTime.Now.Year - 1 : DateTime.Now.Year;
							newSection.Is_Active = true;
							newSection.Created_By = nUser_Id;
							newSection.Created_On = DateTime.Now;
							int nClassId = Convert.ToInt32(ClassId);
							if (dbcontext.Section.Where(a => a.Class_Id == nClassId && a.Name.Trim().Replace(" ", "") == Section_Name.Trim().Replace(" ", "").ToString() && (a.Is_Deleted == false || a.Is_Deleted == null)).Count() == 0)
							{
								dbcontext.Section.Add(newSection);
								dbcontext.SaveChanges();
								sReturnText = "OK";
								nLoopCount++;
								if (nLoopCount == nCount )
								{
									transaction.Commit();
								}

							}
							else
							{
								sReturnText = "Section Already Exists For Class";
								transaction.Rollback();
								break;

							}

						}

						
					}
					catch (Exception e)
					{
						transaction.Rollback();
						}
					}
				return Json(sReturnText, JsonRequestBehavior.AllowGet);
			}
		}
		#endregion
	}
}