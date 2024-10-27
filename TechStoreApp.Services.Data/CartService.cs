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
using TechStoreApp.Web.ViewModels.Products;

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
        public async Task<JsonResult> AddToCartAsync(AddToCartViewModel model)
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

        public async Task<CartViewModel> GetCartItemsAsync()
        {
            var userId = userService.GetUserId();

            var _user = await context.Users
                .Where(x => x.Id == userId)
                .Include(x => x.Cart)
                    .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync();
                

            var cartItems = _user?.Cart?.CartItems
                .Select(ci => new CartItemViewModel
                {
                    Quantity = ci.Quantity,
                    CartId = ci.ProductId,
                    ProductId = ci.ProductId,
                    Product = context.Products
                        .Where(p => p.ProductId == ci.ProductId)
                        .Select(p => new ProductViewModel()
                        {
                            ProductId = p.ProductId,
                            ImageUrl = p.ImageUrl
                        })
                        .FirstOrDefault() ?? new ProductViewModel()
                })
                .ToList();

            CartViewModel newViewModel = new()
            {
                CartItems = cartItems
            };

            return newViewModel;
        }

        public async Task<JsonResult> IncreaseCountAsync(CartFormModel model)
        {
            var product = await context.Products
                .FindAsync(model.ProductId);

            if (product.Stock <= 0)
            {
                return new JsonResult(new { success = false, message = "Out of stock!" });
            }

            var userId = userService.GetUserId();

            var _user = await context.Users
                .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync(u => u.Id == userId);
            

            var _cartItem = _user.Cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == model.ProductId);

            if (_cartItem.Quantity >= product.Stock)
            {
                return new JsonResult(new { success = false, newQuantity = _cartItem.Quantity, message = "Reached stock limit!" });
            }

            _cartItem.Quantity++;
            await context.SaveChangesAsync();

            return new JsonResult(new { success = true, newQuantity = _cartItem.Quantity, message = "Successfully increased items!" });
        }

        public async Task<JsonResult> DecreaseCountAsync(CartFormModel model)
        {
            var userId = userService.GetUserId();

            var _user = context.Users
                .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                .FirstOrDefault(u => u.Id == userId);


            var _cartItem = _user.Cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == model.ProductId);

            _cartItem.Quantity--;
            await context.SaveChangesAsync();

            return new JsonResult(new { success = true, newQuantity = _cartItem.Quantity, message = "Successfully decreased items!" });
        }

        public async Task<JsonResult> GetCartItemsCountAsync()
        {
            var userId = userService.GetUserId();
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new JsonResult(new { total = 0 });
            }

            if (user.Cart == null)
            {
                return new JsonResult(new { total = 0 });
            }

            var totalItems = user.Cart.CartItems
                .Sum(c => c.Quantity);

            return new JsonResult(new { total = totalItems });
        }


        public Task<JsonResult> RemoveFromCart(RemoveFromCartViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<JsonResult> ClearCart()
        {
            throw new NotImplementedException();
        }
    }
}
