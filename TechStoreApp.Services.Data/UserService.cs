using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Services.Data
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUrlHelperFactory urlHelperFactory;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender<ApplicationUser> emailSender;
        private readonly IRepository<ApplicationUser, Guid> userRepository;

        public UserService(IHttpContextAccessor _httpContextAccessor,
                           IUrlHelperFactory _urlHelperFactory,

                           SignInManager<ApplicationUser> _signInManager,
                           UserManager<ApplicationUser> _userManager,
                           IRepository<ApplicationUser, Guid> _userRepository,
                           IEmailSender<ApplicationUser> _emailSender)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            urlHelperFactory = _urlHelperFactory;
            userRepository = _userRepository;
            emailSender = _emailSender;
            httpContextAccessor = _httpContextAccessor;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }
        public async Task<Microsoft.AspNetCore.Identity.SignInResult> SignInAsync(LoginViewModel model)
        {
            bool rememberMe = model.RememberMe;
            bool shouldLockout = false;
            var userName = model.UserName;
            var password = model.Password;

            return await signInManager.PasswordSignInAsync(
                userName, 
                password,
                rememberMe,
                shouldLockout);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = CreateUser();
            await userManager.AddToRoleAsync(user, "User");

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.ProfilePictureUrl = model.ProfilePictureUrl;

            if (model.ProfilePictureUrl == null)
            {
                user.ProfilePictureUrl = "https://i.pinimg.com/originals/c0/27/be/c027bec07c2dc08b9df60921dfd539bd.webp";
            }

            var result = await userManager.CreateAsync(user, model.Password);

            return result;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(ConfirmEmailViewModel model)
        {
            // Autherized
            var userId = GetUserId();
            var code = model.Code;

            if (code == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Code is required." });
            }

            var user = await userManager.FindByIdAsync(userId);

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ConfirmEmailAsync(user!, code);

            return result;
        }

        public async Task<IdentityResult> ForgotPasswordAsync(ForgotPasswordViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return IdentityResult.Success;
            }

            if (await userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user is confirmed
                return IdentityResult.Success;
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

            var actionContext = new ActionContext(httpContextAccessor.HttpContext,
                new RouteData(), 
                new ActionDescriptor());

            var urlHelper = urlHelperFactory.GetUrlHelper(actionContext);

            var callbackUrl = urlHelper.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code = resetToken },
                protocol: "https");

            await emailSender.SendConfirmationLinkAsync(
                user,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return IdentityResult.Success;

        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var resetToken = model.Code;

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return IdentityResult.Success;
            }

            var result = await userManager.ResetPasswordAsync(user, resetToken, model.Password);
            return IdentityResult.Success;
        }

        public ApplicationUser CreateUser()
        {
            return Activator.CreateInstance<ApplicationUser>();
        }

        public string GetUserId()
        {
            return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        public async Task<ApplicationUser> GetUserByTheirIdAsync(string Id)
        {
            return await userRepository
                .GetByIdAsync(Guid.Parse(Id))!;
        }
    }
}
