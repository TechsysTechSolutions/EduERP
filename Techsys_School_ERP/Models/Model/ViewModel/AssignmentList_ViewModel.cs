using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Models.Model.ViewModel
{
	public class AssignmentList_ViewModel
	{		
		public long Id { get; set; }

		public string Class { get; set; }

		public string Section { get; set; }

		public string Subject { get; set; }

		public DateTime Assigned_Date { get; set; }

		public DateTime Completion_Date { get; set; }

		public string Assignment_Title { get; set; }
		
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