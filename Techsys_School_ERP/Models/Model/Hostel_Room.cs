﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Hostel_Room
	{


		[Key]
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string Room_No  { get; set; }

		public string Description { get; set; }

		public int No_Of_Persons { get; set; }

	//	public int No_Of_Seats { get; set; }

		public long Academic_Year { get; set; }

		public bool Is_Active { get; set; }

		public bool? Is_Deleted { get; set; }

		public DateTime Created_On { get; set; }

		public int Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public int? Updated_By { get; set; }
		


	}
}