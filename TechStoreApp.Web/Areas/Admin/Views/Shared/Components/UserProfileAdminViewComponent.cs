using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Web.ViewModels.Header;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Web.Areas.Admin.Views.Shared.Components
{
    public class UserProfileAdminViewComponent : ViewComponent
    {
        private readonly TechStoreDbContext _context;

        public UserProfileAdminViewComponent(TechStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new ProfileViewModel()
                {
                    ProfilePictureUrl = u.ProfilePictureUrl!
                })
                .FirstOrDefaultAsync();

            var userModel = new UserModel()
            {
                User = user
            };

            return View("UserProfileAdmin", userModel);
        }

    }
}
