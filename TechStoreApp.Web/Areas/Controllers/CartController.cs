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
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels;
using TechStoreApp.Web.ViewModels.Cart;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Web.Areas.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService cartService;

        public CartController(ICartService _cartService)
        {
            cartService = _cartService;
        }
        
        [HttpPost]
        public async Task<JsonResult> AddToCart([FromBody] AddToCartViewModel model)
        {
            return await cartService.AddToCartAsync(model);
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var usersCartItems = await cartService.GetCartItemsAsync();

            return View(usersCartItems);
        }
        [HttpPut]
        public async Task<JsonResult> IncreaseCount([FromBody] CartFormModel model)
        {
            return await cartService.IncreaseCountAsync(model);
        }

        [HttpPut]
        public async Task<JsonResult> DecreaseCount([FromBody] CartFormModel model)
        {
            return await cartService.DecreaseCountAsync(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetCartItemsCount()
        {
            return await cartService.GetCartItemsCountAsync();
        }
        [HttpDelete]
        public async Task<JsonResult> RemoveFromCart([FromBody] RemoveFromCartViewModel model)
        {
            return await cartService.RemoveFromCartAsync(model);
        }
        [HttpDelete]
        public async Task<JsonResult> ClearCart()
        {
            return await cartService.ClearCartAsync();
        }
    }
}
