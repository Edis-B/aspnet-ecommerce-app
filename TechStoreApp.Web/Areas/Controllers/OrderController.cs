using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.Utilities;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.Orders;

namespace TechStoreApp.Web.Areas.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly ICartService cartService;

        public OrderController(IOrderService _orderService, 
            ICartService _cartService)
        {
            orderService = _orderService;
            cartService = _cartService;
        }

        [HttpPost]
        public async Task<IActionResult> SharedOrderForm(OrderPageViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                var address = model.Address;

                // Repopulate other data
                model = await orderService.GetOrderViewModelAsync(null);
                model.Address = address;

                return View("Order", model);
            }

            if (action == "FinalizeOrder")
            {
                TempData["Model"] = JsonConvert.SerializeObject(model.Address);
                return RedirectToActionPreserveMethod("FinalizeOrder", "Order");

            } else if (action == "SaveAddress")
            {
                TempData["Model"] = JsonConvert.SerializeObject(model.Address);
                return RedirectToActionPreserveMethod("SaveAddress", "Address");
            }

            return View("Order", model);
        }

        [HttpGet]
        public async Task<IActionResult> Order(int? addressId)
        {
            var orderViewModel = await orderService.GetOrderViewModelAsync(addressId);

            return View("Order", orderViewModel);
        }

        [HttpGet]
        public IActionResult FinalizeOrder()
        {
            return RedirectToAction(nameof(Order));
        }

        [HttpPost]
        public async Task<IActionResult> FinalizeOrder(AddressFormModel model)
        {
            // TempData from SharedForm
            model = TempDataUtility.GetTempData<AddressFormModel>(TempData, "Model") ?? model;

            var newModel = await orderService.GetOrderFinalizedModelAsync(model);

            return View("OrderFinalized", newModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendOrder(SendOrderViewModel model)
        {
            await orderService.SendOrderAsync(model);

            return RedirectToAction("Index", "Account");
        }

        [HttpGet("Order/CompletedOrder/{orderId:int}")]
        public async Task<IActionResult> CompletedOrder(int orderId)
        {
            var model = await orderService.GetDetailsOfOrder(orderId);

            if (model == null) {
                return RedirectToAction("Index", "Home");
            }

            return View("CompletedOrder", model);
        }
    }
}
