using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Staff_Salary_Slip
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		public int Staff_Id { get; set; }

		public int Month_Id { get; set; }

		public int Total_Leave_Days { get; set; }

		public int Applied_Leave_days { get; set; }

		public string Leave_Dates { get; set; }

		public DateTime Generated_on { get; set; }

		public decimal Total_Amount { get; set; }

		public decimal Deductions { get; set; }

		public long Academic_Year { get; set; }

		public bool Is_Active { get; set; }

		public bool Is_Deleted { get; set; }

		public DateTime? Created_On { get; set; }

		public int Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public int? Updated_By { get; set; }
	}
}