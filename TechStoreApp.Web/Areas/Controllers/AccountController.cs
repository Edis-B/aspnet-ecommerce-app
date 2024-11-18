using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System.Text;
using System.Text.Encodings.Web;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.User;
using static TechStoreApp.Common.EntityValidationConstraints;

namespace TechStoreApp.Web.Areas.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        public AccountController(IUserService _userService)
        {
            userService = _userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await userService.LogoutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string ReturnUrl)
        {
            var model = new LoginViewModel { ReturnUrl = ReturnUrl };

            return View("Login", model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var signInStatus = await userService.SignInAsync(model);

            if (!signInStatus.Succeeded)
            {
                return View("Login", model);
            }

            if (model.ReturnUrl != null)
            {
                return LocalRedirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Register(string returnUrl)
        {
            return View("Register");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }

            var result = await userService.RegisterAsync(model);

            if (!result.Succeeded)
            {
                return View("Register", model);
            }

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmEmail(string code, string userId)
        {
            var model = new ConfirmEmailViewModel()
            {
                Code = code
            };

            var result = await userService.ConfirmEmailAsync(model);

            if (result.Succeeded)
            {
                var emailModel = new SentEmailViewModel()
                {
                    Message = "An email has been sent to your inbox."
                };

                return View("SentEmailViewModel", emailModel);
            } 
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            var model = new ForgotPasswordViewModel();

            return View("ForgotPassword", model);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ForgotPassword");
            }

            var result = await userService.ForgotPasswordAsync(model);

            if (!result.Succeeded)
            {
                string err = result.Errors
                    .FirstOrDefault()!
                    .Description;

                return BadRequest(err);
            }

            return RedirectToPage("./ForgotPasswordConfirmation");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            
            var model = new ResetPasswordViewModel()
            {
                Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
            };

            return View("ResetPassword", model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ResetPassword", model);
            } 

            var result = await userService.ResetPasswordAsync(model);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            else
            {
                return View("PasswordChanged");
            }
        }

    }
}
