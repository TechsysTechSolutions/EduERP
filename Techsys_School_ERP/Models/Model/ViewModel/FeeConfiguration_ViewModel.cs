using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Techsys_School_ERP.Model.ViewModel
{
	public class FeeConfiguration_ViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string User_Id { get; set; }

		public long Academic_Year { get; set; }

		public int Frequency { get; set; }

		public decimal? Amount { get; set; }

		public decimal Total { get; set; }

		public decimal? Yearly_Amount { get; set; }

		public decimal? First_Term_Amount { get; set; }

		public decimal? Second_Term_Amount { get; set; }

		public decimal? Third_Term_Amount { get; set; }

		public String Next_Due_Date { get; set; }

		public decimal? Late_Fees { get; set; }


	}
}