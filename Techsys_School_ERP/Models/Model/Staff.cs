
namespace Techsys_School_ERP.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

	[Table("Staff")]
	public partial class Staff
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Staff_Id { get; set; }

		public int? Staff_Type_Id { get; set; }


		[StringLength(50)]
		[Required(ErrorMessage = "First Name is Required.")]
		[Display(Name = "FIRST NAME")]
		public string First_Name { get; set; }

		[StringLength(50)]
		[Display(Name = "EMPLOYEE NO")]
		public string Employee_Id { get; set; }

		[StringLength(50)]
		[Display(Name = "MIDDLE NAME")]
		public string Middle_Name { get; set; }

		[StringLength(50)]
		[Required(ErrorMessage = "Last Name is Required.")]
		[Display(Name = "LAST NAME")]
		public string Last_Name { get; set; }

		[Display(Name = "GENDER")]
		public int? Gender_Id { get; set; }

		[Display(Name = "DOB")]
		[Required(ErrorMessage = "Date Of birth is Required.")]
		public DateTime? DOB { get; set; }

		[Display(Name = "DATE OF JOINING")]
		[Required(ErrorMessage = "Date Of Joining is Required.")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? Date_Of_Joining { get; set; }

		[StringLength(50)]
		[Display(Name = "FATHER NAME")]
		public string Father_Name { get; set; }

		[StringLength(10)]
		[Display(Name = "MOBILE NO")]
		public string Mobile_No { get; set; }

		[StringLength(10)]
		[Display(Name = "ALT MOBILE NO")]
		public string Alt_Mobile_No { get; set; }

		[StringLength(30)]
		[Display(Name = "EMAIL")]
		[Required(ErrorMessage = "Email is Required.")]
		public string Email_Id { get; set; }

		[Display(Name = "BLOOD GROUP")]
		public int? Blood_Group_Id { get; set; }

		[StringLength(500)]
		[Display(Name = "ADDRESS LINE1")]
		[Required(ErrorMessage = "Address Line1 is Required.")]
		public string Address_Line1 { get; set; }

		[StringLength(500)]
		[Display(Name = "ADDRESS LINE2")]
		[Required(ErrorMessage = "Address Line2 is Required.")]
		public string Address_Line2 { get; set; }

		[Display(Name = "CITY")]
		public int? City_Id { get; set; }

		[Display(Name = "STATE")]
		public int? State_Id { get; set; }

		[Display(Name = "COUNTRY")]
		public int? Country_Id { get; set; }


		//[Display(Name = "HANDLING SUBJECTS")]
		//public string Handling_Subjects { get; set; }


		[Display(Name = "EXP(YRS)")]
		public int Experience_in_Years { get; set; }

		[Display(Name = "IS MARRIED?")]
		public bool Is_Married { get; set; }

		[StringLength(6)]
		[Display(Name = "PINCODE")]
		public string PinCode { get; set; }

		//[StringLength(50)]
		//public string Experience { get; set; }
		[Display(Name = "ACADEMIC YEAR")]
		public long? Academic_Year { get; set; }

        public bool? Is_Active { get; set; }

        public bool? Is_Deleted { get; set; }

        public DateTime? Created_On { get; set; }


		[Display(Name = "Created By")]
		public int Created_By { get; set; }

		[Display(Name = "Updated Date")]
		public DateTime? Updated_On { get; set; }

		[Display(Name = "AADHAR NO")]
		public string Aadhar_Number { get; set; }

		[Display(Name = "HANDLING SUBJECTS")]
		public string Handling_Subjects { get; set; }

		[Display(Name = "Updated By")]
		public int Updated_By { get; set; }

		public byte[] Photo { get; set; }

		//[ForeignKey("Staff_Type_Id")]
		//public virtual Staff_Type FStaff_Type { get; set; }

		[ForeignKey("Gender_Id")]
		public virtual Gender Gender { get; set; }

		[ForeignKey("Blood_Group_Id")]
		public virtual Blood_Group BloodGroup { get; set; }

	}
}
