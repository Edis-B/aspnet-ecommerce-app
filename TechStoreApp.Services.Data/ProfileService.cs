using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.ApiViewModels.Users;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Services.Data
{
    public class ProfileService : IProfileService
    {

        private readonly IRepository<ApplicationUser, Guid> userRepository;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        public ProfileService(IRepository<ApplicationUser, Guid> _userRepository,
            UserManager<ApplicationUser> _userManager,
            RoleManager<ApplicationRole> _roleManager,
            IUserService _userService)
        {
            userRepository = _userRepository;
            roleManager = _roleManager;
            userManager = _userManager;
            userService = _userService;
        }

        public async Task<IdentityResult> RemoveRoleRemoveRoleAsync(string userId, string role)
        {
            var user = await GetUserFromIdAsync(Guid.Parse(userId));
            if (user == null)
            {
                return IdentityResult.Failed();
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                return IdentityResult.Failed();
            }

            return await userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IdentityResult> AssignRoleAssignRoleAsync(string userId, string role)
        {
            var user = await GetUserFromIdAsync(Guid.Parse(userId));
            if (user == null)
            {
                return IdentityResult.Failed();
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                return IdentityResult.Failed();
            }

            return await userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await GetUserFromIdAsync(Guid.Parse(userId));
            
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidUserId",
                    Description = "The provided user ID is not valid."
                });
            }


            if (userService.GetUserId() == Guid.Parse(userId))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "The user does not exist."
                });
            }

            return await userManager.DeleteAsync(user);
        }

        public async Task<ManageUsersViewModel> GetAllUsersPageAsync(string? userName, string? email, int page, int itemsPerPage)
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

        public async Task<IEnumerable<UserDetailsApiViewModel>> GetAllUsersAsync()
        {
            var users = userRepository.GetAllAttached();

            List<UserDetailsApiViewModel> results = new List<UserDetailsApiViewModel>();

            foreach (var user in users)
            {
                var newUser = new UserDetailsApiViewModel()
                {
                    UserId = user.Id.ToString(),
                    Email = user.Email!,
                    UserName = user.UserName!,
                    ProfilePictureUrl = user.ProfilePictureUrl!,
                    
                };

                var res = await userManager.GetRolesAsync(user);
                newUser.Roles = res.ToList();

                results.Add(newUser);
            }

            return results;
        }

        public async Task<PfpViewModel> GetUserProfilePictureUrlAsync()
        {
            var userId = userService.GetUserId();

            var model = await userRepository.GetAllAttached()
                .Where(u => u.Id == userId)
                .Select(u => new PfpViewModel()
                {
                    ProfilePictureUrl = u.ProfilePictureUrl,
                })
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task<ApplicationUser> GetUserFromIdAsync(Guid id)
        {
            var user = await userRepository
                .GetAllAttached()
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            return user!;
        }

        public async Task<UserDetailsApiViewModel> GetUserByTheirIdAsync(string userId)
        {
            var user = await GetUserFromIdAsync(Guid.Parse(userId));

            if (user == null)
            {
                return default!;
            }

            return new UserDetailsApiViewModel()
            {
                UserId = user.Id.ToString(),
                Email = user.Email!,
                UserName = user.UserName!,
                ProfilePictureUrl = user.ProfilePictureUrl!,
                Roles = (await userManager.GetRolesAsync(user))
                    .ToList()
            };
        }
    }
}
