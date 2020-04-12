
using Ecommerce.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Identity.Areas.Identity.ViewModels
{
    public class UserEditViewModel
    {
        public AppUser User { get; set; }
        public IList<AppRole> Roles { get; set; } = new List<AppRole>();
        public IList<string> SelectedRoles { get; set; } = new List<string>();
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public async Task<IdentityResult> Save(UserManager<AppUser> userManager)
        {
            IdentityResult result;
            var user = await userManager.FindByIdAsync(User.Id.ToString());

            if (user == null)
            {
                user = new AppUser
                {
                    Id = User.Id != Guid.Empty ? User.Id : Guid.NewGuid(),
                    UserName = User.UserName,
                    Email = User.Email
                };
                User.Id = user.Id;
                result = await userManager.CreateAsync(user, Password);
                if (!result.Succeeded)
                {
                    return result;
                }
            }
            else
            {
                result = await userManager.SetUserNameAsync(user, User.UserName);
                if (!result.Succeeded)
                {
                    return result;
                }

                result = await userManager.SetEmailAsync(user, User.Email);
                if (!result.Succeeded)
                {
                    return result;
                }
                if (!string.IsNullOrWhiteSpace(Password))
                {
                    //result = await userManager.RemovePasswordAsync(user);
                    //if (!result.Succeeded)
                    //{
                    //    return result;
                    //}
                    //result = await userManager.AddPasswordAsync(user, Password);
                    //if (!result.Succeeded)
                    //{
                    //    return result;
                    //}
                    string resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                    result = await userManager.ResetPasswordAsync(user, resetToken, Password);

                    //ChangePasswordAsync changes the user password
                    //result = await userManager.ChangePasswordAsync(user, Password, PasswordConfirm);
                    if (!result.Succeeded)
                    {
                        return result;
                    }
                }
            }

            // Remove old roles
            var roles = await userManager.GetRolesAsync(user);
            result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                return result;
            }

            // Add current roles
            result = await userManager.AddToRolesAsync(user, SelectedRoles);
            if (!result.Succeeded)
            {
                return result;
            }
            return result;
        }
    }
}
