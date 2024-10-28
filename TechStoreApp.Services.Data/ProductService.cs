using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Products;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Services.Data
{
    public class ProductService : IProductService
    {
        private readonly TechStoreDbContext context;
        private readonly IUserService userService;
        public ProductService(TechStoreDbContext _context, IUserService _userService)
        {
            context = _context;
            userService = _userService;

        }
        public async Task<ProductViewModel> GetProductViewModelAsync(int productId)
        {
            var userId = userService.GetUserId();

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

            return product;
        }
        public async Task CreateAndAddReviewToDBAsync(ReviewFormModel model)
        {
            var userId = userService.GetUserId();

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
        }
    }
}
