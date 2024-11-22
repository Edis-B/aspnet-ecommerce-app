using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Services.Data
{
    public class ProfileService : IProfileService
    {

        private readonly IRepository<ApplicationUser, Guid> userRepository;
        private readonly IUserService userService;
        public ProfileService(IRepository<ApplicationUser, Guid> _userRepository,
            IUserService _userService)
        {
            userRepository = _userRepository;
            userService = _userService;
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

            var models = await users
                .Select(u => new UsersDetailsViewModel()
                {
                    UserId = u.Id.ToString(),
                    Email = u.Email!,
                    UserName = u.UserName!,
                    ProfilePictureUrl = u.ProfilePictureUrl!
                })
                .ToListAsync();

            int totalPages = (int)Math.Ceiling((double)models.Count() / itemsPerPage);

            models = models
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            var newModel = new ManageUsersViewModel()
            {
                ItemsPerPage = itemsPerPage,
                TotalPages = totalPages,
                Page = page,
                Users = models,
                EmailQuery = email,
                UserNameQuery = userName
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

    }
}
