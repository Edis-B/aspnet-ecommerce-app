using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Services.Data
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly TechStoreDbContext context;

        public UserService(IHttpContextAccessor _httpContextAccessor, TechStoreDbContext _context,
            SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _userManager)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            httpContextAccessor = _httpContextAccessor;
            context = _context;
        }

        public string GetUserId()
        {
            return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }
        public async Task<ApplicationUser> GetUserByTheirIdAsync()
        {
            var userId = GetUserId();

            return await context.Users.FindAsync(userId);
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<SignInResult> SignInAsync(LoginViewModel model)
        {
            bool rememberMe = model.RememberMe;
            bool shouldLockout = false;
            var userName = model.UserName;
            var password = model.Password;

            return await signInManager.PasswordSignInAsync(
                userName, 
                password,
                rememberMe,
                shouldLockout);
        }
    }
}
