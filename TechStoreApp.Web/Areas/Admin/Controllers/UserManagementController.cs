using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels;
using static TechStoreApp.Common.GeneralConstraints;
using static TechStoreApp.Web.Infrastructure.TempDataUtility;

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
            var model = await profileService.GetAllUsersPageAsync(userName, email, page, itemsPerPage);

            return View("Manage", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result  = await profileService.DeleteUserAsync(userId);

            if (result.Errors.Any()) {
                AppendErrorViewModelToTempData(TempData, nameof(ErrorViewModel), result.Errors.Select(e => e.Description.ToString()), 403);
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            
            return RedirectToAction("Manage"); 
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            var result = await profileService.RemoveFromRoleAsync(userId, role);

            if (result.Errors.Any()) {
                AppendErrorViewModelToTempData(TempData, nameof(ErrorViewModel), result.Errors.Select(e => e.Description.ToString()), 403);
                return RedirectToAction("Error", "Home", new { area = "" });
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var result = await profileService.AssignRoleAsync(userId, role);

            if (!result.Errors.Any()) {
                return RedirectToAction("Manage"); 
            }

            return View("Error");
        }
    }
}
