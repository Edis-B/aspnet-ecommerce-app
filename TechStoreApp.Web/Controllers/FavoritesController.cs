using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Favorites;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Web.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService favoriteService;
        private readonly IRequestService requestService;
        public FavoritesController(IFavoriteService _favoriteService,
            IRequestService _requestService)
        {
            favoriteService = _favoriteService;
            requestService = _requestService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(ProductIdFormModel model)
        {
            if (requestService.IsAjaxRequest(Request))
            {
                model = await requestService.ExtractModelFromRequestBody<ProductIdFormModel>(Request);
                var response = await favoriteService.AddToFavoritesAsync(model);

                return response;
            }
            else
            {
                var response = await favoriteService.AddToFavoritesAsync(model);

                return RedirectToAction("Favorites", "Favorites");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(ProductIdFormModel model)
        {
            if (requestService.IsAjaxRequest(Request))
            {
                model = await requestService.ExtractModelFromRequestBody<ProductIdFormModel>(Request);

                var response = await favoriteService.RemoveFromFavoritesAsync(model);
                return response;
            }
            else
            {
                var response = await favoriteService.RemoveFromFavoritesAsync(model);

                return RedirectToAction("Favorites", "Favorites");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            var model = await favoriteService.GetUserFavoritesAsync();

            return View(model);
        }
    }
}
