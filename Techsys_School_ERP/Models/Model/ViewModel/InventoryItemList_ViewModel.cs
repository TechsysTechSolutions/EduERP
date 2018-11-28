using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model.ViewModel
{
	public class InventoryItemList_ViewModel
	{

		public int Id { get; set; }

		public string Name { get; set; }

		public int Inventory_id { get; set; }

		public DateTime? Purchase_Date { get; set; }

		public DateTime? Repair_Date { get; set; }

		public DateTime? Expiry_Date { get; set; }

		public decimal Amount { get; set; }

		public bool Is_Repaired { get; set; }

		public int Total_Number { get; set; }

		public bool Is_In_Good_Condition { get; set; }

		public string Purchase_Place { get; set; }

		public string Goods_Condition { get; set; }

		public string User_Id { get; set; }

		public long Academic_Year { get; set; }

		public bool Is_Active { get; set; }

		public bool? Is_Deleted { get; set; }

		public DateTime Created_On { get; set; }

		public int Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public int Updated_By { get; set; }
	}
}