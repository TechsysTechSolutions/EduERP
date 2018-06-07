using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model.ViewModel
{
	public class Student_SiblingList_ViewModel
	{

		public long Sibling_Detail_Id { get; set; }

		public long Student_Id { get; set; }

		public long Sibling_Student_Id { get; set; }

		public long Academic_Year { get; set; }

		public string Student_Name { get; set; }

		public string Roll_No { get; set; }

		public string Class { get; set; }

		public string Section { get; set; }

		public bool Is_Active { get; set; }

		public bool? Is_Deleted { get; set; }

		public DateTime Created_On { get; set; }

		public int Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public int Updated_By { get; set; }
	}
}