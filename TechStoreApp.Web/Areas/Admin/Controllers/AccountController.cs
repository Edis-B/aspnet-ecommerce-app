using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels;
using static TechStoreApp.Common.GeneralConstraints;
namespace TechStoreApp.Web.Areas.Admin.Controllers
{
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IProfileService profileService;
        public AccountController(IUserService _userService,
            IProfileService _profileService)
        {
            userService = _userService;
            profileService = _profileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? userId)
        {
            if (userId == null)
            {
                return View("Error", new ErrorViewModel()
                {
                    Messages = ["User Id is invalid"],
                    StatusCode = 404
                });
            }

            var model = await profileService.GetUserViewModel(Guid.Parse(userId));

            return View("Index", model);
        }
    }
}
