using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.Products;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Web.Areas.Controllers
{
    public class ProductController : Controller
    {
        private readonly TechStoreDbContext context;
        public ProductController(TechStoreDbContext _context)
        {
            context = _context;
        }

        public async Task<IActionResult> RedirectToDetails(int productId)
        {
            var userId = GetUserId();

            var product = await context.Products
                .Where(p => p.ProductId == productId)
                .Select(p => new ProductViewModel 
                { 
                    ProductId = productId,
                    CategoryId = p.CategoryId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl,
                    CheckedString = p.Favorites.Any(f => f.UserId == userId) ? "checked" : "unchecked"
                })
                .FirstOrDefaultAsync();

            product.Reviews = await context.Reviews
                .Where(r => r.ProductId == productId)
                .Select(r => new ReviewViewModel
                {
                    Comment = r.Comment,
                    ProductId = r.ProductId,
                    Rating = r.Rating,
                    Author = r.User.UserName
                })
                .ToListAsync();

            return View("Product", product);
        }
        [HttpPost]
        public async Task<JsonResult> CreateReview([FromBody] ReviewFormModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var newReview = new Review
            {
                ReviewDate = DateTime.Now,
                Rating = model.Rating,
                ProductId = model.ProductId,
                Comment = model.Comment,
                UserId = userId

            };

            await context.AddAsync(newReview);
            await context.SaveChangesAsync();

            return Json(new { message = "Successfully added" });
        }

        public string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
