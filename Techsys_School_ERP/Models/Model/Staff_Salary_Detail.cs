using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace Techsys_School_ERP.Model
{
	public partial class Staff_Salary_Detail
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Staff_Salary_Id { get; set; }

		public int Staff_Id { get; set; }

		//public int  Staff_Type_Id { get; set; }

		public decimal Basic { get; set; }

		public decimal? DA { get; set; }

		public decimal? Medical { get; set; }

		public decimal? Conveyance { get; set; }

		public decimal? HRA { get; set; }

		public decimal? LTA { get; set; }

		public decimal? Other { get; set; }

		public decimal? Provident_Fund { get; set; }

		public decimal? ESIC { get; set; }

		public decimal? Professional_Tax { get; set; }

		public long Academic_Year { get; set; }

		public bool Is_Active { get; set; }

		public bool Is_Deleted { get; set; }

		public DateTime? Created_On { get; set; }

		public string Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public string Updated_By { get; set; }

		public decimal Gross_Salary { get; set; }

		public decimal Net_Salary { get; set; }

		public decimal Other_Deductions { get; set; }

		
	}
}
