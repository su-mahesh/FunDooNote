using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using CommonLayer.RequestModel;
using Microsoft.Extensions.Configuration;

namespace FundooNotes.Services
{
    public class EmailService
    {
        private readonly IConfiguration config;

        public EmailService(IConfiguration config)
        {
            this.config = config;
        }

        public void SendPasswordResetLinkEmail(ForgetPasswordModel resetLink)
        {
            try
            {
                string HtmlBody;
                using (StreamReader streamReader = new StreamReader(config["IssuerEmailDetail:HtmlBodyFile"], Encoding.UTF8))
                {
                    HtmlBody = streamReader.ReadToEnd();
                }
                HtmlBody = HtmlBody.Replace("JwtToken", resetLink.JwtToken);
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(config["IssuerEmailDetail:Email"]);
                message.To.Add(new MailAddress(resetLink.Email));
                message.Subject = "Reset Password";
                message.IsBodyHtml = true;
                message.Body = HtmlBody;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; 
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(config["IssuerEmailDetail:Email"], config["IssuerEmailDetail:Password"]);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) {
                throw;
            }
        }
    }
}
