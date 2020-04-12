using Ecommerce.Data.EF;
using Ecommerce.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Areas.Identity.ViewModels
{
    public class RoleListModel
    {
        public RoleListModel()
        {
            Roles = new List<ListItem>();
        }

        public IList<ListItem> Roles { get; set; }

        public static RoleListModel Get(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            var model = new RoleListModel
            {
                Roles = roleManager.Roles
                    .OrderBy(r => r.Name)
                    .Select(r => new ListItem
                    {
                        Id = r.Id,
                        Name = r.Name
                    }).ToList()
            };

            foreach (var role in model.Roles)
            {
                
                var userinrole = userManager.GetUsersInRoleAsync(role.Name);
                role.UserCount = userinrole.Result.Count;
               
            }
            return model;
        }

        public class ListItem
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public int UserCount { get; set; }
        }
    }
}
