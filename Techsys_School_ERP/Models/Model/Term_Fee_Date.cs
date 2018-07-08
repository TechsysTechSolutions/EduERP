using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Term_Fee_Date
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int Term_Id {get; set;}

		public int academic_year { get; set; }

		public DateTime? Fee_Pay_CutOff_Date { get; set; }

	}
}