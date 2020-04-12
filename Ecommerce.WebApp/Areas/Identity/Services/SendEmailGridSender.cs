using Ecommerce.Identity.Areas.Identity.Email;
using Ecommerce.Identity.Areas.Identity.Helpers;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Identity.Areas.Identity.Services
{
    
    public class SendEmailGridSender : IEmailSender
    {
        private readonly Appsettings _appSettings;
        public SendEmailGridSender(IOptions<Appsettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public async Task<SendEmailResponse> SendEmailAsync(string userEmail, string emailSubject, string message)
        {
            var apiKey = _appSettings.SendGridKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("phungnhatphu4@gmail.com", _appSettings.SendGridUser);
            var subject = emailSubject;
            var to = new EmailAddress(userEmail, "test");
            var plainTextContent = message;
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return new SendEmailResponse();
        }
    }
}
