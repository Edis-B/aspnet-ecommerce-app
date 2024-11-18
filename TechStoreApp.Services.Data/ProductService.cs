using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Category;
using TechStoreApp.Web.ViewModels.Products;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Services.Data
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product, int> productRepository;
        private readonly IRepository<Review, int> reviewRepository;
        private readonly IRepository<Category, int> categoryRepository;
        private readonly IUserService userService;
        public ProductService(IUserService _userService,
            IRepository<Review, int> _reviewRepository,
            IRepository<Category, int> _categoryRepository,
            IRepository<Product, int> _productRepository)
        {
            userService = _userService;
            productRepository = _productRepository;
            reviewRepository = _reviewRepository;
            categoryRepository = _categoryRepository;
        }

        public async Task<ProductViewModel> GetProductViewModelAsync(int productId)
        {
            var userId = userService.GetUserId();

            var product = await productRepository
                .GetAllAttached()
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
                    TotalLikes = p.Favorites.Sum(f => 1),
                    CheckedString = p.Favorites.Any(f => f.UserId == userId) ? "checked" : "unchecked"
                })
                .FirstOrDefaultAsync();

            product.Reviews = await reviewRepository
                .GetAllAttached()
                .Where(r => r.ProductId == productId)
                .Select(r => new ReviewViewModel
                {
                    Comment = r.Comment,
                    ProductId = r.ProductId,
                    Rating = r.Rating,
                    Author = r.User!.UserName ?? "Error with UserName"
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

            await reviewRepository.AddAsync(newReview);
        }

        public async Task<EditProductViewModel> GetEditProductViewModelAsync(int productId)
        {
            var product = await productRepository
                .GetAllAttached()
                .Include(p => p.Category)
                .Where(p => p.ProductId == productId)
                .Select(p => new EditProductViewModel
                {
                    ProductId = productId,
                    ProductName = p.Name,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Description ?? "Invalid Category",
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl,
                    Categories = categoryRepository
                        .GetAllAttached()
                        .Select(c => new CategoryViewModel()
                        {
                            Id = c.CategoryId,
                            Description = c.Description ?? "Error with category"
                        })                   
                        .ToList()
                })
                .FirstOrDefaultAsync() ?? null;

            return product!;
        }

        public async Task EditProductAsync(EditProductViewModel model)
        {
            var product = await productRepository.GetByIdAsync(model.ProductId);

            product.CategoryId = model.CategoryId;
            product.Name = model.ProductName;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.ImageUrl = model.ImageUrl;

            await productRepository.UpdateAsync(product);
        }

        public async Task<int> AddProductAsync(AddProductViewModel model)
        {
            var newProduct = new Product()
            {
                Name = model.ProductName,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                ImageUrl = model.ImageUrl,
                CategoryId = model.CategoryId,
            };

            await productRepository.AddAsync(newProduct);

            return newProduct.ProductId;
        }

        public AddProductViewModel GetAddProductViewModel()
        {
            var newModel = new AddProductViewModel()
            {
                Categories = categoryRepository
                    .GetAllAttached()
                    .Select(c => new CategoryViewModel()
                    {
                        Description = c.Description,
                        Id = c.CategoryId
                    })
            };

            return newModel;

        }
    }
}
