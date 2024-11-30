using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Web.ViewModels.ApiViewModels.Users;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IProfileService
    {
        Task<ManageUsersViewModel> GetAllUsersPageAsync(string? userName, string? email, int page, int itemsPerPage);
        Task<IdentityResult> AssignRoleAssignRoleAsync(string userId, string role);
        Task<IdentityResult> RemoveRoleRemoveRoleAsync(string userId, string role);
        Task<IdentityResult> DeleteUserAsync(string userId);
        Task<PfpViewModel> GetUserProfilePictureUrlAsync();
        // Api
        Task<IEnumerable<UserDetailsApiViewModel>> GetAllUsersAsync();
    }
}
