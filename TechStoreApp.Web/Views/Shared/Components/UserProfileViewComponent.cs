using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Web.ViewModels.Header;

namespace TechStoreApp.Web.Views.Product
{
    public class UserProfileViewComponent : ViewComponent
    {
        private readonly TechStoreDbContext _context;

        public UserProfileViewComponent(TechStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var userModel = new UserModel() 
            { 
                User = user
            };

            return View("UserProfile", userModel);
        }

    }
}
