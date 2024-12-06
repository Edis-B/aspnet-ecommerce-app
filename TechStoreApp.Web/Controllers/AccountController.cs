using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.Infrastructure;
using TechStoreApp.Web.ViewModels.User;
using static TechStoreApp.Web.Infrastructure.TempDataUtility;
namespace TechStoreApp.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly ICookieService cookieService;
        public AccountController(IUserService _userService,
            ICookieService _cookieService)
        {
            userService = _userService;
            cookieService = _cookieService;
        }

        [HttpGet]
        public IActionResult Index()
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
        public IActionResult Login(string ReturnUrl)
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
                TempData["ErrorMessage"] = "Invalid username or password.";
                return View("Login", model);
            }

            await cookieService.AttachIsUserAdminToCookie(model.RememberMe);

            if (model.ReturnUrl != null)
            {
                return LocalRedirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
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

            if (result.Errors.Any())
            {
                TempData["ErrorMessages"] = result.Errors.Select(e => e.Description.ToString()).ToList();
                return View("Register", model);
            }

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
        public IActionResult ForgotPassword()
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
        public IActionResult ResetPassword(string? code = null)
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
