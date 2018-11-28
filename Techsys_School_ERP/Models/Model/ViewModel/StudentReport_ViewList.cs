using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model.ViewModel
{
	public class StudentReport_ViewList
	{

		public Student Student { get; set; }

		public Student_Other_Details Student_Other_Details { get; set; }

		public List<Student_Prev_School_Details> Student_Prev_School_Details_List { get; set; }

	    public List<Student_Sibling_Detail> Student_Sibling_Detail_List { get; set; }

		public Attendance Student_Attendance { get; set; }

		public Mark Student_Mark { get; set; }

	}
}