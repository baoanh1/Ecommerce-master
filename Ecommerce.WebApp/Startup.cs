using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Application.Services.DependencyInjection;
using Ecommerce.Core.Services;
using Ecommerce.Data.EF;
using Ecommerce.Data.Entities;
using Ecommerce.Identity.Areas.Chat;
using Ecommerce.Identity.Areas.Identity;
using Ecommerce.Identity.Areas.Identity.Email;
using Ecommerce.Identity.Areas.Identity.Helpers;
using Ecommerce.Identity.Chat.Hubs;
using Ecommerce.WebApp.Areas.Admin;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartBreadcrumbs.Extensions;

namespace Ecommerce.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            var identitySettingsSection =
                 Configuration.GetSection("AppSettings");
            services.Configure<Appsettings>(identitySettingsSection);
            services.AddSession();
            services.AddDbContext<EcommerceDbContext>(options =>
			{
				options.UseSqlite("Data Source=Ecomerce.db");
			});
            
            //var sv = services.AddSendGridEmailSender();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            //services.AddScoped<MediaService>();
            services.AddUnitOfWork<EcommerceDbContext>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            var sv1 = services.AddCustomIdentity(Configuration);
            services.AddMvc();
            
            services.AddSignalR();
            services.AddBreadcrumbs(GetType().Assembly, options =>
            {
                options.DontLookForDefaultNode = true;
                options.TagName = "nav";
                options.TagClasses = "mynav";
                options.OlClasses = "breadcrumb fa fa-home";
                options.LiClasses = "breadcrumb-item";
                options.ActiveLiClasses = "breadcrumb-item active";
                options.SeparatorElement = "<li class=\"separator pl-2 pr-2\"> <i class=\"fa fa-angle-double-right\"></i> </li>";
                // Testing
                options.DontLookForDefaultNode = true;

            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            //DefaultIdentitySeed.SeedData(userManager, roleManager);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UserChatModule();
            app.useAdminModule();
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
