using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ecommerce.Data.Entities;
using Ecommerce.Identity.Areas.Identity.Email;
using Ecommerce.Identity.Areas.Identity.Helpers;
using Ecommerce.Identity.Areas.Identity.ViewModels;
using Ecommerce.WebApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ecommerce.Identity.Areas.Login
{

    [Area("Login")]

    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private UserManager<AppUser> _usermanager { get; }
        private SignInManager<AppUser> _signinmanager { get; }
        private readonly ILogger<AccountController> _logger;
        private readonly Appsettings _appsettings;
        private IEmailSender _emailSender;
        public AccountController(UserManager<AppUser> usermanager, SignInManager<AppUser> signinmanager, ILogger<AccountController> logger, IEmailSender emailSender, IOptions<Appsettings> appsettings)
        {
            _usermanager = usermanager;
            _signinmanager = signinmanager;
            _logger = logger;
            _emailSender = emailSender;
            _appsettings = appsettings.Value;
        }
      
        [HttpGet]
        public IActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var res = await _usermanager.CreateAsync(user, model.Password);
                if (res.Succeeded)
                {
                 
                    await _usermanager.AddToRoleAsync(user, "Member");
                    var token = await _usermanager.GenerateEmailConfirmationTokenAsync(user);
                    
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);
                    await _emailSender.SendEmailAsync(user.Email, "confirm your email", "Please confirm your email by click this link:<a href=\"" + confirmationLink+"\">click here</a>");
                    //_logger.Log(LogLevel.Warning, confirmationLink);
                    ViewBag.ErrorTitle = "Registration successful";
                    ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                        "email, by clicking on the confirmation link we have emailed you";
                    return View("ExternalLoginResponse");
                    //await _signinmanager.SignInAsync(user, isPersistent: false);
                    //return RedirectToAction(nameof(HomeController.Index), "Home");

                    if (_signinmanager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    ViewBag.ErrorTitle = "Registration successful";
                    ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                            "email, by clicking on the confirmation link we have emailed you";
                    return View("Error");
                    foreach (var error in res.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email have already registered");
                    return View(model);
                }

            }
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _usermanager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    model.ExternalLogins = (await _signinmanager.GetExternalAuthenticationSchemesAsync()).ToList();
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View("Login", model);
                }
                else if(user.EmailConfirmed)
                {
                    var res = await _signinmanager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (res.Succeeded)
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                
            }
            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
    
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });

            var properties = _signinmanager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }
        [HttpGet]
        public async Task<IActionResult> Login(string returnurl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnurl,
                ExternalLogins = (await _signinmanager.GetExternalAuthenticationSchemesAsync()).ToList()
            };


            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                (await _signinmanager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty,
                    $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            var info = await _signinmanager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty,
                    "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            AppUser user = null;

            if (email != null)
            {
                user = await _usermanager.FindByEmailAsync(email);

                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View("Login", loginViewModel);
                }
            }

            var signInResult = await _signinmanager.ExternalLoginSignInAsync(
                                        info.LoginProvider, info.ProviderKey,
                                        isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                if (email != null)
                {
                    if (user == null)
                    {
                        user = new AppUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await _usermanager.CreateAsync(user);

                        // After a local user account is created, generate and log the
                        // email confirmation link
                        var token = await _usermanager.GenerateEmailConfirmationTokenAsync(user);

                        var confirmationLink = Url.Action("ConfirmEmail", "Account",
                                        new { userId = user.Id, token = token }, Request.Scheme);
                        await _emailSender.SendEmailAsync(user.Email, "confirm your email", "Please confirm your email by click this link:<a href=\"" + confirmationLink + "\">click here</a>");
                        _logger.Log(LogLevel.Warning, confirmationLink);

                        string gmaillink = "https://mail.google.com/mail/u/0/?tab=rm&ogbl#inbox";
                        ViewBag.ErrorTitle = "Registration successful";
                        ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                            "email, by clicking on the confirmation link we have emailed you";
                        return View("ExternalLoginResponse");
                    }

                    await _usermanager.AddLoginAsync(user, info);
                    await _signinmanager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on Pragim@PragimTech.com";

                return View("Error");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await _usermanager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("NotFound");
            }

            var result = await _usermanager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signinmanager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}