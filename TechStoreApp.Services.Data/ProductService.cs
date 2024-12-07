using Microsoft.EntityFrameworkCore;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Category;
using TechStoreApp.Web.ViewModels.Products;
using TechStoreApp.Web.ViewModels.Reviews;
using TechStoreApp.Web.ViewModels.ApiViewModels.Products;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TechStoreApp.Web.ViewModels.User;

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
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return default!;
            }

            var result = new ProductViewModel
            {
                ProductId = productId,
                CategoryId = product!.CategoryId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                TotalLikes = product.Favorites.Sum(f => 1),
                CheckedString = product.Favorites.Any(f => f.UserId == userId) ? "checked" : "unchecked"
            };

            result.Reviews = product.Reviews
                .Select(r => new ReviewViewModel
                {
                    Comment = r.Comment,
                    ProductId = r.ProductId,
                    Rating = r.Rating,
                    Author = new UserViewModel()
                    {
                        Name = r.User!.UserName!,
                        PictureUrl = r.User.ProfilePictureUrl!
                    }
                })
                .ToList();

            return result;
        }



        public async Task<EditProductViewModel> GetEditProductViewModelAsync(int productId)
        {
            var product = await productRepository
                .GetAllAttached()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return default!;
            }

            var results = new EditProductViewModel
            {
                ProductId = productId,
                ProductName = product.Name,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Description ?? "Invalid Category",
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
            };

            results.Categories = await categoryRepository
                .GetAllAttached()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.CategoryId,
                    Description = c.Description ?? "Error with category"
                })
                .ToListAsync();

            return results!;
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
                        Description = c.Description!,
                        Id = c.CategoryId
                    })
            };

            return newModel;

        }

        public IEnumerable<ProductApiViewModel> ApiGetAllProducts()
        {
            var result = productRepository.GetAllAttached()
                .Select(p => new ProductApiViewModel()
                {
                    ProductId = p.ProductId,    
                    CategoryId = p.CategoryId,
                    Name = p.Name,
                    Description= p.Description,
                    Price = p.Price,
                    Stock= p.Stock,
                    ImageUrl = p.ImageUrl,  
                    TotalLikes = p.Favorites.Count()    
                });

            return result;
        }

        public IEnumerable<ProductApiViewModel> ApiGetAllProductsByQuery(string? productName = null, int? categoryId = null)
        {
            var query = productRepository.GetAllAttached();

            if (productName != null)
            {
                productName = productName!.ToLower();
                query = query.Where(p => p.Name.Contains(productName!));             
            }

            if (categoryId != null)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            var result = query.Select(p => new ProductApiViewModel()
            {
                ProductId = p.ProductId,
                CategoryId = p.CategoryId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                TotalLikes = p.Favorites.Count()
            });

            return result;
        }
    }
}
