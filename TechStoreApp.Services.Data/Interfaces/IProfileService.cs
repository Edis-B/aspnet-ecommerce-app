using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileViewModel> GetUserProfilePictureUrlAsync();

        Task<ManageUsersViewModel> GetAllUsersAsync(string? userName, string? email, int page, int itemsPerPage);
    }
}
