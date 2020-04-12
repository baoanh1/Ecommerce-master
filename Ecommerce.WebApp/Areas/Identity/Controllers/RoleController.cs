using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Data.Entities;
using Ecommerce.WebApp.Areas.Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Identity.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IdentityRoleClaim<Guid> roleClaim;
        public RoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [Route("/manager/roles")]
        
        public IActionResult List()
        {
            return View(RoleListModel.Get(_roleManager, _userManager));
        }

        [Route("/manager/role/{id:Guid}")]
        
        public IActionResult Edit(Guid id)
        {
            return View("Edit", RoleEditModel.GetById(_roleManager, id));
        }

        [Route("/manager/role")]
    
        public IActionResult Add()
        {
            return View("Edit", RoleEditModel.Create());
        }

        [HttpPost]
        [Route("/manager/role/save")]
   
        public IActionResult Save(RoleEditModel model)
        {
            if (model.Save(_roleManager))
            {
                //SuccessMessage("The role has been saved.");
                return RedirectToAction("Edit", new { id = model.Role.Id });
            }

            //ErrorMessage("The role could not be saved.", false);
            return View("Edit", model);
        }

        [Route("/manager/role/delete")]
  
        public IActionResult Delete(Guid id)
        {
            var role = _roleManager.FindByIdAsync(id.ToString()).Result;
            if (role != null)
            {
                _roleManager.DeleteAsync(role);

                //SuccessMessage("The role has been deleted.");
                return RedirectToAction("List");
            }

            //ErrorMessage("The role could not be deleted.", false);
            return RedirectToAction("List");
        }
    }
}