using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model.ViewModel
{
	public class Student_PrevSchoolList_ViewModel
	{
		public int Id { get; set; }

		public  long Student_Id{ get; set; }

		public string Name { get; set; }

		public long From_Year { get; set; }

		public long To_Year { get; set; }

		public string Leaving_Reason { get; set; }

		public string Comments { get; set; }

		public long Academic_Year { get; set; }

		public bool Is_Active { get; set; }

		public bool? Is_Deleted { get; set; }

		public DateTime Created_On { get; set; }

		public int Created_By { get; set; }

		public string User_Id { get; set; }

		public DateTime? Updated_On { get; set; }

		public int Updated_By { get; set; }
	}
}