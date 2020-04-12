using Ecommerce.Data.EF;
using Ecommerce.Data.Entities;
using Ecommerce.Identity.Areas.Identity.Email;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Identity.Areas.Identity
{
    public static class IdentityModuleExtensions
    {
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //options.Lockout.MaxFailedAccessAttempts = 5;
                //options.Lockout.AllowedForNewUsers = false;
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<EcommerceDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication().AddGoogle(option =>
            {
                option.ClientId = "321191039276-rhjpf9put7r6iek3qv1hm5kk9jb4bq2n.apps.googleusercontent.com";
                option.ClientSecret = "rsUXY7ZxI6ih2eQFzB5Aao-S";
            })
                 .AddFacebook(option =>
                 {
                     option.AppId = "2054085701488076";
                     option.AppSecret = "72e3439d9b76567f6a095184290be586";
                 });

            var sv = services.AddSendGridEmailSender();
            
            return services;
      
        }
    }
}
