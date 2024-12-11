using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.ApiViewModels.Users;
using TechStoreApp.Web.ViewModels.User;
using static TechStoreApp.Common.GeneralConstraints;

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

        public async Task<IdentityResult> RemoveFromRoleAsync(string userId, string role)
        {
            var result = new IdentityResult();
            var currUserId = userService.GetUserId();

            if (Guid.Parse(userId) == currUserId && role == AdminRoleName)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "Invalid Role Removal",
                    Description = "Cannot remove your own Admin role"
                });
            }

            var user = await userService.GetUserByTheirIdAsync(Guid.Parse(userId));
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

        public async Task<IdentityResult> AssignRoleAsync(string userId, string role)
        {
            var user = await userService.GetUserByTheirIdAsync(Guid.Parse(userId));
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
            var user = await userService.GetUserByTheirIdAsync(Guid.Parse(userId));
            
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "Invalid UserId",
                    Description = "The provided user ID is not valid."
                });
            }


            if (userService.GetUserId() == Guid.Parse(userId))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "Delete Error",
                    Description = "Cannot delete your own account."
                });
            }

            return await userManager.DeleteAsync(user);
        }

        public async Task<ManageUsersViewModel> GetAllUsersPageAsync(string? userName, string? email, int page, int itemsPerPage)
        {
            var users = await userRepository
                .GetAllAttached()
                .ToListAsync();

            if (!string.IsNullOrEmpty(email))
            {
                users = users
                    .Where(u => u.Email!.ToLower().Contains(email.ToLower()))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(userName))
            {
                users = users
                    .Where(u => u.UserName!.ToLower().Contains(userName.ToLower()))
                    .ToList();
            }

            var usersModels = new List<UsersDetailsViewModel>();

            var allRoles = roleManager
                .Roles
                .Select(r => r.ToString()).ToList();

            foreach (var u in users)
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

        public async Task<IEnumerable<UserDetailsApiViewModel>> ApiGetAllUsersAsync()
        {
            var users = userRepository.GetAll()
                .ToList();

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

            var model = await userRepository
                .GetByIdAsync(userId);

            return new PfpViewModel()
            {
                ProfilePictureUrl = model.ProfilePictureUrl ?? "Profile picture error",
            } ?? default!;
        }

        public async Task<UserDetailsApiViewModel> ApiGetUserByTheirIdAsync(string userId)
        {
            var user = await userService.GetUserByTheirIdAsync(Guid.Parse(userId));

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

        public async Task<UserViewModel> GetUserViewModel(Guid userId)
        {
            var user = await userService.GetUserByTheirIdAsync(userId); 
            
            if (user == null) 
            {
                return default!;
            }

            return new UserViewModel()
            {
                UserId = user.Id.ToString(),
                Name = user.UserName!,
                Email = user.Email!,
                PictureUrl = user.ProfilePictureUrl!,
                Roles = (await userManager.GetRolesAsync(user)).ToList()
            };
        }

        public async Task<bool> UpdateUserProfilePicture(string profilePictureUrl)
        {
            var userId = userService.GetUserId();

            var user = await userService.GetUserByTheirIdAsync(userId);

            user.ProfilePictureUrl = profilePictureUrl;

            await userRepository.UpdateAsync(user);
            return true;
        }
    }
}
