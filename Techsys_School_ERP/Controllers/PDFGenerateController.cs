using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Techsys_School_ERP.DBAccess;
using Techsys_School_ERP.Model;
using System.Web.Security;
using System.Data.Entity;
using Techsys_School_ERP.Model.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Globalization;
using System.Text;

namespace Techsys_School_ERP.Controllers
{
	public class PDFGenerateController : CommonController

	{

		
		public static PdfPTable GenerateFeesPaymentRecipt(Fee_Payment fee_Payment)
		{

			CommonController oCommonController = new CommonController();
			PdfPTable outerTable = new PdfPTable(1);

			try
			{


				PdfPTable headingTable = new PdfPTable(1);

				PdfPCell headingTableCell = new PdfPCell(new Phrase("FEE RECIPT" + " - " + Convert.ToString(""), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				headingTableCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				headingTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
				headingTableCell.PaddingTop = 45f;
				headingTableCell.PaddingBottom = 45f;
				headingTable.AddCell(headingTableCell);


				/*****************************************************************************************LOGO DESIGN *******************************************************************************/
				PdfPTable logoTable = new PdfPTable(2);


				string startupPath = AppDomain.CurrentDomain.BaseDirectory;

				string targetPath = startupPath + "Content\\images\\imageupload_-_Copy.png";



				iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(targetPath);
				//Resize image depend upon your need
				jpg.ScaleToFit(200f, 60f);
				//Give space before image
				jpg.SpacingBefore = 20f;
				//jpg.SpacingBefore = 150f;
				//Give some space after the image
				jpg.SpacingAfter = 20f;
				jpg.Alignment = Element.ALIGN_CENTER;


				PdfPCell cellImage = new PdfPCell(jpg);
				cellImage.PaddingLeft = 2f;
				//cellImage.Border = iTextSharp.text.Rectangle.NO_BORDER;


				PdfPCell logoCell1 = new PdfPCell();

				cellImage.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellImage.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				logoCell1.Border = iTextSharp.text.Rectangle.NO_BORDER;

				logoTable.AddCell(cellImage);
				logoTable.AddCell(logoCell1);

				

				/*****************************************************************************************LOGO DESIGN *******************************************************************************/
				PdfPTable studentDetailTable = new PdfPTable(3);
				float[] studentDetailTablewidths = new float[] { 200f, 200f, 200f };
				studentDetailTable.SetWidths(studentDetailTablewidths);
				/****************************************************FIRST TABLE (EMPLOYEE DETAILS ********************************************************************************************************************************/
				PdfPCell studentDetailTableCell11 = new PdfPCell(new Phrase("RECIPT NO  " + "\n"  + "\n"+ Convert.ToString(fee_Payment.Recipt_no) + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				studentDetailTableCell11.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				studentDetailTableCell11.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				studentDetailTableCell11.PaddingTop = 15f;
				studentDetailTableCell11.PaddingBottom = 15f;

				//PdfPCell studentDetailTableCell11a = new PdfPCell(new Phrase("PAYMENT DATE  " +"\n" + "\n" + Convert.ToString(fee_Payment.Payment_date), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				//studentDetailTableCell11a.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				//studentDetailTableCell11a.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				////staffDetailTableCell11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				//studentDetailTableCell11a.PaddingTop = 15f;
				//studentDetailTableCell11a.PaddingBottom = 15f;

				PdfPCell studentDetailTableCell12 = new PdfPCell(new Phrase("PAYMENT DATE  " + "\n" +"\n" + Convert.ToString(fee_Payment.Payment_date), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				studentDetailTableCell12.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				studentDetailTableCell12.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				//staffDetailTableCell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				studentDetailTableCell12.PaddingTop = 15f;
				studentDetailTableCell12.PaddingBottom = 15f;

				//PdfPCell studentDetailTableCell12a = new PdfPCell(new Phrase(Convert.ToString(fee_Payment.Payment_date), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				//studentDetailTableCell12a.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				////studentDetailTableCell12a.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				//studentDetailTableCell12a.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				//studentDetailTableCell12a.PaddingTop = 15f;
				//studentDetailTableCell12a.PaddingBottom = 15f;

				Student student = new Student();

				using (var dbcontext = new SchoolERPDBContext())
				{
					student = dbcontext.Student.Where(x => x.Student_Id == fee_Payment.Student_id).FirstOrDefault();


				}


				using (var dbcontext = new SchoolERPDBContext())
				{
					student = dbcontext.Student.Where(x => x.Student_Id == fee_Payment.Student_id).FirstOrDefault();


				}

				PdfPCell studentDetailTableCell13 = new PdfPCell(new Phrase("STUDENT NAME " + "\n" +"\n" + student.First_Name + " " + student.Last_Name + " " + student.Middle_Name, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				studentDetailTableCell13.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				studentDetailTableCell13.Border = iTextSharp.text.Rectangle.NO_BORDER;
				studentDetailTableCell13.PaddingTop = 15f;
				studentDetailTableCell13.PaddingBottom = 15f;


			

				PdfPCell studentDetailTableCell21 = new PdfPCell(new Phrase("ROLL NO" + "\n" + "\n" + student.Roll_No, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				studentDetailTableCell21.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				studentDetailTableCell21.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				studentDetailTableCell21.PaddingTop = 15f;
				studentDetailTableCell21.PaddingBottom = 15f;



				string sFrequency = string.Empty;
				using (var dbcontext = new SchoolERPDBContext())
				{
					sFrequency = dbcontext.Frequency.Where(x => x.Id == fee_Payment.Frequency).FirstOrDefault().Name;

				}

				PdfPCell studentDetailTableCell22 = new PdfPCell(new Phrase("FREQUENCY" + "\n" + "\n" + sFrequency, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				studentDetailTableCell22.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				studentDetailTableCell22.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				studentDetailTableCell22.PaddingTop = 15f;
				studentDetailTableCell22.PaddingBottom = 15f;

			

				PdfPCell studentDetailTableCell23 = new PdfPCell(new Phrase("CLASS & SECTION" + "\n" + "\n" + Convert.ToString(oCommonController.GetClassANdSectionForStudent(fee_Payment.Student_id)), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				studentDetailTableCell23.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				studentDetailTableCell23.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				studentDetailTableCell23.PaddingTop = 15f;
				studentDetailTableCell23.PaddingBottom = 15f;


				studentDetailTable.AddCell(studentDetailTableCell11);
				studentDetailTable.AddCell(studentDetailTableCell12);
				studentDetailTable.AddCell(studentDetailTableCell13);
				studentDetailTable.AddCell(studentDetailTableCell21);

				studentDetailTable.AddCell(studentDetailTableCell22);
				studentDetailTable.AddCell(studentDetailTableCell23);
				
				List<FeeConfiguration_ViewModel> lstFeeConfig = new List<FeeConfiguration_ViewModel>();
				lstFeeConfig = oCommonController.GetFeeStructureForStudent(Convert.ToString(fee_Payment.Student_id), Convert.ToString(fee_Payment.Frequency));


				PdfPTable mySIItemDescriptionTable = new PdfPTable(1);
				PdfPTable feeComponentDescTable = new PdfPTable(1);
				PdfPTable amountDescriptionTable = new PdfPTable(1);

				PdfPTable feeTable = new PdfPTable(4);
				float[] feeTablewidths = new float[] { 50f, 250f, 50f,50f };
				feeTable.SetWidths(feeTablewidths);
				feeTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

				//mySIItemDescriptionTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
				//mySIItemDescriptionTable.DefaultCell.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				//feeComponentDescTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
				//amountDescriptionTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

				//feeTable.AddCell(new Phrase(Convert.ToString("SI NO") + "\n" + "", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));


				//feeTable.AddCell(new Phrase(Convert.ToString("FEE COMPONENT") + "\n" + "", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));

				//feeTable.AddCell(new Phrase(Convert.ToString("AMOUNT") + "\n" + "", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));

				//feeTable.AddCell(new Phrase(Convert.ToString("PAISE") + "\n" + "", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));

				PdfPCell SIheaderItemCell = new PdfPCell(new Phrase(Convert.ToString("SI NO"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				SIheaderItemCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				SIheaderItemCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER ;
				SIheaderItemCell.PaddingTop = 45f;
				SIheaderItemCell.PaddingBottom = 45f;

				PdfPCell feeComponentheaderCell = new PdfPCell(new Phrase(Convert.ToString("FEE COMPONENT"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				feeComponentheaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				feeComponentheaderCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER ;
				feeComponentheaderCell.PaddingTop = 45f;
				feeComponentheaderCell.PaddingBottom = 45f;

				PdfPCell amountItemheaderCell = new PdfPCell(new Phrase(Convert.ToString("AMOUNT"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				amountItemheaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				amountItemheaderCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				amountItemheaderCell.PaddingTop = 45f;
				amountItemheaderCell.PaddingBottom = 45f;

				PdfPCell paiseItemheaderCell = new PdfPCell(new Phrase(Convert.ToString("PAISE"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				paiseItemheaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				paiseItemheaderCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				paiseItemheaderCell.PaddingTop = 45f;
				paiseItemheaderCell.PaddingBottom = 45f;


				feeTable.AddCell(SIheaderItemCell);
				feeTable.AddCell(feeComponentheaderCell);
				feeTable.AddCell(amountItemheaderCell);
				feeTable.AddCell(paiseItemheaderCell);
				
				for (int nRowCount = 0; nRowCount < lstFeeConfig.ToList().Count; nRowCount++)
				{

					PdfPCell SIItemCell = new PdfPCell(new Phrase(Convert.ToString(nRowCount + 1) + ". " , new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					SIItemCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
					SIItemCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER  | iTextSharp.text.Rectangle.TOP_BORDER;
					SIItemCell.PaddingTop = 15f;
					SIItemCell.PaddingBottom = 15f;

					PdfPCell feeComponentCell = new PdfPCell(new Phrase(Convert.ToString(lstFeeConfig[nRowCount].Name), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					feeComponentCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					feeComponentCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
					feeComponentCell.PaddingTop = 15f;
					feeComponentCell.PaddingBottom = 15f;

					var values = Convert.ToString(lstFeeConfig[nRowCount].Amount).Split('.');
					int firstValue = int.Parse(values[0]);

					PdfPCell amountItemCell = new PdfPCell(new Phrase(string.Format(new CultureInfo("en-IN", false), "{0:n0}", firstValue), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					amountItemCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
					amountItemCell.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					amountItemCell.PaddingTop = 15f;
					amountItemCell.PaddingBottom = 15f;

					PdfPCell paiseItemCell = new PdfPCell(new Phrase(Convert.ToString(values[1]), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
					paiseItemCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
					paiseItemCell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					paiseItemCell.PaddingTop = 15f;
					paiseItemCell.PaddingBottom = 15f;
					
					feeTable.AddCell(SIItemCell);
					feeTable.AddCell(feeComponentCell);
					feeTable.AddCell(amountItemCell);
					feeTable.AddCell(paiseItemCell);
					feeTable.AddCell(SIItemCell);
					feeTable.AddCell(feeComponentCell);
					feeTable.AddCell(amountItemCell);
					feeTable.AddCell(paiseItemCell);

				}

				/*********************************************************************GROSS SALARY **********************************************************************************************/

				PdfPTable grossAndNetAmountTable = new PdfPTable(3);

				float[] width = new float[] { 300f, 50f , 50f };
				grossAndNetAmountTable.SetWidths(width);

				PdfPCell grossAndNetAmountTableCell11 = new PdfPCell(new Phrase(Convert.ToString("TOTAL AMOUNT"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				grossAndNetAmountTableCell11.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
				grossAndNetAmountTableCell11.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				grossAndNetAmountTableCell11.PaddingTop = 15f;
				grossAndNetAmountTableCell11.PaddingBottom = 15f;

				var totalvalues = Convert.ToString(lstFeeConfig[0].Total).Split('.');
				int totalfirstValue = int.Parse(totalvalues[0]);

				PdfPCell grossAndNetAmountTableCell12 = new PdfPCell(new Phrase(Convert.ToString(string.Format(new CultureInfo("en-IN", false), "{0:n0}", totalfirstValue)), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				grossAndNetAmountTableCell12.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				grossAndNetAmountTableCell12.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				grossAndNetAmountTableCell12.PaddingTop = 15f;
				grossAndNetAmountTableCell12.PaddingBottom = 15f;

				PdfPCell grossAndNetAmountTableCell13 = new PdfPCell(new Phrase(Convert.ToString(totalvalues[1]), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				grossAndNetAmountTableCell13.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				grossAndNetAmountTableCell13.Border = iTextSharp.text.Rectangle.NO_BORDER;
				grossAndNetAmountTableCell13.PaddingTop = 15f;
				grossAndNetAmountTableCell13.PaddingBottom = 15f;



				grossAndNetAmountTable.AddCell(grossAndNetAmountTableCell11);
				grossAndNetAmountTable.AddCell(grossAndNetAmountTableCell12);
				grossAndNetAmountTable.AddCell(grossAndNetAmountTableCell13);

				/*********************************************************************GROSS SALARY ENDS **********************************************************************************************/

				
				PdfPTable amountInWordsTable = new PdfPTable(1);
				PdfPCell amountInWordstTableCell11 = new PdfPCell(new Phrase(Convert.ToString("AMOUNT IN WORDS : " + NumberToWordsConverter.NumToWord(Convert.ToDecimal(totalvalues[0]))), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				amountInWordstTableCell11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				amountInWordstTableCell11.Border = iTextSharp.text.Rectangle.NO_BORDER;
				amountInWordstTableCell11.PaddingTop = 45f;
				amountInWordstTableCell11.PaddingBottom = 45f;

				amountInWordsTable.AddCell(amountInWordstTableCell11);



				PdfPTable authorisedSignatoryTable = new PdfPTable(1);
				PdfPCell authorisedSignatoryTableCell11 = new PdfPCell(new Phrase(Convert.ToString("AUTHORISED SIGNATURE "), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				authorisedSignatoryTableCell11.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
				authorisedSignatoryTableCell11.Border = iTextSharp.text.Rectangle.NO_BORDER;
				authorisedSignatoryTableCell11.PaddingTop = 45f;
				authorisedSignatoryTableCell11.PaddingBottom = 45f;

				authorisedSignatoryTable.AddCell(authorisedSignatoryTableCell11);

				PdfPTable LateFeesAndDueDateTable = new PdfPTable(2);

				PdfPCell LateFeesAndDueDateTableCell11 = new PdfPCell(new Phrase(Convert.ToString("LATE FEES IF ANY : " + Convert.ToString(lstFeeConfig[0].Late_Fees)), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				LateFeesAndDueDateTableCell11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				LateFeesAndDueDateTableCell11.Border = iTextSharp.text.Rectangle.NO_BORDER;
				LateFeesAndDueDateTableCell11.PaddingTop = 25f;
				LateFeesAndDueDateTableCell11.PaddingBottom = 25f;

			//	DateTime dtNextDueDate = DateTime.ParseExact(Convert.ToString(lstFeeConfig[0].Next_Due_Date), "dd-MM-yy", null);

				PdfPCell LateFeesAndDueDateTableCell12 = new PdfPCell(new Phrase(Convert.ToString("NEXT DUE DATE : " + Convert.ToString(Convert.ToDateTime(lstFeeConfig[0].Next_Due_Date).ToShortDateString())), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 22.5f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(Color.Black))));
				LateFeesAndDueDateTableCell12.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				LateFeesAndDueDateTableCell12.Border = iTextSharp.text.Rectangle.NO_BORDER;
				LateFeesAndDueDateTableCell12.PaddingTop = 25f;
				LateFeesAndDueDateTableCell12.PaddingBottom = 25f;

				LateFeesAndDueDateTable.AddCell(LateFeesAndDueDateTableCell11);
				LateFeesAndDueDateTable.AddCell(LateFeesAndDueDateTableCell12);


				outerTable.AddCell(headingTable);
				outerTable.AddCell(logoTable);
				outerTable.AddCell(studentDetailTable);
			    outerTable.AddCell(feeTable);
				outerTable.AddCell(grossAndNetAmountTable);
				outerTable.AddCell(amountInWordsTable);
				outerTable.AddCell(LateFeesAndDueDateTable);
				outerTable.AddCell(authorisedSignatoryTable);

			}
			catch (Exception ex)
			{

			}

			return outerTable;
			//return null;
		}


		public  void DownloadFileFromServer(string Id, string Invoice_Number)
		{
			try
			{
				string sInvoice_Name;
			
				string path = Server.MapPath("~/views//billing//Invoice/PDF_Document/" + Id+ "_" + Invoice_Number);

				System.IO.FileInfo file = new System.IO.FileInfo(path);
				if (file.Exists)
				{
					Response.Clear();
					Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
					Response.AddHeader("Content-Length", file.Length.ToString());
					Response.ContentType = "application/....";
					Response.WriteFile(file.FullName);
					Response.End();
				}
				else
				{
					Response.Write("This file does not exist.");
				}
			}



			catch (Exception rt)
			{
				// Response.Write(rt.Message);
			}
		}
	}

	public  class NumberToWordsConverter
	{
		private string[] numbers = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
		private string[] tens = { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

		public NumberToWordsConverter()
		{

		}

		public string AmountInWords(decimal Num)
		{
			string returnValue;
			//I have created this function for converting amount in indian rupees (INR).
			//You can manipulate as you wish like decimal setting, Doller (any currency) Prefix.


			string strNum;
			string strNumDec;
			string StrWord;
			strNum = Num.ToString();


			if (strNum.IndexOf(".") + 1 != 0)
			{
				strNumDec = strNum.Substring(strNum.IndexOf(".") + 2 - 1);


				if (strNumDec.Length == 1)
				{
					strNumDec = strNumDec + "0";
				}
				if (strNumDec.Length > 2)
				{
					strNumDec = strNumDec.Substring(0, 2);
				}


				strNum = strNum.Substring(0, strNum.IndexOf(".") + 0);
				StrWord = ((double.Parse(strNum) == 1) ? " Rupee " : " Rupees ") + NumToWord((decimal)(double.Parse(strNum))) + ((double.Parse(strNumDec) > 0) ? (" and Paise" + cWord3((decimal)(double.Parse(strNumDec)))) : "");
			}
			else
			{
				StrWord = ((double.Parse(strNum) == 1) ? " Rupee " : " Rupees ") + NumToWord((decimal)(double.Parse(strNum)));
			}
			returnValue = StrWord + " Only";
			return returnValue;
		}
		static public string NumToWord(decimal Num)
		{
			string returnValue;


			//I divided this function in two part.
			//1. Three or less digit number.
			//2. more than three digit number.
			string strNum;
			string StrWord;
			strNum = Num.ToString();


			if (strNum.Length <= 3)
			{
				StrWord = cWord3((decimal)(double.Parse(strNum)));
			}
			else
			{
				StrWord = cWordG3((decimal)(double.Parse(strNum.Substring(0, strNum.Length - 3)))) + " " + cWord3((decimal)(double.Parse(strNum.Substring(strNum.Length - 2 - 1))));
			}
			returnValue = StrWord;
			return returnValue;
		}
		static public string cWordG3(decimal Num)
		{
			string returnValue;
			//2. more than three digit number.
			string strNum = "";
			string StrWord = "";
			string readNum = "";
			strNum = Num.ToString();
			if (strNum.Length % 2 != 0)
			{
				readNum = System.Convert.ToString(double.Parse(strNum.Substring(0, 1)));
				if (readNum != "0")
				{
					StrWord = retWord(decimal.Parse(readNum));
					readNum = System.Convert.ToString(double.Parse("1" + strReplicate("0", strNum.Length - 1) + "000"));
					StrWord = StrWord + " " + retWord(decimal.Parse(readNum));
				}
				strNum = strNum.Substring(1);
			}
			while (!System.Convert.ToBoolean(strNum.Length == 0))
			{
				readNum = System.Convert.ToString(double.Parse(strNum.Substring(0, 2)));
				if (readNum != "0")
				{
					StrWord = StrWord + " " + cWord3(decimal.Parse(readNum));
					readNum = System.Convert.ToString(double.Parse("1" + strReplicate("0", strNum.Length - 2) + "000"));
					StrWord = StrWord + " " + retWord(decimal.Parse(readNum));
				}
				strNum = strNum.Substring(2);
			}
			returnValue = StrWord;
			return returnValue;
		}
		static public string cWord3(decimal Num)
		{
			string returnValue;
			//1. Three or less digit number.
			string strNum = "";
			string StrWord = "";
			string readNum = "";
			if (Num < 0)
			{
				Num = Num * -1;
			}
			strNum = Num.ToString();


			if (strNum.Length == 3)
			{
				readNum = System.Convert.ToString(double.Parse(strNum.Substring(0, 1)));
				StrWord = retWord(decimal.Parse(readNum)) + " Hundred";
				strNum = strNum.Substring(1, strNum.Length - 1);
			}


			if (strNum.Length <= 2)
			{
				if (double.Parse(strNum) >= 0 && double.Parse(strNum) <= 20)
				{
					StrWord = StrWord + " " + retWord((decimal)(double.Parse(strNum)));
				}
				else
				{
					StrWord = StrWord + " " + retWord((decimal)(System.Convert.ToDouble(strNum.Substring(0, 1) + "0"))) + " " + retWord((decimal)(double.Parse(strNum.Substring(1, 1))));
				}
			}


			strNum = Num.ToString();
			returnValue = StrWord;
			return returnValue;
		}
		static public string retWord(decimal Num)
		{
			string returnValue;
			//This two dimensional array store the primary word convertion of number.
			returnValue = "";
			object[,] ArrWordList = new object[,] { { 0, "" }, { 1, "One" }, { 2, "Two" }, { 3, "Three" }, { 4, "Four" }, { 5, "Five" }, { 6, "Six" }, { 7, "Seven" }, { 8, "Eight" }, { 9, "Nine" }, { 10, "Ten" }, { 11, "Eleven" }, { 12, "Twelve" }, { 13, "Thirteen" }, { 14, "Fourteen" }, { 15, "Fifteen" }, { 16, "Sixteen" }, { 17, "Seventeen" }, { 18, "Eighteen" }, { 19, "Nineteen" }, { 20, "Twenty" }, { 30, "Thirty" }, { 40, "Forty" }, { 50, "Fifty" }, { 60, "Sixty" }, { 70, "Seventy" }, { 80, "Eighty" }, { 90, "Ninety" }, { 100, "Hundred" }, { 1000, "Thousand" }, { 100000, "Lakh" }, { 10000000, "Crore" } };


			int i;
			for (i = 0; i <= (ArrWordList.Length - 1); i++)
			{
				if (Num == System.Convert.ToDecimal(ArrWordList[i, 0]))
				{
					returnValue = (string)(ArrWordList[i, 1]);
					break;
				}
			}
			return returnValue;
		}
		static public string strReplicate(string str, int intD)
		{
			string returnValue;
			//This fucntion padded "0" after the number to evaluate hundred, thousand and on....
			//using this function you can replicate any Charactor with given string.
			int i;
			returnValue = "";
			for (i = 1; i <= intD; i++)
			{
				returnValue = returnValue + str;
			}
			return returnValue;
		}

		public string ConvertNumbertoWords(long number)
		{
			if (number == 0) return "ZERO";
			if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
			string words = "";
			if ((number / 1000000) > 0)
			{
				words += ConvertNumbertoWords(number / 100000) + " LAKES ";
				number %= 1000000;
			}
			if ((number / 1000) > 0)
			{
				words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
				number %= 1000;
			}
			if ((number / 100) > 0)
			{
				words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
				number %= 100;
			}
			//if ((number / 10) > 0)  
			//{  
			// words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
			// number %= 10;  
			//}  
			if (number > 0)
			{
				if (words != "") words += "AND ";
				var unitsMap = new[]
				{
			"ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
		};
				var tensMap = new[]
				{
			"ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
		};
				if (number < 20) words += unitsMap[number];
				else
				{
					words += tensMap[number / 10];
					if ((number % 10) > 0) words += " " + unitsMap[number % 10];
				}
			}
			return words;
		}

		public string SpellDecimal(decimal number)
		{
			string[] digit =
	  {
			"", "one", "two", "three", "four", "five", "six",
			"seven", "eight", "nine", "ten", "eleven", "twelve",
			"thirteen", "fourteen", "fifteen", "sixteen",
			"seventeen", "eighteen", "nineteen"
	  };

			string[] baseten =
	  {
			"", "", "twenty", "thirty", "fourty", "fifty",
			"sixty", "seventy", "eighty", "ninety"
	  };

			string[] expo =
	  {
			"", "thousand", "million", "billion", "trillion",
			"quadrillion", "quintillion"
	  };

			if (number == Decimal.Zero)
				return "zero";

			decimal n = Decimal.Truncate(number);
			decimal cents = Decimal.Truncate((number - n) * 100);

			StringBuilder sb = new StringBuilder();
			int thousands = 0;
			decimal power = 1;

			if (n < 0)
			{
				sb.Append("minus ");
				n = -n;
			}

			for (decimal i = n; i >= 1000; i /= 1000)
			{
				power *= 1000;
				thousands++;
			}

			bool sep = false;
			for (decimal i = n; thousands >= 0; i %= power, thousands--, power /= 1000)
			{
				int j = (int)(i / power);
				int k = j % 100;
				int hundreds = j / 100;
				int tens = j % 100 / 10;
				int ones = j % 10;

				if (j == 0)
					continue;

				if (hundreds > 0)
				{
					if (sep)
						sb.Append(", ");

					sb.Append(digit[hundreds]);
					sb.Append(" hundred");
					sep = true;
				}

				if (k != 0)
				{
					if (sep)
					{
						sb.Append(" and ");
						sep = false;
					}

					if (k < 20)
						sb.Append(digit[k]);
					else
					{
						sb.Append(baseten[tens]);
						if (ones > 0)
						{
							sb.Append("-");
							sb.Append(digit[ones]);
						}
					}
				}

				if (thousands > 0)
				{
					sb.Append(" ");
					sb.Append(expo[thousands]);
					sep = true;
				}
			}

			/* sb.Append(" and ");
			 if (cents < 10) sb.Append("0");
			 sb.Append(cents);
			 sb.Append("/100");*/
			//string s = new CultureInfo("en-US").TextInfo.ToTitleCase(
			return new CultureInfo("en-US").TextInfo.ToTitleCase(sb.ToString() + " ONLY");
		}
	}
}