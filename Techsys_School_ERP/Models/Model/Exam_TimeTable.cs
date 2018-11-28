﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Exam_TimeTable
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int Exam_Id { get; set; }

		public int Subject_Id { get; set; }

		public int Class_Id { get; set; }

		public int Section_Id { get; set; }

		public DateTime Exam_Date { get; set; }

		public int Exam_Session { get; set; }

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