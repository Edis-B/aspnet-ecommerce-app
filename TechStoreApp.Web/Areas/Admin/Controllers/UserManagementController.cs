using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;
using static TechStoreApp.Common.GeneralConstraints;

namespace TechStoreApp.Web.Areas.Admin.Controllers
{
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class UserManagementController : Controller
    {
        private readonly IUserService userService;
        private readonly IProfileService profileService;
        public UserManagementController(IUserService _userService,
            IProfileService _profileService)
        {
            userService = _userService;
            profileService = _profileService;
        }

        [HttpGet]
        public async Task<IActionResult> Manage(string? userName, string? email, int page = 1, int itemsPerPage = 12)
        {
            var model = await profileService.GetAllUsersAsync(userName, email, page, itemsPerPage);

            return View("Manage", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result  = await profileService.DeleteUserAsync(userId);

            if (!result.Errors.Any()) { 
                return RedirectToAction("Manage"); 
            }
            
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            var result = await profileService.RemoveRoleRemoveRoleAsync(userId, role);

            if (!result.Errors.Any()) {
                return RedirectToAction("Manage"); 
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var result = await profileService.AssignRoleAssignRoleAsync(userId, role);

            if (!result.Errors.Any()) {
                return RedirectToAction("Manage"); 
            }

            return View("Error");
        }
    }
}
