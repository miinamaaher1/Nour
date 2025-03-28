using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace XCourse.Services.Implementations.EmailServices
{
    public class GmailEmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromEmail = "xcourse.platform@gmail.com";
            var fromPassword = "Gz7!qW@9Yp#Xr4$L";

            var message = new MailMessage();
            message.From = new MailAddress(fromEmail);
            message.To.Add(email);
            message.Subject = subject;
            message.Body = $"<html><body>{htmlMessage}</body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };

            smtpClient.Send(message);
        }
    }
}
