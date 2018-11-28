using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Transport_Destination_Detail
	{
		[Key]
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int No_Of_Seats { get; set; }

		public string Vehicle_No { get; set; }

		public int? Stop_1 { get; set; }

		public int? Stop_2 { get; set; }

		public int? Stop_3 { get; set; }

		public int? Stop_4 { get; set; }

		public int? Stop_5 { get; set; }

		public int? Stop_6 { get; set; }

		public int? Stop_7 { get; set; }

		public int? Stop_8 { get; set; }

		public int? Stop_9 { get; set; }

		public int? Stop_10 { get; set; }

		public int? Stop_11 { get; set; }

		public int? Stop_12 { get; set; }

		public int? Stop_13 { get; set; }

		public int? Stop_14 { get; set; }

		public int? Stop_15 { get; set; }

		public int? Stop_16 { get; set; }

		public int? Stop_17 { get; set; }

		public int? Stop_18 { get; set; }

		public int? Stop_19 { get; set; }

		public int? Stop_20 { get; set; }

		public long Academic_Year { get; set; }

		public bool Is_Active { get; set; }

		public bool? Is_Deleted { get; set; }

		public DateTime Created_On { get; set; }

		public int Created_By { get; set; }

		public DateTime? Updated_On { get; set; }

		public int Updated_By { get; set; }

	

	}
}