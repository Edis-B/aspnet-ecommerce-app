using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Versioning;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels;
using TechStoreApp.Web.ViewModels.Cart;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Web.Areas.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly TechStoreDbContext context;

        public CartController(TechStoreDbContext _context)
        {
            context = _context;
        }
        
        [HttpPost]
        public async Task<JsonResult> AddToCart([FromBody] AddToCartViewModel model)
        {
            int productId = model.ProductId;
            string userId = GetUserId();

            var product = await context.Products
                .FindAsync(productId);

            if (product.Stock <= 0)
            {
                return Json(new { success = false, message = "Out of stock!" });
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
            else
            {
                var cartItem = user.Cart.CartItems
                .Where(ci => ci.ProductId == productId)
                .FirstOrDefault();

                if (cartItem.Quantity >= product.Stock)
                {
                    return Json(new { success = false, message = "Reached stock limit!" });
                }

                cartItem.Quantity++;
                await context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "Product added to cart!" });
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var userId = GetUserId();
            var _user = await context.Users
                .Where(u => u.Id == userId)
                .Include(c => c.Cart)
                    .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
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

            return View(newViewModel);
        }
        [HttpPut]
        public async Task<JsonResult> IncreaseCount([FromBody] CartFormModel model)
        {
            var product = await context.Products
                .FindAsync(model.ProductId);

            if (product.Stock <= 0)
            {
                return Json(new { success = false, message = "Out of stock!" });
            }

            var userId = GetUserId();
            var _user = await context.Users
                .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync(u => u.Id == userId);
;

			var _cartItem = _user.Cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == model.ProductId);

            if (_cartItem.Quantity >= product.Stock)
            {
                return Json(new { success = false, newQuantity = _cartItem.Quantity, message = "Reached stock limit!" });
            }

            _cartItem.Quantity++;
            await context.SaveChangesAsync();

            return Json(new { success = true, newQuantity = _cartItem.Quantity, message = "Successfully increased items!" });
        }

        [HttpPut]
        public async Task<IActionResult> DecreaseCount([FromBody] CartFormModel model)
        {
            var userId = GetUserId();
            var _user = context.Users
                .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                .FirstOrDefault(u => u.Id == userId);
            

            var _cartItem = _user.Cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == model.ProductId);

            _cartItem.Quantity--;
            await context.SaveChangesAsync();

            return Json(new { success = true, newQuantity = _cartItem.Quantity, message = "Successfully decreased items!" });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetCartItemsCount()
        {
            var userId = GetUserId();
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Json(new { total = 0 });
            }

            if (user.Cart == null)
            {
                return Json(new { total = 0 });
            }

            var totalItems = user.Cart.CartItems
                .Sum(c => c.Quantity);

            return Json(new { total = totalItems });
        }
        [HttpDelete]
        public async Task<JsonResult> RemoveFromCart([FromBody] RemoveFromCartViewModel model)
        {
            var userId = GetUserId();
            var cartItem = await context.CartItems
                .Where(ci => ci.ProductId == model.ProductId)
                .Where(ci => ci.Cart.UserId == userId)
                .FirstOrDefaultAsync();

            context.CartItems.Remove(cartItem);
            await context.SaveChangesAsync();

            return Json(new { success = true, message = "Successfully removed item from cart!" });
        }
        [HttpDelete]
        public async Task<JsonResult> ClearCart()
        {
            var userId = GetUserId();

            var cart = await context.Carts
                .Where(c => c.UserId == userId)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                return Json(new { success = false, message = "Cart is already empty!" });
            }

            context.Remove(cart);
            await context.SaveChangesAsync();

            return Json(new { success = true, message = "Successfully removed item from cart!" });
        }
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
