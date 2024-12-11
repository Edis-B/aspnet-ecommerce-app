using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService profileService;
        private readonly IUserService userService;

        public ProfileController(IProfileService _profileService,
            IUserService _userService)
        {
            profileService = _profileService;
            userService = _userService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await profileService.GetUserProfilePictureUrlAsync();

            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProfilePicture(string profilePictureUrl)
        {
            var userId = userService.GetUserId();

            var result = await profileService.UpdateUserProfilePicture(profilePictureUrl);

            return RedirectToAction("Index", "Account");
        }
    }
}
