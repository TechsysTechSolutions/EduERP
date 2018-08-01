using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model.ViewModel
{
	public class StaffSalary_ViewModel
	{
		public int Id { get; set; }

		public int Staff_Id { get; set; }

		public string Staff_Name { get; set; }

		public string Staff_Type { get; set; }
		
		public string Employee_No { get; set; }

		//public int  Staff_Type_Id { get; set; }

		public decimal? Basic { get; set; }

		public decimal? DA { get; set; }

		public decimal? Medical { get; set; }

		public decimal? Conveyance { get; set; }

		public decimal? HRA { get; set; }

		public decimal? LTA { get; set; }

		public decimal? Other { get; set; }

		[Display(Name = "PROVIDENT FUND")]
		public decimal? Provident_Fund { get; set; }

		public decimal? ESIC { get; set; }

		[Display(Name = "PROFESSIONAL TAX")]
		public decimal? Professional_Tax { get; set; }

		[Display(Name = "ACADEMIC YEAR")]
		public long Academic_Year { get; set; }

		public bool Is_Active { get; set; }

		public bool Is_Deleted { get; set; }

		public DateTime? Created_On { get; set; }

		public string Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public string Updated_By { get; set; }

		[Display(Name = "GROSS SALARY")]
		public decimal? Gross_Salary { get; set; }

		[Display(Name = "NET SALARY")]
		public decimal? Net_Salary { get; set; }

		[Display(Name = "OTHER DEDUCTIONS")]
		public decimal? Other_Deductions { get; set; }

		[Display(Name = "PF ACCOUNT NO")]
		public string PF_Account_No { get; set; }

		[Display(Name = "PAN NO")]
		public string PAN_No { get; set; }

		[Display(Name = "BANK NAME")]
		public string Bank_Name { get; set; }

		[Display(Name = "BRANCH NAME")]
		public string Branch_Name { get; set; }

		[Display(Name = "BANK ADDRESS LINE1")]
		public string Bank_AddressLine1 { get; set; }


		[Display(Name = "BANK ADDRESS LINE2")]
		public string Bank_AddressLine2 { get; set; }

		[Display(Name = "CITY")]
		public string City { get; set; }

		[Display(Name = "BANK ACCOUNT NO")]
		public string Bank_Account_No { get; set; }

		[Display(Name = "NAME ON ACCOUNT")]
		public string Name_on_Acount { get; set; }

	}
}