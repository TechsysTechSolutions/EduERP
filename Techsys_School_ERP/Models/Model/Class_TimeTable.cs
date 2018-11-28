using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Class_TimeTable
	{

		[Key]
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int Class_Id { get; set; }

		public int Section_Id { get; set; }

		//public string Week_Day { get; set; }

		//public string Class_Name { get; set; }

		//public string Section_Name { get; set; }

		public int Week { get; set; }

		public int Subject_Id_Period_1 { get; set; }

	//	public int Staff_Id_Period1 { get; set; }

		public int Subject_Id_Period_2 { get; set; }

		//public int Staff_Id_Period2 { get; set; }

		public int Subject_Id_Period_3 { get; set; }

		//public int Staff_Id_Period3 { get; set; }

		public int Subject_Id_Period_4 { get; set; }

		//public int Staff_Id_Period4 { get; set; }

		public int Subject_Id_Period_5 { get; set; }

		//public int Staff_Id_Period5 { get; set; }

		public int Subject_Id_Period_6 { get; set; }

	//	public int Staff_Id_Period6 { get; set; }

		public int Subject_Id_Period_7 { get; set; }

		//public int Staff_Id_Period7 { get; set; }

		public int Subject_Id_Period_8 { get; set; }

		//public int Staff_Id_Period8 { get; set; }

		[Display(Name = "ACADEMIC YEAR")]
		public long Academic_Year { get; set; }

		[Display(Name = "CREATED DATE")]
		public DateTime Created_On { get; set; }

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