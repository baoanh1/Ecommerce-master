using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Data.Entities;
using Ecommerce.Identity.Areas.Identity.ViewModels;
using Ecommerce.WebApp.Areas;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Identity.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class UserController : MessageController
    {
        private RoleManager<AppRole> _roleManager;
        private UserManager<AppUser> _userManager;
        IdentityResult res;
        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Route("manager/users")]
        public IActionResult List()
        {
            
            return View();
        }
        [Route("manager/users/list")]
        public async Task<UserListViewModel> Get()
        {
            var users = _userManager.Users;
            List<userViewModel> listUsers = new List<userViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var model = new userViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    userRoles = roles,
                };
                listUsers.Add(model);
            }
            UserListViewModel userlist = new UserListViewModel();
            userlist.Users = listUsers;
            return userlist;
        }
        [Route("/manager/user/{id:Guid?}")]
        
        public IActionResult Edit(Guid id)
        {
            return View(id);
            //return View(UserEditModel.GetById(_db, id));
        }

        /// <summary>
        /// Gets the edit view for an existing user.
        /// </summary>
        /// <param name="id">The user id</param>
        [Route("/manager/user/edit/{id:Guid}")]
       
        public UserEditViewModel Get(Guid id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            var allroles = _roleManager.Roles.ToList();
            var roles = _userManager.GetRolesAsync(user).Result;
            var model = new UserEditViewModel
            {
                User = user,
                Roles = allroles,
                SelectedRoles = roles
            };

            return model;
        }
        [Route("/manager/user/add")]
        public UserEditViewModel Add()
        {
            var newuser= new UserEditViewModel
            {
                User = new AppUser(),
                Roles = _roleManager.Roles.ToList()
            };
            return newuser;
        }
        /// <summary>
        /// Deletes the user with the given id.
        /// </summary>
        /// <param name="id">The user id</param>
        [Route("/manager/user/delete/{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser != null && user.Id == currentUser.Id)
            {
                return BadRequest("Can't delete yourself.");
            }

            if (user != null)
            {
                var res = await _userManager.DeleteAsync(user);

                var str = Ok(GetSuccessMessage("The user has been deleted."));
                return str;
                //return Ok(Get(user.Id));
            }

            return Ok(GetErrorMessage("The user could not be found."));
        }
        [HttpPost]
        [Route("/manager/user/save")]

        public async Task<IActionResult> Save([FromBody] UserEditViewModel model)
        {
            // Refresh roles in the model if validation fails
            //var temp = UserEditModel.Create(_db);
            //model.Roles = temp.Roles;

            if (model.User == null)
            {
                return BadRequest("The user could not be found.");
            }
            
            try
            {
                var userId = model.User.Id;
                var isNew = userId == Guid.Empty;

                if (string.IsNullOrWhiteSpace(model.User.UserName))
                {
                    return BadRequest("Username is mandatory.");
                }

                if (string.IsNullOrWhiteSpace(model.User.Email))
                {
                    return BadRequest("Email address is mandatory.");
                }

                if (!string.IsNullOrWhiteSpace(model.Password) && model.Password != model.PasswordConfirm)
                {
                    return BadRequest(string.Format("{0} {1} - {2}", "The new passwords does not match.", model.Password, model.PasswordConfirm));
                }

                if (model.User.Id == Guid.Empty && string.IsNullOrWhiteSpace(model.Password))
                {
                    return BadRequest("Password is mandatory when creating a new user.");
                }



                if (!string.IsNullOrWhiteSpace(model.Password) && _userManager.PasswordValidators.Count > 0)
                {
                    var errors = new List<string>();
                    foreach (var validator in _userManager.PasswordValidators)
                    {
                        var errorResult = await validator.ValidateAsync(_userManager, model.User, model.Password);
                        if (!errorResult.Succeeded)
                            errors.AddRange(errorResult.Errors.Select(msg => msg.Description));
                        if (errors.Count > 0)
                        {
                            return BadRequest(string.Join("<br />", errors));
                        }
                    }
                }

                //check username
                //if (await _userManager.CountAsync(u => u.UserName.ToLower().Trim() == model.User.UserName.ToLower().Trim() && u.Id != userId) > 0)
                //{
                //    return BadRequest(GetErrorMessage(_localizer.Security["Username is used by another user."]));
                //}

                ////check email
                //if (await _db.Users.CountAsync(u => u.Email.ToLower().Trim() == model.User.Email.ToLower().Trim() && u.Id != userId) > 0)
                //{
                //    return BadRequest(GetErrorMessage(_localizer.Security["Email address is used by another user."]));
                //}

                res = await model.Save(_userManager);
                var result = res;
                if (result.Succeeded)
                {
                    return Ok(Get(model.User.Id));

                }

                var errorMessages = new List<string>();
                errorMessages.AddRange(result.Errors.Select(msg => msg.Description));

                return BadRequest("The user could not be saved." + "<br/><br/>" + string.Join("<br />", errorMessages));
            }
             
            catch (Exception ex)
            {
                IdentityResult temp = res;
                return BadRequest(ex.Message);
            }
            
        }

    }
}