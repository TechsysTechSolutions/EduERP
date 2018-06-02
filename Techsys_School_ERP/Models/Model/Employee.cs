using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model
{
	public class Employee
	{
		public int EmployeeID { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string EmailID { get; set; }

		public string City { get; set; }

		public string Country { get; set; }
	}
}