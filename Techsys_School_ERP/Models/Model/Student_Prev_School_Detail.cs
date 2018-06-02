using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Student_Prev_School_Detail
	{

		public int Student_PrevSchool_Id { get; set; }

		public long Student_Id { get; set; }

		public int School_Id { get; set; }

		public string Other_School_Name { get; set; }

		public string Other_School_Address { get; set; }

		public long Academic_Year { get; set; }

		public long From_Year { get; set; }

		public long To_Year { get; set; }

		public string Leaving_Reason { get; set; }

		public bool Is_Active { get; set; }

		public bool? Is_Deleted { get; set; }

		public DateTime Created_On { get; set; }

		public int Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public int Updated_By { get; set; }

	}
}