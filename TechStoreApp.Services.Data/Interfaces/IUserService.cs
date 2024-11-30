using Microsoft.AspNetCore.Identity;
using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IUserService
    {
        Task LogoutAsync();
        Task<SignInResult> SignInAsync(LoginViewModel model);
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<IdentityResult> ConfirmEmailAsync(ConfirmEmailViewModel model);
        Task<IdentityResult> ForgotPasswordAsync(ForgotPasswordViewModel model);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<ApplicationUser> GetUserByTheirIdAsync(Guid Id);
        Task<bool> IsUserAdmin(Guid userId);
        Task<bool> DoesUserExistId(string userId);
        ApplicationUser CreateUser();
        Guid GetUserId();
    }
}
