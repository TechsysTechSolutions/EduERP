using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Attendance
	{

		[Key]
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		public long Student_Id { get; set; }

		public int Class_Id { get; set; }

		public int Term_Id { get; set; }

		public int Section_Id {get ;set ;}

		public DateTime Leave_Date { get; set; }

		public string Leave_Reason { get; set; }

		public int Academic_Year { get; set; }

		public bool Is_Active { get; set; }

		public bool? Is_Deleted { get; set; }

		public DateTime Created_On { get; set; }

		public int Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public int? Updated_By { get; set; }


	}
}