using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.DTOs.CartItems;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Services.Data
{
    public class CartItemService : ICartItemService
    {
        private readonly IRepository<CartItem, object> cartItemRepository;
        private readonly IRepository<Product, int> productRepository;
        private readonly IRepository<ApplicationUser, Guid> userRepository;
        private readonly IUserService userService;
        public CartItemService (IRepository<CartItem, object> _cartItemRepository,
            IRepository<Product, int> _productRepository,
            IRepository<ApplicationUser, Guid> _userRepository,
            IUserService _userService)
        {
            userRepository = _userRepository;
            cartItemRepository = _cartItemRepository;
            productRepository = _productRepository;
            userService = _userService;
        }
        public async Task<JsonResult> IncreaseCountAsync(ProductIdFormModel model)
        {
            var product = await productRepository.GetByIdAsync(model.ProductId);

            if (product.Stock <= 0)
            {
                return new JsonResult(new { success = false, message = "Out of stock!" });
            }

            var userId = userService.GetUserId();

            var user = await userRepository.GetAllAttached()
                .Where(u => u.Id == userId)
                .Include(u => u.Cart)
                    .ThenInclude(c => c!.CartItems)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new JsonResult(new { success = false, message = "User Error!" });
            }

            if (user.Cart == null)
            {
                return new JsonResult(new { success = false, message = "Cart Error!" });
            }

            var theCartItem = user.Cart!.CartItems
                .FirstOrDefault(ci => ci.ProductId == model.ProductId);

            if (theCartItem!.Quantity >= product.Stock)
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
                    .ThenInclude(c => c!.CartItems)
                .FirstOrDefaultAsync();

            if (user == null || user!.Cart == null || user.Cart.CartItems == null)
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
                    .ThenInclude(c => c!.CartItems)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new JsonResult(new CartItemDto { Total = 0 });
            }

            if (user.Cart == null)
            {
                return new JsonResult(new CartItemDto { Total = 0 });
            }

            var totalItems = user.Cart.CartItems
                .Sum(c => c.Quantity);

            return new JsonResult(new CartItemDto { Total = totalItems });
        }

        public async Task<JsonResult> RemoveFromCartAsync(ProductIdFormModel model)
        {
            var userId = userService.GetUserId();

            var cartItem = await cartItemRepository
                .GetAllAttached()
                .Where(ci => ci.ProductId == model.ProductId)
                .Where(ci => ci.Cart!.UserId == userId)
                .FirstOrDefaultAsync();

            if (cartItem == null)
            {
                return new JsonResult(new { success = true, message = "Item is not in cart!" });
            }

            await cartItemRepository.DeleteAsync(cartItem.CartId, cartItem.ProductId);

            return new JsonResult(new { success = true, message = "Successfully removed item from cart!" });
        }
    }
}
