using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Products;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Web.Areas.Controllers
{
    public class ProductController : Controller
    {
        private readonly TechStoreDbContext context;
        private readonly IProductService productService;
        public ProductController(TechStoreDbContext _context, IProductService _productService)
        {
            productService = _productService;
            context = _context;
        }

        public async Task<IActionResult> RedirectToDetails(int productId)
        {
            var product = await productService.GetProductViewModelAsync(productId);

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
