using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Favorites;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Services.Data
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IRepository<Favorited, object> favoritedRepository;
        private readonly IRepository<Product, int> productRepository;
        private readonly IUserService userService;
        public FavoriteService(IRepository<Favorited, object> _favoritedRepository,
            IRepository<Product, int> _productRepository,
            IUserService _userService)
        {
            favoritedRepository = _favoritedRepository;
            productRepository = _productRepository;
            userService = _userService;
        }
        public async Task<JsonResult> AddToFavoritesAsync(ProductIdFormModel model)
        {
            var userId = userService.GetUserId();

            var newFavoriteProduct = new Favorited
            {
                ProductId = model.ProductId,
                UserId = userId,
                FavoritedAt = DateTime.Now
            };

            await favoritedRepository.AddAsync(newFavoriteProduct);

            return new JsonResult(new { message = "Successfully added to favorites" });
        }
        public async Task<JsonResult> RemoveFromFavoritesAsync(ProductIdFormModel model)
        {
            var userId = userService.GetUserId();

            await favoritedRepository.DeleteAsync(userId, model.ProductId);

            return new JsonResult(new { message = "Successfully removed from favorites" });
        }
        public async Task<FavoriteViewModel> GetUserFavoritesAsync()
        {
            var userId = userService.GetUserId();

            var model = new FavoriteViewModel();
            var userFavorites = favoritedRepository.GetAllAttached()
                .Include(f => f.Product)
                    .ThenInclude(p => p.Category)
                .Include(f => f.Product.Favorites)
                .Where(f => f.UserId == userId);

            model.Products = await userFavorites
                .Select(f => new ProductViewModel
                {
                    ProductId = f.ProductId,
                    Name = f.Product.Name,
                    ImageUrl = f.Product.ImageUrl,
                    DateLiked = f.FavoritedAt.ToString("dd/MM/yyyy"),
                    TotalLikes = f.Product.Favorites.Sum(f => 1)
                })
                .ToListAsync();

            var interestedCategories = userFavorites
                .Select(f => f.Product.CategoryId)
                .Distinct()
                .ToArray();

            var likedProducts = model.Products.Select(p => p.ProductId);

            var recommendedProducts = new List<ProductViewModel>();
            foreach (var categoryId in interestedCategories)
            {
                recommendedProducts.AddRange(
                    productRepository.GetAllAttached()
                    .Where(p => p.CategoryId == categoryId)
                    .Where(p => !likedProducts.Contains(p.ProductId))
                    .Select(p => new ProductViewModel
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        ImageUrl = p.ImageUrl,
                        TotalLikes = p.Favorites.Count
                    })
                    .OrderByDescending(p => p.TotalLikes)
                    .Take(10 / (interestedCategories.Length == 0 ? 1 : interestedCategories.Length))
                );
            }

            Random rnd = new Random();

            recommendedProducts = recommendedProducts
                .OrderBy(rp => rnd.Next(1, 100))
                .Take(10)
                .ToList();

            model.RecommendedProducts = recommendedProducts;

            return model;
        }
    }
}
