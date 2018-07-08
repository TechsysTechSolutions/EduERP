using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model.ViewModel
{
	public class Staff_ViewModel
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		public string Name { get; set; }

		public string Employee_No { get; set; }

		public string Emergency_ContactNo { get; set; }

		public long Academic_Year { get; set; }

		public bool Is_Active { get; set; }

		public bool Is_Deleted { get; set; }

		public DateTime? Created_On { get; set; }

		public int Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public int? Updated_By { get; set; }

		

	}
}