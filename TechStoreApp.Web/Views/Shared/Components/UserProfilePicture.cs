using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Web.Views.Shared.Components
{
    [ViewComponent]
    public class UserProfilePicture : ViewComponent
    {
        private readonly IProfileService profileService;

        public UserProfilePicture(IProfileService _profileService)
        {
            profileService = _profileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model  = await profileService.GetUserProfilePictureUrlAsync();

            return View("UserProfilePicture", model);
        }
    }
}
