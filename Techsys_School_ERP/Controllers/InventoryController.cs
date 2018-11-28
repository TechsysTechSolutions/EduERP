using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Techsys_School_ERP.DBAccess;
using Techsys_School_ERP.Model;
using Techsys_School_ERP.Model.ViewModel;

namespace Techsys_School_ERP.Controllers
{
	
	public class InventoryController : CommonController
	{
        // GET: Inventory
        public ActionResult AddInventoryDetail()
        {
			List<InventoryItemList_ViewModel> inventoryItemListViewModel = new List<InventoryItemList_ViewModel>();
			long nYear = GetAcademicYear();
			using (var dbcontext = new SchoolERPDBContext())
			{
				inventoryItemListViewModel = (from usr in dbcontext.Users
										  join ivntry in dbcontext.Inventory on usr.Id equals ivntry.Created_By
										  join ivntryItem in dbcontext.Inventory_Item_Detail on ivntry.Id equals ivntryItem.Inventory_id
										  where (ivntryItem.Is_Deleted == null || ivntryItem.Is_Deleted == false )&& ivntryItem.Academic_Year == nYear
										  select new InventoryItemList_ViewModel
										  {
											  Id = ivntry.Id,
											  Name = ivntry.Name,
											  Academic_Year = ivntryItem.Academic_Year,
											  Total_Number = ivntryItem.Total_Number ,
											  Amount = ivntryItem.Amount,
											  Purchase_Date = ivntryItem.Purchase_Date,
											  Expiry_Date = ivntryItem.Expiry_Date,
											  Is_Repaired = ivntryItem.Is_Repaired,
											  Is_In_Good_Condition = ivntryItem.Is_In_Good_Condition,
											  Repair_Date = ivntryItem.Repair_Date,
											 // User_Id = usr.User_Id,
											 // Is_Deleted = ivntry.Is_Deleted,
											  Updated_On = ivntry.Updated_On,
											 // Created_By = ivntry.Created_By

										  }).ToList();



			}

			return View(inventoryItemListViewModel);
			//return View();
        }

		[HttpPost]
		public ActionResult AddInventoryDetail(Inventory_Item_Detail Inventory_Item_Detail)
		{
		

			try
			{
				Inventory_Item_Detail newInventory_Item_Detail = new Inventory_Item_Detail();
				int nUser_Id;
				long nYear = GetAcademicYear();
				using (var dbcontext = new SchoolERPDBContext())
				{
					//nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
					nUser_Id = 5;
				}
				using (var dbcontext = new SchoolERPDBContext())
				{
					//newInventory_Item_Detail.Inventory_id = Inventory_Item_Detail.Inventory_id;
					//newInventory_Item_Detail.Academic_Year = GetAcademicYear();
					//newInventory_Item_Detail.Is_Active = true;
					//newInventory_Item_Detail.Created_By = nUser_Id;
					//newInventory_Item_Detail.Created_On = DateTime.Now;
					Inventory_Item_Detail.Created_By = 5;
					Inventory_Item_Detail.Created_On = DateTime.Now;
					Inventory_Item_Detail.Academic_Year = GetAcademicYear();
					if (dbcontext.Inventory_Item_Detail.Where(a => a.Inventory_id == Inventory_Item_Detail.Inventory_id && (a.Is_Deleted == false  || a.Is_Deleted == null) && a.Academic_Year == nYear).Count() == 0)
					{
						dbcontext.Inventory_Item_Detail.Add(Inventory_Item_Detail);
						dbcontext.SaveChanges();
						return Json("OK", JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json("Already Exists", JsonRequestBehavior.AllowGet);
					}
				}

			}
			catch (Exception e)
			{
				return Json("Failure", JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult InventoryDetailList()
		{
			return View();
		}

		public ActionResult InventoryItemsList()
		{
			List<List_ViewModel> inventoryListViewModel = new List<List_ViewModel>();
			using (var dbcontext = new SchoolERPDBContext())
			{
				inventoryListViewModel = (from usr in dbcontext.Users
									 join ivntry in dbcontext.Inventory on usr.Id equals  ivntry.Created_By
									 where (ivntry.Is_Deleted == null || ivntry.Is_Deleted == false)
									 select new List_ViewModel
									 {
										 Id = ivntry.Id,
										 Name = ivntry.Name,
										 Academic_Year = usr.Academic_Year,
										 User_Id = usr.User_Id,
										 Is_Deleted = ivntry.Is_Deleted,
										 Created_On = ivntry.Created_On,
										 Created_By = ivntry.Created_By

									 }).ToList();



			}
			//return View(examListViewModel);
			return View(inventoryListViewModel);
		}

		public ActionResult GetInventoryItemList(string q)
		{
			return Json( SearchAndGetInventoryList(q) , JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult CreateInventory(string Inventory_Item_Name)
		{
			try
			{
				Inventory newInventory = new Inventory();
				int nUser_Id;
				using (var dbcontext = new SchoolERPDBContext())
				{
					//nUser_Id = dbcontext.Users.Where(x => x.User_Id == User.Identity.Name).ToList()[0].Id; ;
					nUser_Id = 5;
				}
				using (var dbcontext = new SchoolERPDBContext())
				{
					newInventory.Name = Inventory_Item_Name;
					newInventory.Academic_Year = GetAcademicYear();
					newInventory.Is_Active = true;
					newInventory.Created_By = nUser_Id;
					newInventory.Created_On = DateTime.Now;
					if (dbcontext.Inventory.Where(a => a.Name == Inventory_Item_Name.Trim().ToString() && a.Is_Deleted == false).Count() == 0)
					{
						dbcontext.Inventory.Add(newInventory);
						dbcontext.SaveChanges();
						return Json("OK", JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json("Inventory Item Already Exists.", JsonRequestBehavior.AllowGet);
					}
				}

			}
			catch (Exception e)
			{
				return Json("Failure", JsonRequestBehavior.AllowGet);
			}


		}

	}
}