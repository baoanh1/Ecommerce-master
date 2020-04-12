using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.netcore.Areas.Identity.Models;
using eCommerce.netcore.Areas.Identity.ViewModels;
using eCommerce.netcore.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.netcore.Areas.Identity.Controllers
{
    [Area("Identity")]

    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private UserManager<AppUser> _usermanager {get; }
        private SignInManager<AppUser> _signinmanager { get; } 
        public AccountController(UserManager<AppUser> usermanager, SignInManager<AppUser> signinmanager)
        {
            _usermanager = usermanager;
            _signinmanager = signinmanager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var res = await _usermanager.CreateAsync(user, model.Password);
                if(res.Succeeded)
                {
                    await _usermanager.AddToRoleAsync(user, "Member");
                    await _signinmanager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }

            }
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var res = await _signinmanager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if(res.Succeeded)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signinmanager.SignOutAsync();
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}