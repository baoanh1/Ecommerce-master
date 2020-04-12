using Ecommerce.Identity.Areas.Identity.Services;
using Ecommerce.WebApp.Areas.Identity.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Identity.Areas.Identity.Email
{
    public static class SendGridExtension
    {
        public static IServiceCollection AddSendGridEmailSender(this IServiceCollection service)
        {
            //service.AddTransient<IEmailSender, SendEmailGridSender>();
            service.AddTransient<IEmailSender, SendEmailMailkitSender>();
            
            return service;
        }
    }
}
