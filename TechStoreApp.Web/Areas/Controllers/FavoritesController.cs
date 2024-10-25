using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Data.Models.Models;
using TechStoreApp.Web.ViewModels.Favorites;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Web.Areas.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly TechStoreDbContext context;
        public FavoritesController(TechStoreDbContext _context)
        {
            context = _context;
        }
        [HttpPost]
        public async Task<IActionResult> AddToFavorites([FromBody] FavoriteFormModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var newFavoriteProduct = new Favorited {
                ProductId = model.ProductId,
                UserId = userId,
                FavoritedAt = DateTime.Now
            };

            await context.Favorited.AddAsync(newFavoriteProduct);
            await context.SaveChangesAsync();

            return Json(new { message = "Successfully added to favorites" });
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
