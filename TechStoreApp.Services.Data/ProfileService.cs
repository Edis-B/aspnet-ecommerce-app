using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Services.Data
{
    public class ProfileService : IProfileService
    {

        private readonly IRepository<ApplicationUser, Guid> userRepository;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        public ProfileService(IRepository<ApplicationUser, Guid> _userRepository,
            UserManager<ApplicationUser> _userManager,
            RoleManager<IdentityRole> _roleManager,
            IUserService _userService)
        {
            userRepository = _userRepository;
            roleManager = _roleManager;
            userManager = _userManager;
            userService = _userService;
        }

        public async Task<IdentityResult> RemoveRoleRemoveRoleAsync(string userId, string role)
        {
            var user = await GetUserFromIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed();
            }

            return await userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IdentityResult> AssignRoleAssignRoleAsync(string userId, string role)
        {
            var user = await GetUserFromIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed();
            }

            return await userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await GetUserFromIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed();
            }

            return await userManager.DeleteAsync(user);
        }

        public async Task<ManageUsersViewModel> GetAllUsersAsync(string? userName, string? email, int page, int itemsPerPage)
        {
            var users = userRepository.GetAllAttached();

            if (!string.IsNullOrEmpty(email))
            {
                users = users
                    .Where(u => u.Email!.ToLower().Contains(email.ToLower()));
            }

            if (!string.IsNullOrEmpty(userName))
            {
                users = users
                    .Where(u => u.UserName!.ToLower().Contains(userName.ToLower()));
            }

            var usersList = await users.ToListAsync();
            var usersModels = new List<UsersDetailsViewModel>();
            var allRoles = roleManager.Roles.Select(r => r.ToString()).ToList();
            foreach (var u in usersList)
            {
                var roles = await userManager.GetRolesAsync(u);
                var missingRoles = allRoles.Except(roles).ToList();
                usersModels.Add(new UsersDetailsViewModel
                {
                    UserId = u.Id.ToString(),
                    Email = u.Email!,
                    UserName = u.UserName!,
                    ProfilePictureUrl = u.ProfilePictureUrl!,
                    UserRoles = roles.ToList(),
                    MissingRoles = missingRoles,
                });
            }

            int totalPages = (int)Math.Ceiling((double)usersModels.Count() / itemsPerPage);

            usersModels = usersModels
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            var newModel = new ManageUsersViewModel()
            {
                ItemsPerPage = itemsPerPage,
                TotalPages = totalPages,
                Page = page,
                EmailQuery = email,
                UserNameQuery = userName,
                Users = usersModels,
            };

            return newModel;
        }

        public async Task<ProfileViewModel> GetUserProfilePictureUrlAsync()
        {
            var userId = userService.GetUserId();

            var model = await userRepository.GetAllAttached()
                .Where(u => u.Id == userId)
                .Select(u => new ProfileViewModel()
                {
                    ProfilePictureUrl = u.ProfilePictureUrl,
                })
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task<ApplicationUser> GetUserFromIdAsync(string id)
        {
            return await userRepository
                .GetAllAttached()
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

        }
    }
}
