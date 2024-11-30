using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Web.Views.Shared.Components
{
    [ViewComponent]
    public class UserProfile : ViewComponent
    {
        private readonly IProfileService profileService;

        public UserProfile(IProfileService _profileService)
        {
            profileService = _profileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model  = await profileService.GetUserProfilePictureUrlAsync();

            return View("UserProfile", model);
        }
    }
}
