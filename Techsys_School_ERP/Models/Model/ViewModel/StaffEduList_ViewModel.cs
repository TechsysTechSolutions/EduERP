using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model.ViewModel
{
	public class StaffEduList_ViewModel
	{
		public long Id { get; set; }

		public string Institution { get; set; }

		public string Qualification { get; set; }

		public long? Year_Of_Passing { get; set; }

		

		public string Specialization { get; set; }

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