using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly ICartItemService cartItemService;
        private readonly IRequestService requestService;

        public CartController(ICartService _cartService,
            ICartItemService _cartItemService,
            IRequestService _requestService)
        {
            cartService = _cartService;
            cartItemService = _cartItemService;
            requestService = _requestService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(ProductIdFormModel model)
        {
            if (requestService.IsAjaxRequest(Request))
            {
                model = await requestService.ExtractModelFromRequestBody<ProductIdFormModel>(Request);
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
                model = await requestService.ExtractModelFromRequestBody<ProductIdFormModel>(Request);
                var response = await cartItemService.IncreaseCountAsync(model);

                return response;
            }
            else
            {
                var response = await cartItemService.IncreaseCountAsync(model);

                return RedirectToAction("Cart", "Cart");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseCount(ProductIdFormModel model)
        {
            if (requestService.IsAjaxRequest(Request))
            {
                model = await requestService.ExtractModelFromRequestBody<ProductIdFormModel>(Request);
                var response = await cartItemService.DecreaseCountAsync(model);

                return response;
            }
            else
            {
                var response = await cartItemService.DecreaseCountAsync(model);

                return RedirectToAction("Cart", "Cart");
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(ProductIdFormModel model)
        {
            if (requestService.IsAjaxRequest(Request))
            {
                model = await requestService.ExtractModelFromRequestBody<ProductIdFormModel>(Request);
                var response = await cartItemService.RemoveFromCartAsync(model);

                return response;
            }
            else
            {
                var response = await cartItemService.RemoveFromCartAsync(model);

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
            return await cartItemService.GetCartItemsCountAsync();
        }

    }
}
