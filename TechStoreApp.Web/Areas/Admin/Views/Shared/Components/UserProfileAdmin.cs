using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Web.Areas.Admin.Views.Shared.Components
{
    [ViewComponent]
    public class UserProfileAdmin : ViewComponent
    {
        private readonly IUserService userService;
        private readonly IProfileService profileService;

        public UserProfileAdmin(IUserService _userService,
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
