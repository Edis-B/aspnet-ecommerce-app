using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechStoreApp.Data;

namespace TechStoreApp.Web.Areas.Controllers
{
    public class ProfileController : Controller
    {
        private readonly TechStoreDbContext _context;
        public ProfileController(TechStoreDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Orders)
                .FirstOrDefaultAsync();

            return View(user);
        }
    }
}
