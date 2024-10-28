using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Services.Data
{
    public class ProfileService : IProfileService
    {

        private readonly TechStoreDbContext context;
        private readonly IUserService userService;
        public ProfileService(TechStoreDbContext _context, IUserService _userService)
        {
            context = _context;
            userService = _userService;

        }
        public async Task<ProfileViewModel> GetUserProfilePictureUrlAsync()
        {
            var userId = userService.GetUserId();

            var model = await context.Users
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
