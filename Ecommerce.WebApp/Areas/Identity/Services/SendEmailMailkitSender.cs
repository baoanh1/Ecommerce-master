using Ecommerce.Identity.Areas.Identity.Email;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Areas.Identity.Services
{
    public class SendEmailMailkitSender : IEmailSender
    {
        public async Task<SendEmailResponse> SendEmailAsync(string userEmail, string emailSubject, string message)
        {
            var messages = new MimeMessage();
            messages.From.Add(new MailboxAddress("Tom Gmail", "phungnhatphu4@gmail.com"));
            messages.To.Add(new MailboxAddress("Tom Hotmail", userEmail));
            messages.Subject = emailSubject;

            messages.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {

                client.Connect("smtp.gmail.com", 587);


                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("phungnhatphu4@gmail.com", "nhatphu90");

                await client.SendAsync(messages);
                client.Disconnect(true);
            }
            return new SendEmailResponse();
        }
    }
}
