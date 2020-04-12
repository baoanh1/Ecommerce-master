using eCommerce.netcore.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.netcore.Areas.Identity
{
    public static class DefaultIdentitySeed
    {
        public static void SeedData (UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers (UserManager<AppUser> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                AppUser user = new AppUser();
                user.UserName = "admin";
                user.Email = "admin@gmail.com";

                IdentityResult result = userManager.CreateAsync
                (user, "NxP@2020").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,"Admin").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<AppRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                AppRole role = new AppRole();
                role.Name = "Admin";                
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Member").Result)
            {
                AppRole role = new AppRole();
                role.Name = "Member";
               
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
        //public static void CreateRolesAndAdminUser(IServiceProvider serviceProvider)
        //{
        //    const string adminRoleName = "Admin";
        //    string[] rolesName = { adminRoleName, "Member"};
        //    foreach(var role in rolesName)
        //    {
        //        CreateRoles(serviceProvider, role);
        //    }
        //    string email = "admin@gmail.com";
        //    string pass = "Admin@12345";
        //    AddUserToRole(serviceProvider, email, pass, adminRoleName);
        //}
        //private static void CreateRoles(IServiceProvider serviceProvider, string rolename)
        //{
        //    var rolemanager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    Task<bool> rolesExists = rolemanager.RoleExistsAsync(rolename);
        //    rolesExists.Wait();
        //    if(!rolesExists.Result)
        //    {
        //        Task<IdentityResult> res = rolemanager.CreateAsync(new IdentityRole(rolename));
        //        res.Wait();
        //    }
        //}
        //private static void AddUserToRole(IServiceProvider serviceProvider, string email, string pass, string rolename)
        //{
        //    var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        //    Task<AppUser> existUser = userManager.FindByEmailAsync(email);
        //    existUser.Wait();
        //    AppUser appUser = existUser.Result;
        //    if(existUser.Result == null)
        //    {
        //        AppUser newUser = new AppUser
        //        {
        //            UserName = "admin",
        //            Email = email
        //        };
        //        Task<IdentityResult> TaskcreateUser = userManager.CreateAsync(newUser, pass);
        //        TaskcreateUser.Wait();
        //        if(TaskcreateUser.Result.Succeeded)
        //        {
        //            appUser = newUser;
        //        }
        //    }
        //    Task<IdentityResult> newUserRole = userManager.CreateAsync(appUser, rolename);
        //    newUserRole.Wait();
        //}
    }
}
