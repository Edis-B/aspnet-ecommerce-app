using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Category;
using TechStoreApp.Web.ViewModels.Home;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Services.Data
{
    public class HomeService : IHomeService
    {
        private readonly IRepository<Product, int> productRepository;
        private readonly IRepository<Review, int> reviewRepository;
        private readonly IRepository<Category, int> categoryRepository;

        private readonly IUserService userService;
        List<int> categories = new List<int>() { 1, 3, 5 };
        public HomeService(IRepository<Product, int> _productRepository,
            IRepository<Review, int> _reviewRepository,
            IRepository<Category, int> _categoryRepository,
            IUserService _userService)
        {
            productRepository = _productRepository;
            reviewRepository = _reviewRepository;
            categoryRepository = _categoryRepository;
            userService = _userService;
        }
        public async Task<HomeViewModel> GetHomeViewModel()
        {
            var userId = userService.GetUserId();

            var Products = await productRepository.GetAllAttached()
                .Where(p => p.IsFeatured)
                .Where(p => p.Stock > 0)
                .Select(p => new ProductViewModel()
                {
                    ProductId = p.ProductId,
                    CategoryId = p.CategoryId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl,
                    TotalLikes = p.Favorites.Count(),
                    CheckedString = p.Favorites.Any(f => f.UserId == userId) ? "checked" : "unchecked",
                })
                .ToListAsync();

            //var Reviews = reviewRepository.GetAllAttached();

            var Categories = await categoryRepository.GetAllAttached()
                .Where(c => categories.Contains(c.CategoryId))
                .Select(c => new CategoryViewModel()
                {
                    ImageUrls = c.Products.Take(4).Select(p => p.ImageUrl).ToList()!,
                    Description = c.Description!,
                    Id = c.CategoryId
                })
                .ToListAsync();

            var model = new HomeViewModel()
            {
                FeaturedProducts = Products,

                Categories = Categories
            };

            return model;
        }
    }
}
