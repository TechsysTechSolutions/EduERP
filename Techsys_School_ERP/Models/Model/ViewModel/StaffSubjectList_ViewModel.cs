using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model.ViewModel
{
	public class StaffSubjectList_ViewModel
	{

		public int Id { get; set; }

		public int Staff_Id { get; set; }

		public int Class_Id { get; set; }

		public int Section_Id { get; set; }

		public int Subject_Id { get; set; }

		public string Staff_Name { get; set; }

		public string Class_Name { get; set; }

		public string Section_Name { get; set; }

		public string Subject_Name { get; set; }

		public long Academic_Year { get; set; }

		public bool Is_Active { get; set; }

		public bool? Is_Deleted { get; set; }

		public DateTime Created_On { get; set; }

		public int Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public int Updated_By { get; set; }
	}
}