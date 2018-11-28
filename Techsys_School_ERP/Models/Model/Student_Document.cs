using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Student_Document
	{
		[Key]
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public long Student_Id { get; set; }

		[Display(Name = "DOCUMENT1")]
		public byte[] document1 { get; set; }

		[Display(Name = "DOCUMENT2")]
		public byte[] document2 { get; set; }

		[Display(Name = "DOCUMENT3")]
		public byte[] document3 { get; set; }

		[Display(Name = "DOCUMENT4")]
		public byte[] document4 { get; set; }

		[Display(Name = "DOCUMENT5")]
		public byte[] document5 { get; set; }

		[Display(Name = "DOCUMENT6")]
		public byte[] document6 { get; set; }


	}
}