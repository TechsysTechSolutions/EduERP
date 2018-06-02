using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Mark
	{
		[Key]
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public long Student_Id { get; set; }

		public int Class_Id { get; set; }

		public int Section_Id { get; set; }

		public int Exam_Id { get; set; }

		public int Subject_Id1 { get; set; }

		public int Subject_Id2 { get; set; }

		public int Subject_Id3 { get; set; }

		public int Subject_Id4 { get; set; }

		public int Subject_Id5 { get; set; }

		public int Subject_Id6 { get; set; }

		public int Subject_Id7 { get; set; }

		public int Subject_Id8 { get; set; }

		public int Subject_Id9 { get; set; }

		public int Subject_Id10 { get; set; }

		public int Mark1 { get; set; }

		public int Mark2 { get; set; }

		public int Mark3 { get; set; }

		public int Mark4 { get; set; }

		public int Mark5 { get; set; }

		public int Mark6 { get; set; }

		public int Mark7 { get; set; }

		public int Mark8 { get; set; }

		public int Mark9 { get; set; }

		public int Mark10 { get; set; }

		public long Academic_Year { get; set; }

		public bool Is_Active { get; set; }

		public bool? Is_Deleted { get; set; }

		public DateTime Created_On { get; set; }

		public int Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public int Updated_By { get; set; }

	}
}