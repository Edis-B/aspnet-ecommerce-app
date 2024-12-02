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
        private readonly IUserService userService;
        public FavoriteService(IRepository<Favorited, object> _favoritedRepository,
            IUserService _userService)
        {
            favoritedRepository = _favoritedRepository;
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

            model.Products = await favoritedRepository 
                .GetAllAttached()
                .Where(f => f.UserId == userId)
                .Select(f => new ProductViewModel
                {
                    ProductId = f.ProductId,
                    Name = f.Product.Name,
                    ImageUrl = f.Product.ImageUrl,
                    DateLiked = f.FavoritedAt.ToString("dd/MM/yyyy"),
                    TotalLikes = f.Product.Favorites.Sum(f => 1)
                })
                .ToListAsync();

            return model;
        }
    }
}
