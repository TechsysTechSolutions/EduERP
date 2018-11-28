using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Techsys_School_ERP.DBAccess;
using Techsys_School_ERP.Model.ViewModel;
using Techsys_School_ERP.Model;
using System.Data.Entity;
using System.Reflection;

namespace Techsys_School_ERP.Controllers
{
    public class TransportController : CommonController
    {
        // GET: Transport
        public ActionResult Index()
        {
            return View();
        }


		public ActionResult TransportDestinationList()
		{

			long nYear = GetAcademicYear();

			List<TransportDestinationList_ViewModel> transportList = new List<TransportDestinationList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				transportList = (from usr in dbcontext.Users
								 join transportDestination in dbcontext.TransportDestination on usr.Id equals transportDestination.Created_By
								 where (transportDestination.Is_Deleted == null || transportDestination.Is_Deleted == false)
								 select new TransportDestinationList_ViewModel
								 {
									 Id = transportDestination.Id,
									 Name = transportDestination.Name,
									 Amount = transportDestination.Amount,
									 Academic_Year = transportDestination.Academic_Year,
									 Updated_On = transportDestination.Updated_On == null ? null : transportDestination.Updated_On

								 }).ToList();

			}
			return View(transportList);
			//return View();
		}

		[HttpPost]
		public ActionResult AddTransportDestination(TransportDestination transport_Destination)
		{
			string sReturnText = string.Empty;
			long nYear = GetAcademicYear();
			try
			{
				using (var dbcontext = new SchoolERPDBContext())
				{
					if (dbcontext.TransportDestination.Where(x => x.Name == transport_Destination.Name && x.Academic_Year == transport_Destination.Academic_Year && (x.Is_Deleted == null || x.Is_Deleted == false)).Count() == 0)
					{
						transport_Destination.Created_By = 5;
						transport_Destination.Created_On = DateTime.Now;
						transport_Destination.Is_Active = true;
						//transport_Destination.Is_Deleted = false;
						transport_Destination.Academic_Year = nYear;

						dbcontext.TransportDestination.Add(transport_Destination);
						dbcontext.SaveChanges();
						sReturnText = "OK";

					}
					else
					{
						sReturnText = "Transport Destination Already Exists";
					}
				}
			}
			catch (Exception ex)
			{
				sReturnText = ex.InnerException.Message.ToString();

			}
			return Json(sReturnText, JsonRequestBehavior.AllowGet);
		}


		public ActionResult AddTransport()
		{
			return View();
		}

		[HttpPost]
		public ActionResult SaveTransportRoute( Transport_Destination_Detail Transport_Destination_Detail ,string[] Route_Id)
		{
			string sReturn_text = string.Empty;
			using (var dbcontext = new SchoolERPDBContext())
			{
				//using (var transaction = dbcontext.Database.BeginTransaction())
				//{
					try
					{
					if (dbcontext.Transport_Destination_Detail.Where(x => x.Name == Transport_Destination_Detail.Name && x.Academic_Year == Transport_Destination_Detail.Academic_Year && (x.Is_Deleted == null || x.Is_Deleted == false)).Count() == 0)
					{
						for (int nStopCount = 1; nStopCount <= Route_Id.Count(); nStopCount++)
						{
							int stop_Value = Convert.ToInt16(Route_Id[nStopCount - 1]);
							PropertyInfo propertyInfo = Transport_Destination_Detail.GetType().GetProperty("Stop_" + nStopCount);
							// make sure object has the property we are after
							if (propertyInfo != null)
							{
								propertyInfo.SetValue(Transport_Destination_Detail, stop_Value, null);
							}
						}
						Transport_Destination_Detail.Created_By = 5;
						Transport_Destination_Detail.Created_On = DateTime.Now;
						dbcontext.Transport_Destination_Detail.Add(Transport_Destination_Detail);
						dbcontext.SaveChanges();
						sReturn_text = "OK";

					}
					else
					{
						sReturn_text = "Exists";
					}
					}
					catch (Exception ex)
					{
					sReturn_text = ex.InnerException.Message.ToString();
					}
				//}
			}
			return Json(sReturn_text, JsonRequestBehavior.AllowGet);
		}


		public JsonResult GetRouteNameList(string q)
		{
			return Json(SearchAndGetRouteNameList(q), JsonRequestBehavior.AllowGet);

		

		}


		public ActionResult TransportList()
		{
			long nYear = GetAcademicYear();

			List<TransportList_ViewModel> transportList = new List<TransportList_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				transportList = (from usr in dbcontext.Users
									 join transport in dbcontext.Transport on usr.Id equals transport.Created_By
									 where (transport.Is_Deleted == null || transport.Is_Deleted == false)
									 select new TransportList_ViewModel
									 {
										 Id = transport.Id,
										 Name = transport.Name,
										 Description = transport.Description,
										 Academic_Year = transport.Academic_Year,
										 Vehicle_No = transport.Vehicle_No,
										 No_Of_Seats = transport.No_Of_Seats,
										 Updated_On = transport.Updated_On == null ? null : transport.Updated_On

									 }).ToList();


				
			}



			return View(transportList);

		}


		[HttpPost]
		public JsonResult TransportList(Transport transport)
		{
			string sReturn_Text = string.Empty;
			try
			{
				transport.Created_By = 5;
				transport.Created_On = DateTime.Now;
				transport.Is_Active = true;
				long nYear = GetAcademicYear();

				using (var dbcontext = new SchoolERPDBContext())
				{
					if (dbcontext.Transport.Where(x => x.Name == transport.Name && (x.Is_Deleted == false || x.Is_Deleted == null) && x.Academic_Year == nYear).Count() == 0)
					{
						dbcontext.Transport.Add(transport);
						dbcontext.SaveChanges();
						sReturn_Text = "OK";
					}
					else
					{
						sReturn_Text = "Vehicle No Already Exists";

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
				var transportToBeDeleted = dbcontext.Transport.Find(Id);
				transportToBeDeleted.Is_Deleted = true;
				transportToBeDeleted.Is_Active = false;
				transportToBeDeleted.Updated_By = 5;
				transportToBeDeleted.Updated_On = DateTime.Now;

				dbcontext.Entry(transportToBeDeleted).State = EntityState.Modified;
				dbcontext.SaveChanges();

			}
			return RedirectToAction("TransportList");
			//return View();
		}
	}
}