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
