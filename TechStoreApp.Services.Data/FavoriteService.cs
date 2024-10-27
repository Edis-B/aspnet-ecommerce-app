using Microsoft.AspNetCore.Mvc;
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
using TechStoreApp.Web.ViewModels.Favorites;

namespace TechStoreApp.Services.Data
{
    public class FavoriteService : IFavoriteService
    {
        private readonly TechStoreDbContext context;
        private readonly IUserService userService;
        public FavoriteService(TechStoreDbContext _context, IUserService _userService)
        {
            context = _context;
            userService = _userService;

        }
        public async Task<JsonResult> AddToFavoritesAsync(FavoriteFormModel model)
        {
            string userId = userService.GetUserId();

            var newFavoriteProduct = new Favorited
            {
                ProductId = model.ProductId,
                UserId = userId,
                FavoritedAt = DateTime.Now
            };

            await context.Favorited.AddAsync(newFavoriteProduct);
            await context.SaveChangesAsync();

            return new JsonResult(new { message = "Successfully added to favorites" });
        }

        public Task<FavoriteViewModel> GetUserFavoritesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<JsonResult> RemoveFromFavoritesAsync(FavoriteFormModel model)
        {
            throw new NotImplementedException();
        }
    }
}
