using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Favorites;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Web.Areas.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService favoriteService;
        public FavoritesController(IFavoriteService _favoriteService)
        {
            favoriteService = _favoriteService;
        }
        [HttpPost]
        public async Task<JsonResult> AddToFavorites([FromBody] FavoriteFormModel model)
        {
            return await favoriteService.AddToFavoritesAsync(model);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFromFavorites([FromBody] FavoriteFormModel model)
        {
            return await favoriteService.RemoveFromFavoritesAsync(model);
        }
        [HttpGet]
        public async Task<IActionResult> Favorites(FavoriteViewModel model)
        {
            return View(await favoriteService.GetUserFavoritesAsync());
        }
    }
}
