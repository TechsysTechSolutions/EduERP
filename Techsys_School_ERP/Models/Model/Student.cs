using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Techsys_School_ERP.Model
{
	public class Student
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Student_Id { get; set; }

		[StringLength(20)]
		[Display(Name = "ROLL NO")]
		public string Roll_No { get; set; }

		[StringLength(50)]
		[Required(ErrorMessage = "First Name is Required.")]
		[Display(Name = "FIRST NAME")]
		public string First_Name { get; set; }

		[StringLength(50)]
		[Display(Name = "MIDDLE NAME")]
		public string Middle_Name { get; set; }

		[StringLength(50)]
		[Required(ErrorMessage = "Last Name is Required.")]
		[Display(Name = "LAST NAME")]
		public string Last_Name { get; set; }

		[Display(Name = "GENDER")]

		public int Gender_Id { get; set; }


		[Display(Name = "DOB")]
		[Required(ErrorMessage = "Date Of birth is Required.")]
		//[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? DOB { get; set; }

		[Display(Name = "ENROLLMENT DATE")]
		[Required(ErrorMessage = "Enrollment Date is Required.")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? Enrollment_Date { get; set; }

		[StringLength(50)]
		[Display(Name = "FATHER NAME")]
		public string Father_Name { get; set; }

		[StringLength(50)]
		[Display(Name = "MOTHER NAME")]
		public string Mother_Name { get; set; }


		[Display(Name = "BLOOD GROUP")]
		public int Blood_Group_Id { get; set; }

		[StringLength(500)]
		[Display(Name = "ADDRESS LINE1")]
		[Required(ErrorMessage = "Address Line1 is Required.")]
		public string Address_Line1 { get; set; }

		[StringLength(500)]
		[Display(Name = "ADDRESS LINE2")]
		[Required(ErrorMessage = "Address Line2 is Required.")]
		public string Address_Line2 { get; set; }

		[Display(Name = "CITY")]
		public int City_Id { get; set; }

		[Display(Name = "STATE")]
		public int State_Id { get; set; }


		[Display(Name = "COUNTRY")]
		public int Country_Id { get; set; }

		[StringLength(10)]
		[Display(Name = "PHONE1")]
		[Required(ErrorMessage = "Phone1 is Required.")]
		public string Phone_No1 { get; set; }

		[StringLength(10)]
		[Display(Name = "PHONE NO2")]
		public string Phone_No2 { get; set; }

		[StringLength(10)]
		[Display(Name = "LANDLINE")]
		public string LandLine { get; set; }

		[StringLength(30)]
		[Display(Name = "EMAIL")]
		[Required(ErrorMessage = "Email is Required.")]
		public string Email_Id { get; set; }

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

		[StringLength(10)]
		[Display(Name = "PIN CODE")]
		public string Pincode { get; set; }

		public byte[] Photo { get; set; }

		[Display(Name = "AADHAR NO")]
		[Required(ErrorMessage = "Aadhar No is Required.")]
		public long? Aadhar_No { get; set; }

		[Display(Name = "CLASS")]
		public int Class_Id { get; set; }

		[Display(Name = "SECTION")]
		public int Section_Id { get; set; }


		[Display(Name = "IS HOSTEL STUDENT?")]
		public bool? Is_HostelStudent { get; set; }

		public bool? Is_FeesDueRemaining { get; set; }

		public decimal? Fees_Due_Amount { get; set; }


		//[ForeignKey("Section_Id")]
		//public virtual Section FSection { get; set; }

		//[ForeignKey("Class_Id")]
		//public virtual Class FClass { get; set; }


		//[ForeignKey("Gender_Id")]
		//public virtual Gender Gender { get; set; }

		//[ForeignKey("Blood_Group_Id")]
		//public virtual Blood_Group BloodGroup { get; set; }

	}
}