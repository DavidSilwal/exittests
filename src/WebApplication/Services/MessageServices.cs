using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            //var emailMessage = new MimeMessage();
            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("David Silwal", "msunuwar11@outlook.com"));
            //message.To.Add(new MailboxAddress("David Silwal", email));
            //message.Subject = subject;
            //message.Body = new TextPart("plain")
            //{
            //    Text = msg
            //};

            //using (var client = new SmtpClient())
            //{
            //    client.Connect("smtp.live.com", 587, false);
            //    client.AuthenticationMechanisms.Remove("XOAUTH2");
            //    client.Authenticate("msunuwar11@outlook.com", "9843104350Qwe123");
            //    client.Send(message);
            //    client.Disconnect(true);
            //}

            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
