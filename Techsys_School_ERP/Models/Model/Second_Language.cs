using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Second_Language
	{
		public int Id { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; }
	}
}