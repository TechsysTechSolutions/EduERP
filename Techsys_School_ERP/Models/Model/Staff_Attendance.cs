using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Staff_Attendance
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int Staff_Id { get; set; }

		public DateTime From_Date { get; set; }

		public DateTime To_Date { get; set; }

		public DateTime Leave_Date { get; set; }

		public string Reason { get; set; }

		[Display(Name = "ACADEMIC YEAR")]
		public long? Academic_Year { get; set; }

		public bool? Is_Active { get; set; }

		public bool? Is_Deleted { get; set; }

		public DateTime? Created_On { get; set; }

		[Display(Name = "Created By")]
		public int Created_By { get; set; }

		[Display(Name = "Updated Date")]
		public DateTime? Updated_On { get; set; }	
	}
}