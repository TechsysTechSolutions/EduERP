using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model.ViewModel
{
	public class Exam_TimeTableList_ViewModel
	{
		public int Id { get; set; }

		public int Exam_Id { get; set; }

		public string Exam_Name { get; set; }

		public int Subject_Id { get; set; }

		public string Subject_Name { get; set; }

		public int Class_Id { get; set; }

		public string Class_Name { get; set; }

		public int Section_Id { get; set; }

		public string Section_Name { get; set; }

		public long Academic_Year { get; set; }

		public string Exam_Date { get; set; }

		public int Exam_Session { get; set; }

		public DateTime Class_Exam_Date { get; set; }
	}
}