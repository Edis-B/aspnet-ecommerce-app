using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Header;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Web.Areas.Admin.Views.Shared.Components
{
    public class UserProfileAdminViewComponent : ViewComponent
    {
        private readonly IUserService userService;
        private readonly IProfileService profileService;

        public UserProfileAdminViewComponent(IUserService _userService,
            IProfileService _profileService)
        {
            userService = _userService;
            profileService = _profileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await profileService.GetUserProfilePictureUrlAsync();

            return View("UserProfileAdmin", model);
        }

    }
}
