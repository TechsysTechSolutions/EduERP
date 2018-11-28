using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Inventory_Item_Detail
	{

		[Key]
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		//public string Name { get; set; }

		public int Inventory_id { get; set; }

		public DateTime? Purchase_Date { get; set; }

		public DateTime? Repair_Date { get; set; }

		public DateTime? Expiry_Date { get; set; }

		public decimal Amount { get; set; }

		public bool Is_Repaired { get; set; }

		public int Total_Number { get; set; }

		public bool Is_In_Good_Condition { get; set; }

		[Display(Name = "ACADEMIC YEAR")]
		public long Academic_Year { get; set; }

		[Display(Name = "CREATED DATE")]
		public DateTime Created_On { get; set; }

		[Display(Name = "CREATED BY")]
		public int Created_By { get; set; }

		[Display(Name = "UPDATED DATE")]
		public DateTime? Updated_On { get; set; }

		[Display(Name = "UPDATED BY")]
		public int? Updated_By { get; set; }

		public bool Is_Active { get; set; }

		public bool Is_Deleted { get; set; }

		public string Purchase_Place { get; set; }

		public string Goods_Condition { get; set; }




	}
}