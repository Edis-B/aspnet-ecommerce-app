using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Services.Data
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly TechStoreDbContext context;

        public UserService(IHttpContextAccessor _httpContextAccessor, TechStoreDbContext _context)
        {
            this.httpContextAccessor = _httpContextAccessor;
            context = _context;
        }

        public string GetUserId()
        {
            return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }
        public async Task<ApplicationUser> GetUserByTheirId()
        {
            var userId = GetUserId();

            return await context.Users.FindAsync(userId);
        }
    }
}
