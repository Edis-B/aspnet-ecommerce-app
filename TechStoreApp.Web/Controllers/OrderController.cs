using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.Infrastructure;
using TechStoreApp.Web.ViewModels.Orders;
using static TechStoreApp.Common.GeneralConstraints;

namespace TechStoreApp.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly ICartService cartService;
        private readonly IUserService userService;

        public OrderController(IOrderService _orderService,
            ICartService _cartService,
            IUserService _userService)
        {
            orderService = _orderService;
            cartService = _cartService;
            userService = _userService;
        }

        [HttpPost]
        public async Task<IActionResult> SharedOrderForm(OrderPageViewModel model, string action, int PaymentMethod = 1)
        {
            model.PaymentId = model.PaymentId == 0 ? PaymentMethod : model.PaymentId;

            if (!ModelState.IsValid)
            {
                return View("Order", model);
            }

            if (action == "FinalizeOrder")
            {
                TempData["Model"] = JsonConvert.SerializeObject(model);
                return RedirectToAction("FinalizeOrder", "Order");

            }
            else if (action == "SaveAddress")
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
        public async Task<IActionResult> FinalizeOrder(OrderFinalizedPageViewModel model, int PaymentMethod)
        {
            // TempData from SharedForm Redirection
            var unfinishedOrder = TempDataUtility
                .GetTempData<OrderPageViewModel>(TempData, "Model");

            if (model == null)
            {
                return RedirectToAction(nameof(Order));
            }

            
            OrderFinalizedPageViewModel? newModel = await orderService.GetOrderFinalizedModelAsync(unfinishedOrder);

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

            if (model == null)
            {
                if (User.IsInRole(AdminRoleName))
                {
                    return RedirectToAction("Error", "Home");
                }
                else return RedirectToAction("Index", "Home");
            }

            return View("CompletedOrder", model);
        }
    }
}
