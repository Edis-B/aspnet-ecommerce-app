using Microsoft.AspNetCore.Identity;
using TechStoreApp.Web.ViewModels.ApiViewModels.Users;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IProfileService
    {
        Task<ManageUsersViewModel> GetAllUsersPageAsync(string? userName, string? email, int page, int itemsPerPage);
        Task<IdentityResult> AssignRoleAsync(string userId, string role);
        Task<IdentityResult> RemoveFromRoleAsync(string userId, string role);
        Task<IdentityResult> DeleteUserAsync(string userId);
        Task<UserViewModel> GetUserViewModel(Guid userId);
        Task<PfpViewModel> GetUserProfilePictureUrlAsync();

        // Api
        Task<IEnumerable<UserDetailsApiViewModel>> ApiGetAllUsersAsync();
        Task<UserDetailsApiViewModel> ApiGetUserByTheirIdAsync(string userId);
    }
}
