using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Versioning;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels;
using TechStoreApp.Web.ViewModels.Cart;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IRequestService requestService;

        public CartController(ICartService _cartService,
            IRequestService _requestService)
        {
            cartService = _cartService;
            requestService = _requestService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(ProductIdFormModel model)
        {
            if (requestService.IsAjaxRequest(Request))
            {
                model = await requestService.GetProductIdFromRequest<ProductIdFormModel>(Request);
                var response = await cartService.AddToCartAsync(model);

                return response;
            }
            else
            {
                var response = await cartService.AddToCartAsync(model);

                return RedirectToAction("Cart", "Cart");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var usersCartItems = await cartService.GetCartItemsAsync();

            return View(usersCartItems);
        }

        [HttpPost]
        public async Task<IActionResult> IncreaseCount(ProductIdFormModel model)
        {
            if (requestService.IsAjaxRequest(Request))
            {
                model = await requestService.GetProductIdFromRequest<ProductIdFormModel>(Request);
                var response = await cartService.IncreaseCountAsync(model);

                return response;
            }
            else
            {
                var response = await cartService.IncreaseCountAsync(model);

                return RedirectToAction("Cart", "Cart");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseCount(ProductIdFormModel model)
        {
            if (requestService.IsAjaxRequest(Request))
            {
                model = await requestService.GetProductIdFromRequest<ProductIdFormModel>(Request);
                var response = await cartService.DecreaseCountAsync(model);

                return response;
            }
            else
            {
                var response = await cartService.DecreaseCountAsync(model);

                return RedirectToAction("Cart", "Cart");
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(ProductIdFormModel model)
        {
            if (requestService.IsAjaxRequest(Request))
            {
                model = await requestService.GetProductIdFromRequest<ProductIdFormModel>(Request);
                var response = await cartService.RemoveFromCartAsync(model);

                return response;
            }
            else
            {
                var response = await cartService.RemoveFromCartAsync(model);

                return RedirectToAction("Cart", "Cart");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            if (requestService.IsAjaxRequest(Request))
            {
                var response = await cartService.ClearCartAsync();

                return response;
            }
            else
            {
                var response = await cartService.ClearCartAsync();

                return RedirectToAction("Cart", "Cart");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetCartItemsCount()
        {
            return await cartService.GetCartItemsCountAsync();
        }

    }
}
