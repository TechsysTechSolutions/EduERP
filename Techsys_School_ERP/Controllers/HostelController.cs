using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Techsys_School_ERP.DBAccess;
using Techsys_School_ERP.Model;
using Techsys_School_ERP.Model.ViewModel;

namespace Techsys_School_ERP.Controllers
{
    public class HostelController : CommonController
    {
        // GET: Hostel
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult HostelRoomList()
		{
			long nYear = GetAcademicYear();

			List<HostelList_ViewModel> hostelRoomObj = new List<HostelList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				if (dbcontext.Hostel_Room.Count() > 0)
				{
					hostelRoomObj = (from usr in dbcontext.Users
										  join hostelRoom in dbcontext.Hostel_Room on usr.Id equals hostelRoom.Created_By
										  where (hostelRoom.Is_Deleted == null || hostelRoom.Is_Deleted == false)
										  select new HostelList_ViewModel
										  {
											  Id = hostelRoom.Id,
											  Room_No = hostelRoom.Room_No,
											  Description = hostelRoom.Description,
											  Academic_Year = hostelRoom.Academic_Year,
											  No_Of_Persons = hostelRoom.No_Of_Persons,
											  Updated_On = hostelRoom.Updated_On == null ? null : hostelRoom.Updated_On

										  }).ToList();

					//var hostelRoomList = dbcontext.Hostel_Room.ToList();



				//	return View(hostelRoomObj);


				}

			
			else
			{
				hostelRoomObj = null;
					//return View(hostelRoomObj);
				}
			}


			
			return View(hostelRoomObj);

		}


		[HttpPost]
		public JsonResult HostelRoomList(Hostel_Room hostel_Room)
		{
			string sReturn_Text = string.Empty;
			try
			{
				hostel_Room.Created_By = 5;
				hostel_Room.Created_On = DateTime.Now;
				hostel_Room.Is_Active = true;
				long nYear = GetAcademicYear();

				using (var dbcontext = new SchoolERPDBContext())
				{
					if (dbcontext.Hostel_Room.Where(x => x.Room_No == hostel_Room.Room_No && (x.Is_Deleted == false || x.Is_Deleted == null) && x.Academic_Year == nYear).Count() == 0)
					{
						dbcontext.Hostel_Room.Add(hostel_Room);
						dbcontext.SaveChanges();
						sReturn_Text = "OK";
					}
					else
					{
						sReturn_Text = "Room No Already Exists";

					}
				}

				}
			catch (Exception ex)
			{
				sReturn_Text = ex.InnerException.Message.ToString();
			}
			return Json(sReturn_Text, JsonRequestBehavior.AllowGet);
		}


		public ActionResult Delete(long Id)
		{

			using (var dbcontext = new SchoolERPDBContext())
			{
				var hostelRoomToBeDeleted = dbcontext.Hostel_Room.Find(Id);
				hostelRoomToBeDeleted.Is_Deleted = true;
				hostelRoomToBeDeleted.Is_Active = false;
				hostelRoomToBeDeleted.Updated_By = 5;
				hostelRoomToBeDeleted.Updated_On = DateTime.Now;

				dbcontext.Entry(hostelRoomToBeDeleted).State = EntityState.Modified;
				dbcontext.SaveChanges();

			}
			return RedirectToAction("HostelRoomList");
			//return View();
		}
	}
}