using System;
using System.Threading.Tasks;

using Core.Data.Dto;
using Core.Services.Business;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Web.Models.AccountViewModel;

namespace Web.Controllers {
    // [Route("[controller]/[action]")]
    public class AccountController: Controller {
        private readonly IAccountBusinessService _accountBusinessService;
        private readonly ILogger _logger;

        public AccountController(IAccountBusinessService accountBusinessService, ILogger<AccountController> logger) {
            _accountBusinessService = accountBusinessService;
            _logger = logger;
        }

        public async Task<IActionResult> Index() {
            //var user = _accountBusinessService.CreateUser(new ApplicationUserDto() {
            //    UserName = "Admin",
            //    NormalizedUserName = "Administrator",
            //    Email = "test@test.com",
            //    EmailConfirmed = true,
            //    PhoneNumber = "3239226969",
            //    PhoneNumberConfirmed = true
            //});

            //     var xxx = await _accountBusinessService.UpdateUserProfile("1111", new UserProfileDto() { });

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null) {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            if(ModelState.IsValid) {
                try {
                    var result = await _accountBusinessService.PasswordSignInAsync(model.Email, model.Password, model.RememberMe);
                    if(result.Succeeded) {
                        _logger.LogInformation("User logged in.");
                        return RedirectToLocal(returnUrl);
                    }
                    if(result.RequiresTwoFactor) {
                        //return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                    }
                    if(result.IsLockedOut) {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToAction(nameof(Lockout));
                    } else {
                        ModelState.AddModelError("All", "Invalid login attempt.");
                        return View(model);
                    }
                } catch(Exception e) {
                    ModelState.AddModelError("All", e.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() {
            await _accountBusinessService.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            if(ModelState.IsValid) {
                var user = new ApplicationUserDto { UserName = model.Email, Email = model.Email };
                var result = await _accountBusinessService.CreateUser(user, model.Password);
                if(result != null) {
                    _logger.LogInformation("User created a new account with password.");
                    var code = await _accountBusinessService.GenerateEmailConfirmationTokenAsync(result);
                    //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await _accountBusinessService.SignInAsync(result, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                // AddErrors(result);
            }

            // If execution got this far, something failed, redisplay the form.
            return View(model);
        }

        #region HELPERS
        //private void AddErrors(IdentityResult result) {
        //    foreach(var error in result.Errors) {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }
        //}

        private IActionResult RedirectToLocal(string returnUrl) {
            if(Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            } else {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        #endregion

    }
}