using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Student_Other_Details
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long StudentDetail_Id { get; set; }

		public long Student_Id { get; set; }

		[Required(ErrorMessage = "Identification Mark1 is Required.")]
		[Display(Name = "IDENTIFICATION MARK1")]
		public string Identification_Mark1 { get; set; }

		[Required(ErrorMessage = "Identification Mark2 is Required.")]
		[Display(Name = "IDENTIFICATION MARK2")]
		public string Identification_Mark2 { get; set; }

		[Display(Name = "IS ALLEGERIC?")]
		public bool Is_Allergic { get; set; }

		[Display(Name = "ALLERGY DETAILS")]
		public string Allergy_Details { get; set; }

		[Display(Name = "FATHER'S OCCUPATION")]
		public int Father_Occupation_Id { get; set; }

		[Display(Name = "ANUUAL INCOME")]
		public decimal Father_Annual_Income { get; set; }

		[Display(Name = " OFFICE ADDRESS")]
		public string Father_Office_Address { get; set; }

		[Display(Name = "OFFICE ADDRESS")]
		public string Mother_Office_Address { get; set; }

		[Display(Name = "MOTHER'S OCCUPATION")]
		public int Mother_Occupation_Id { get; set; }

		[Display(Name = " ANUUAL INCOME")]
		public decimal Mother_Annual_Income { get; set; }

		[Display(Name = "CATEGORY")]
		public int Category_Id { get; set; }

		[Display(Name = "CASTE")]
		public string Caste { get; set; }

		[Display(Name = "RELIGION")]
		public string Religion { get; set; }

		[Display(Name = "MOTHER TONGUE")]
		public string Mother_tongue { get; set; }

		[Display(Name = "SECOND LANGUAGE")]
		public int Second_Language_Opted_Id { get; set; }

		[Display(Name = "BIRTH CERTIFICATE")]
		public byte[] Birth_Certificate { get; set; }

		[Display(Name = "MEDICAL HISTORY DETAILS(IF ANY)")]
		public string Medical_History_Details { get; set; }

		[Display(Name = "COMPANY NAME")]
		public string Father_Company_Name { get; set; }


		[Display(Name = "COMPANY NAME")]
		public string Mother_Company_Name { get; set; }

		[Display(Name = "DESIGNATION")]
		public string Father_Designation { get; set; }

		[Display(Name = "DESIGNATION")]
		public string Mother_Designation { get; set; }

		[Display(Name = "ID CARD")]
		public byte[] ID_Card { get; set; }

		[Display(Name = "UPLOAD DOCUMENT1")]
		public byte[] Upload_Document1 { get; set; }

		[Display(Name = "UPLOAD DOCUMENT2")]
		public byte[] Upload_Document2 { get; set; }

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