using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model.ViewModel
{
	public class StaffSalarySlip_ViewModel
	{
		public int Id { get; set; }

		public long academic_year { get; set; }

		public int staff_id { get; set; }

		public string Name { get; set; }

		public int salary_month { get; set; }

		public string salary_Month_name { get; set; }

		public DateTime From_Date { get; set; }

		public DateTime To_Date { get; set; }

		public int no_of_leaves { get; set; }

		[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
		public decimal? salary_deduction { get; set; }

		public int leaves_remaining { get; set; }

		[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
		public decimal? gross_salary { get; set; }

		[Display(Name = "CREATED DATE")]
		public DateTime? Created_On { get; set; }

		[Display(Name = "CREATED BY")]
		public int Created_By { get; set; }

		[Display(Name = "UPDATED DATE")]
		public DateTime? Updated_On { get; set; }

		[Display(Name = "UPDATED BY")]
		public int? Updated_By { get; set; }

		public bool Is_Active { get; set; }

		public bool Is_Deleted { get; set; }
	}
}