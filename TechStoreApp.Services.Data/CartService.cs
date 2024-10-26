using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels;
using TechStoreApp.Web.ViewModels.Cart;

namespace TechStoreApp.Services.Data
{
    public class CartService : ICartService
    {
        private readonly TechStoreDbContext context;
        private readonly IUserService userService;
        public CartService(TechStoreDbContext _context, IUserService _userService)
        {
            context = _context;
            userService = _userService;

        }
        public async Task<JsonResult> AddToCart(AddToCartViewModel model)
        {
            int productId = model.ProductId;
            string userId = userService.GetUserId();

            var product = await context.Products
                .FindAsync(productId);

            if (product.Stock <= 0)
            {
                return new JsonResult(new { success = false, message = "Out of stock!" });
            }

            var user = await context.Users
                .Where(x => x.Id == userId)
                .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync();


            // Create a cart if the user doesn't have one
            if (user.Cart == null)
            {
                await context.Carts.AddAsync(new Cart
                {
                    UserId = userId,
                    UpdateDate = DateTime.Now
                });
                await context.SaveChangesAsync();

            }

            var cartId = user.Cart.CartId;

            // Create a cartItem with the correct product and cart id
            if (!user.Cart.CartItems.Any(x => x.ProductId == productId))
            {
                await context.CartItems.AddAsync(new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = 1
                });
                await context.SaveChangesAsync();
            }
            // Increase quantity if cartItem already exists
            else
            {
                var cartItem = user.Cart.CartItems
                .Where(ci => ci.ProductId == productId)
                .FirstOrDefault();

                if (cartItem.Quantity >= product.Stock)
                {
                    return new JsonResult(new { success = false, message = "Reached stock limit!" });
                }

                cartItem.Quantity++;
                await context.SaveChangesAsync();
            }

            return new JsonResult(new { success = true, message = "Product added to cart!" });
        }

        public Task<CartViewModel> Cart()
        {
            throw new NotImplementedException();
        }

        public Task<JsonResult> ClearCart()
        {
            throw new NotImplementedException();
        }

        public Task<JsonResult> DecreaseCount(CartFormModel model)
        {
            throw new NotImplementedException();
        }

        public Task<JsonResult> GetCartItemsCount()
        {
            throw new NotImplementedException();
        }

        public Task<JsonResult> IncreaseCount(CartFormModel model)
        {
            throw new NotImplementedException();
        }

        public Task<JsonResult> RemoveFromCart(RemoveFromCartViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
