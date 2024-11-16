using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Web.Areas.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService profileService;

        public ProfileController(IProfileService _profileService)
        {
            profileService = _profileService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await  profileService.GetUserProfilePictureUrlAsync();

            return View("Index", model);
        }
    }
}
