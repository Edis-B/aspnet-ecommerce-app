using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Web.Areas.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService userService;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService _userService)
        {
            userService = _userService;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await userService.LogoutAsync();

            return RedirectToAction("Index", "Home");
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var signInStatus = await userService.SignInAsync(model);

            if (signInStatus.Succeeded)
            {
                if (returnUrl != null)
                {
                    return LocalRedirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            var user = CreateUser();
            ViewData["PreviousUrl"] = returnUrl;

            user.UserName = model.UserName;
            if (model.ProfilePictureUrl == null)
            {
                user.ProfilePictureUrl = "https://i.pinimg.com/originals/c0/27/be/c027bec07c2dc08b9df60921dfd539bd.webp";
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return View(model);
            }

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        private ApplicationUser CreateUser()
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
    }
}
