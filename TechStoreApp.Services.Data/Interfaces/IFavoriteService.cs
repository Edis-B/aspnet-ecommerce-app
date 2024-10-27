using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Web.ViewModels.Favorites;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IFavoriteService
    {
        Task<JsonResult> AddToFavoritesAsync(FavoriteFormModel model);
        Task<JsonResult> RemoveFromFavoritesAsync(FavoriteFormModel model);
        Task<FavoriteViewModel> GetUserFavoritesAsync();
    }
}
