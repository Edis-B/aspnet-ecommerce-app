using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.User;
using static TechStoreApp.Common.GeneralConstraints;

namespace TechStoreApp.Services.Data
{
    public class UserService : IUserService
    {
        private readonly HttpContext httpContext;
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
            httpContext = _httpContextAccessor.HttpContext!;
            urlHelperFactory = _urlHelperFactory;

            signInManager = _signInManager;
            userManager = _userManager;
            userRepository = _userRepository;
            emailSender = _emailSender;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();

            if (httpContext.Request.Cookies.Any())
            {
                httpContext.Response.Cookies.Delete(MyCookieName);
                httpContext.Response.Cookies.Delete(IsAdminAttr);
            }
            
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> SignInAsync(LoginViewModel model)
        {
            bool rememberMe = model.RememberMe;
            var userName = model.UserName;
            var password = model.Password;
            bool shouldLockout = false;

            var signInResult = await signInManager.PasswordSignInAsync(
                userName,
                password,
                rememberMe,
                shouldLockout);

            if (!signInResult.Succeeded) { return signInResult!; }

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                // Set expiration date of cookie based on remember me option
                ExpiresUtc = rememberMe
                    ? DateTime.UtcNow.AddDays(CookieDurationRememberMe) 
                    : (DateTime?)null,
            };

            var user = await signInManager.UserManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                return default!;
            }

            // Sign the user in with the custom properties
            await signInManager.SignInAsync(user, authProperties);

            return signInResult;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = CreateUser();

            if (userManager.Users.Any(u => u.UserName == model.UserName))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Username is already taken."
                });
            }

            if (userManager.Users.Any(u => u.Email == model.Email))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Email is already registered."
                });
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.ProfilePictureUrl = model.ProfilePictureUrl ?? "https://i.pinimg.com/originals/c0/27/be/c027bec07c2dc08b9df60921dfd539bd.webp";

            var result = await userManager.CreateAsync(user, model.Password);

            await userManager.AddToRoleAsync(user, "User");

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

            var user = await userManager.FindByIdAsync(userId.ToString());

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

            var actionContext = new ActionContext(httpContext,
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
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>.");

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

        public Guid GetUserId()
        {
            string userId = httpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            return userId == null ? default : Guid.Parse(userId);
        }

        public async Task<ApplicationUser> GetUserByTheirIdAsync(Guid Id)
        {
            return await userRepository
                .GetByIdAsync(Id)!;
        }

        public async Task<bool> IsUserAdmin(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            return await userManager.IsInRoleAsync(user!, AdminRoleName);
        }

        public async Task<bool> DoesUserExistId(string userId)
        {
            var canParse = Guid.TryParse(userId, out  Guid id);

            if (!canParse) return false;

            var user = await userManager.FindByIdAsync(userId);

            return user != null;
        }
    }
}
