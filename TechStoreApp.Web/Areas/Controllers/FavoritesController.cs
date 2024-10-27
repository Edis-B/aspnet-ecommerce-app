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
        private readonly TechStoreDbContext context;
        private readonly IFavoriteService favoriteService;
        public FavoritesController(TechStoreDbContext _context, IFavoriteService _favoriteService)
        {
            favoriteService = _favoriteService;
            context = _context;
        }
        [HttpPost]
        public async Task<JsonResult> AddToFavorites([FromBody] FavoriteFormModel model)
        {
            return await favoriteService.AddToFavoritesAsync(model);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFromFavorites([FromBody] FavoriteFormModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var toBeRemoved = await context.Favorited
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == model.ProductId);

            context.Favorited.Remove(toBeRemoved);

            await context.SaveChangesAsync();

            return Json(new { message = "Successfully removed from favorites" });
        }
        [HttpGet]
        public async Task<IActionResult> Favorites(FavoriteViewModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var products = await context.Favorited
                .Where(f => f.UserId == userId)
                .Select(f => new ProductViewModel
                {
                    ProductId = f.ProductId,
                    Name = f.Product.Name,
                    ImageUrl = f.Product.ImageUrl,
                    DateLiked = f.FavoritedAt.ToString("dd/MM/yyyy"),
                })
                .ToListAsync();

            model = new FavoriteViewModel {
                Products = products
            };

            return View(model);
        }
    }
}
