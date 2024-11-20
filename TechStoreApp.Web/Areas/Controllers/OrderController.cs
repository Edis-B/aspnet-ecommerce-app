using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Policy;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.Cart;
using TechStoreApp.Web.ViewModels.Orders;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Web.Areas.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService _orderService)
        {
            orderService = _orderService;
        }

        [HttpPost]
        public IActionResult SharedOrderForm(OrderPageViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View("Order", model);
            }

            if (action == "FinalizeOrder")
            {
                return RedirectToActionPreserveMethod(action, "Order", model);
            } else if (action == "SaveAddress")
            {
                return RedirectToActionPreserveMethod(action, "Address", model);
            }

            return View("Order", model);
        }

        [HttpGet]
        public async Task<IActionResult> Order()
        {
            var orderViewModel = await orderService.GetOrderViewModelAsync();

            return View("Order", orderViewModel);
        }

        [HttpGet]
        public IActionResult FinalizeOrder()
        {
            return RedirectToAction(nameof(Order));
        }

        [HttpPost]
        public async Task<IActionResult> FinalizeOrder(OrderPageViewModel model)
        {
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
