using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Techsys_School_ERP.Model
{
	public class User_Role
	{
		[Key]
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int Role_Id { get; set; }

		public string Name { get; set; }

		public long Academic_Year { get; set; }

		public bool Is_Active { get; set; }

		public bool? Is_Deleted { get; set; }

		public DateTime Created_On { get; set; }

		public int Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public int? Updated_By { get; set; }
	}

	public enum Role_Type {

		SuperAdmin = 1,
		Admin,
		Teacher,
		Parent,
		Student

	};
}