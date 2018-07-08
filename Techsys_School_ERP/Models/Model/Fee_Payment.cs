using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Fee_Payment
	{
		[Key]
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string Recipt_no { get; set; }

		public long Student_id { get; set; }

		public DateTime Payment_date { get; set; }

		public int Frequency { get; set; }

		public decimal Total_Amount { get; set; }

		public decimal Late_fees { get; set; }

		public DateTime? Next_due_date { get; set; }

		public string Collected_by { get; set; }

		public long Academic_Year { get; set; }

		public bool Is_Active { get; set; }

		public bool? Is_Deleted { get; set; }

		public DateTime Created_On { get; set; }

		public int Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public int? Updated_By { get; set; }

		public string File_Name { get; set; }
	}
}