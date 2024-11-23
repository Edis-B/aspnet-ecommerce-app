using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.Cart;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Services.Data
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart, int> cartRepository;
        private readonly IRepository<CartItem, object> cartItemRepository;
        private readonly IRepository<Product, int> productRepository;
        private readonly IRepository<ApplicationUser, Guid> userRepository;
        private readonly IUserService userService;
        public CartService(IRepository<Cart, int> _cartRepository,
            IRepository<CartItem, object> _cartItemRepository,
            IRepository<Product, int> _productRepository,
            IRepository<ApplicationUser, Guid> _userRepository,
            IUserService _userService)
        {
            userRepository = _userRepository;
            cartRepository = _cartRepository;
            cartItemRepository = _cartItemRepository;
            productRepository = _productRepository;
            userService = _userService;
        }
        public async Task<JsonResult> AddToCartAsync(ProductIdFormModel model)
        {
            int productId = model.ProductId;
            string userId = userService.GetUserId();

            var product = await productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                return new JsonResult(new { success = false, message = "Error - product not found" });
            }

            if (product.Stock <= 0)
            {
                return new JsonResult(new { success = false, message = "Out of stock!" });
            }

            var user = await userRepository
                .GetAllAttached()
                .Where(x => x.Id == userId)
                .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync();


            // Create a cart if the user doesn't have one
            if (user.Cart == null)
            {
                var newCart = new Cart {
                    UserId = userId,
                    UpdateDate = DateTime.Now
                };

                await cartRepository.AddAsync(newCart);
            }

            var cartId = user.Cart.CartId;

            // Create a cartItem with the correct product and cart id
            if (!user.Cart.CartItems.Any(x => x.ProductId == productId))
            {
                var newCartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = 1
                };
                await cartItemRepository.AddAsync(newCartItem);
            }
            // Increase quantity if cartItem already exists
            else
            {
                var cartItem = user.Cart.CartItems
                    .FirstOrDefault(ci => ci.ProductId == productId);

                if (cartItem.Quantity >= product.Stock)
                {
                    return new JsonResult(new { success = false, message = "Reached stock limit!" });
                }

                cartItem.Quantity++;
                await cartItemRepository.UpdateAsync(cartItem);
            }

            return new JsonResult(new { success = true, message = "Product added to cart!" });
        }

        public async Task<CartViewModel> GetCartItemsAsync()
        {
            var userId = userService.GetUserId();

            var _user = await userRepository.GetAllAttached()
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
                    Product = productRepository.GetAllAttached()
                        .Where(p => p.ProductId == ci.ProductId)
                        .Select(p => new ProductViewModel()
                        {
                            ProductId = p.ProductId,
                            ImageUrl = p.ImageUrl,
                            Name = p.Name
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

        public async Task<JsonResult> IncreaseCountAsync(ProductIdFormModel model)
        {
            var product = await productRepository.GetByIdAsync(model.ProductId);

            if (product.Stock <= 0)
            {
                return new JsonResult(new { success = false, message = "Out of stock!" });
            }

            var userId = userService.GetUserId();

            var _user = await userRepository.GetAllAttached()
                .Where(u => u.Id == userId)
                .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync();
            

            var theCartItem = _user.Cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == model.ProductId);

            if (theCartItem.Quantity >= product.Stock)
            {
                return new JsonResult(new { success = false, newQuantity = theCartItem.Quantity, message = "Reached stock limit!" });
            }

            theCartItem.Quantity++;
            await cartItemRepository.UpdateAsync(theCartItem);

            return new JsonResult(new { success = true, newQuantity = theCartItem.Quantity, message = "Successfully increased items!" });
        }

        public async Task<JsonResult> DecreaseCountAsync(ProductIdFormModel model)
        {
            var userId = userService.GetUserId();

            var user = await userRepository.GetAllAttached()
                .Where(u => u.Id == userId)
                .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync();

            if (user!.Cart == null || user.Cart.CartItems == null)
            {
                return new JsonResult(new { success = false, message = "Error - User cart not found" });
            }

            var theCartItem = user.Cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == model.ProductId);

            if (theCartItem == null)
            {
                return new JsonResult(new { success = false, message = "Error - Item in cart not found" });
            }

            if (theCartItem.Quantity <= 1)
            {
                await RemoveFromCartAsync(model);
            }

            theCartItem.Quantity--;
            await cartItemRepository.UpdateAsync(theCartItem);

            return new JsonResult(new { success = true, newQuantity = theCartItem.Quantity, message = "Successfully decreased items!" });
        }

        public async Task<JsonResult> GetCartItemsCountAsync()
        {
            var userId = userService.GetUserId();
            var user = await userRepository
                .GetAllAttached()
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

        public async Task<JsonResult> RemoveFromCartAsync(ProductIdFormModel model)
        {
            var userId = userService.GetUserId();

            var cartItem = await cartItemRepository
                .GetAllAttached()
                .Where(ci => ci.ProductId == model.ProductId)
                .Where(ci => ci.Cart.UserId == userId)
                .FirstOrDefaultAsync();

            // Defensive check
            if (cartItem == null) 
            {
                return new JsonResult(new { success = true, message = "Item is not in cart!" });
            }

            await cartItemRepository.DeleteAsync(cartItem.CartId, cartItem.ProductId);

            return new JsonResult(new { success = true, message = "Successfully removed item from cart!" });
        }

        public async Task<JsonResult> ClearCartAsync()
        {
            var userId = userService.GetUserId();

            var cart = await cartRepository
                .GetAllAttached()
                .Where(c => c.UserId == userId)
                    .Include(c => c.CartItems)
                .FirstOrDefaultAsync();

            if (cart == null || cart.CartItems.Count == 0)
            {
                return new JsonResult(new { success = false, message = "Cart is already empty!" });
            }

            await cartRepository.DeleteAsync(cart.CartId);

            return new JsonResult(new { success = true, message = "Successfully removed item from cart!" });
        }
    }
}
