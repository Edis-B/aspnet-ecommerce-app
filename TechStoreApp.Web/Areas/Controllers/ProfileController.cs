using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Web.Areas.Controllers
{
    public class ProfileController : Controller
    {
        private readonly TechStoreDbContext context;
        public ProfileController(TechStoreDbContext _context)
        {
            this.context = _context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await context.Users
                .Where(u => u.Id == userId)
                .Select(u => new ProfileViewModel()
                {
                    ProfilePictureUrl = u.ProfilePictureUrl,
                    
                })
                .FirstOrDefaultAsync();

            return View("Index", user);
        }
    }
}
