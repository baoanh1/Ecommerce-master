using Ecommerce.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Areas.Identity.ViewModels
{
    public class RoleEditModel
    {
        public RoleEditModel()
        {
            SelectedClaims = new List<string>();
        }

        public AppRole Role { get; set; }
        public IList<string> SelectedClaims { get; set; }

        public static RoleEditModel GetById(RoleManager<AppRole> roleManager, Guid id)
        {
            var role = roleManager.FindByIdAsync(id.ToString());

            if (role != null)
            {
                var model = new RoleEditModel
                {
                    Role = role.Result
                };

                var roleClaims = roleManager.GetClaimsAsync(role.Result).ToString();
                foreach (var claim in roleClaims)
                {
                    model.SelectedClaims.Add(claim.ToString());
                }
                return model;
            }

            return null;
        }

        public static RoleEditModel Create()
        {
            return new RoleEditModel
            {
                Role = new AppRole()
            };
        }

        public bool Save(RoleManager<AppRole> roleManager)
        {
            var role = roleManager.FindByIdAsync(Role.Id.ToString()).Result;
            if (role == null)
            {
                Role.Id = Role.Id != Guid.Empty ? Role.Id : Guid.NewGuid();
                Role.NormalizedName = !string.IsNullOrEmpty(Role.NormalizedName)
                    ? Role.NormalizedName.ToUpper()
                    : Role.Name.ToUpper();

                var newrole = new AppRole
                {
                    Id = Role.Id
                };
                roleManager.CreateAsync(role);
            }
            
            else
            {
                role.Name = Role.Name;
                role.NormalizedName = Role.NormalizedName;
                roleManager.UpdateAsync(role);
            }

            var claims = roleManager.GetClaimsAsync(role).Result;
         
            var delete = new List<IdentityRoleClaim<Guid>>();
            var add = new List<IdentityRoleClaim<Guid>>();
            
            foreach (var old in claims)
            {
                if (!SelectedClaims.Contains(old.ToString()))
                {
                    roleManager.RemoveClaimAsync(role, old);
                }
            }

            foreach (var selected in SelectedClaims)
            {
                if (!claims.Any(c => c.Type == selected))
                {
                    add.Add(new IdentityRoleClaim<Guid>
                    {
                        RoleId = role.Id,
                        ClaimType = selected,
                        ClaimValue = selected
                    });
                    
                }
            }
            foreach (var item in add)
            {
                roleManager.RemoveClaimAsync(role, item.ToClaim());
            }

            return true;
        }
    }
}
