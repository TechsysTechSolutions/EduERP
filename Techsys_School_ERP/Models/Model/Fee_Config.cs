using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Fee_Configuration
	{
		[Key]
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int Class_Id { get; set; }

		public int Fee_Id { get; set; }

		public long Academic_Year { get; set; }

		public decimal Amount { get; set; }

		public decimal Total { get; set; }

		public decimal? Total_Excluding_HostelFees { get; set; }

		public decimal? Total_Excluding_Bus_Fees { get; set; }

		public int Frequency { get; set; }

		public bool Is_Active { get; set; }

		public bool? Is_Deleted { get; set; }

		public DateTime Created_On { get; set; }

		public int Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public int? Updated_By { get; set; }

	}
}