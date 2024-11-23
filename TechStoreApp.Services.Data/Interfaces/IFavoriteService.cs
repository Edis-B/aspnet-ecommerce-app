using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Web.ViewModels.Favorites;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IFavoriteService
    {
        Task<JsonResult> AddToFavoritesAsync(ProductIdFormModel model);
        Task<JsonResult> RemoveFromFavoritesAsync(ProductIdFormModel model);
        Task<FavoriteViewModel> GetUserFavoritesAsync();
    }
}
