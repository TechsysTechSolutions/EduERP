using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model.ViewModel
{
	public class SyllabusList_ViewModel
	{
		public int Id { get; set; }

		public int Subject_Id { get; set; }

		public string Subject_Name { get; set; }

		public int Class_Id { get; set; }

		public string Class_Name { get; set; }

		public int Term_Id { get; set; }

		public string Term_Name { get; set; }

		public int Section_Id { get; set; }

		public string Section_Name { get; set; }

		public string Syllabus_Text { get; set; }

		public DateTime Completion_Date { get; set; }

		[Display(Name = "ACADEMIC YEAR")]
		public long Academic_Year { get; set; }

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