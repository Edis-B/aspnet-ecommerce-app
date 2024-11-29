using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Header;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Web.Views.Shared.Components
{
    public class UserProfileViewComponent : ViewComponent
    {
        private readonly IProfileService profileService;

        public UserProfileViewComponent(IProfileService _profileService)
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
