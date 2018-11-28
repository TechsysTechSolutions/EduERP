using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Techsys_School_ERP.Controllers
{
	public class MailController : CommonController
	{
		

		public  ActionResult SendMail(string sEmailId , string sUserId , string sPassword)
		{
			try
			{
				for (int i = 0; i < 1; i++)
				{
					MailMessage mailMsg = new MailMessage();

					// To
					mailMsg.To.Add(new MailAddress("deepikatulip@gmail.com", "To Name"));

					// From
					mailMsg.From = new MailAddress("deepikatulip@gmail.com", "From Name");

					//	mailMsg.Header.SetTo(recipients);

					mailMsg.Subject = "Testing the SendGrid Library";

					string startupPath = AppDomain.CurrentDomain.BaseDirectory;

					string sRandomPassword = CreateRandomPassword(6);

					string targetPath = "https://www.w3schools.com/images/picture.jpg";
					string sPath = "../Content/images/login_cover.jpg";

					// Subject and multipart/alternative Body
					mailMsg.Subject = "subject";
					string text = "text body";
					StringBuilder sbhtml = new StringBuilder();
					sbhtml.Append(@"<p>html body</p>" + i + "sndjw");
					sbhtml.AppendLine("<table>");
					//sbhtml.Append(@);
					sbhtml.Append("<tr style=\" background:gainsboro; \">");
					sbhtml.Append("<td align =\"left\">");
					sbhtml.Append("<p>Welcome Student,</p>");
					sbhtml.Append("<p>Thanks for registering with us.You can login into the school website using the below credentials ,</p>");
					sbhtml.Append("<p style=\"font-weight: bolder;\">User Id : " + sUserId + " </p>");
					sbhtml.Append("<p style=\"font-weight: bolder;\" >Password : " + sPassword + "</p>");
					sbhtml.Append("<img src=\"'" + targetPath  + "'\" >");
					sbhtml.Append("</td>");
					sbhtml.Append("</tr>");
					sbhtml.Append("<tr>");
					sbhtml.Append("<td>");
					sbhtml.Append("<table>");
					sbhtml.Append("<tr>");
					sbhtml.Append("<td>");
					sbhtml.Append("<img src=\" '" + targetPath  + "' \">");
					sbhtml.Append("</td>");
					sbhtml.Append("<td > Sweet, sweet copy </td>");
					sbhtml.Append("</tr>");
					sbhtml.Append("</table>");
					sbhtml.Append("</td>");
					sbhtml.Append("</tr>");
					sbhtml.Append(" <td align = \"left\" > 123 Main St. Nashville, TN 37212 </td>");
					sbhtml.Append("<tr>");
					sbhtml.Append("</tr>");
					sbhtml.Append("</table>");























					string html = @"<p>html body</p>";








					mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
					mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(sbhtml.ToString(), null, MediaTypeNames.Text.Html));

					// Init SmtpClient and send
					SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
					//System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("deepikatulip", "april_11");

					System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("techsystechsolutions", "techsys@2018");
					smtpClient.Credentials = credentials;

					smtpClient.Send(mailMsg);
					/******************************************************************************************************************************************************************/
				}


			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return View();

		}

	}
	}
