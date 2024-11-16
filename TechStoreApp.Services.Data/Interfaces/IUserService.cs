using Microsoft.AspNetCore.Identity;
using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IUserService
    {
        string GetUserId();
        Task LogoutAsync();
        Task<SignInResult> SignInAsync(LoginViewModel model);
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<IdentityResult> ConfirmEmailAsync(ConfirmEmailViewModel model);
        Task<IdentityResult> ForgotPasswordAsync(ForgotPasswordViewModel model);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<ApplicationUser> GetUserByTheirIdAsync(string Id);
        ApplicationUser CreateUser();
    }
}
