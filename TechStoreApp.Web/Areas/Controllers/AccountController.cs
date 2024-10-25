using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using TechStoreApp.Data.Models.Models;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Web.Areas.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            bool rememberMe = model.RememberMe;
            bool shouldLockout = false;
            var userName = model.UserName;
            var password = model.Password;

            var signInStatus = await _signInManager.PasswordSignInAsync(
                userName, password, rememberMe, shouldLockout);

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
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
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch (Exception ex) { throw new Exception(); }
        }
    }
}
