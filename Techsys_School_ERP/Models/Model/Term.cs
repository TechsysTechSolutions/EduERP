using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Term
	{
		[Key]
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime From_Date { get; set; }

		public DateTime To_Date { get; set; }
		
		public long Academic_Year { get; set; }
	}
}