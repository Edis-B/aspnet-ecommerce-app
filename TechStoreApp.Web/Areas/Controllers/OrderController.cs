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
        private readonly TechStoreDbContext context;
        private readonly IOrderService orderService;
        public OrderController(TechStoreDbContext _context, IOrderService _orderService)
        {
            orderService = _orderService;
            context = _context;
        }
        [HttpGet]
        public async Task<IActionResult> Order()
        {
            var orderViewModel = await orderService.GetOrderViewModelAsync();

            return View("Order", orderViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> FinalizeOrder(OrderViewModel model)
        {
            var newModel = await orderService.GetOrderFinalizedModelAsync(model);

            return View("OrderFinalized", newModel);
        }
        [HttpPost]
        public async Task<IActionResult> SendOrder(SendOrderViewModel model)
        {
            await orderService.SendOrderAsync(model);

            return RedirectToAction("Index", "Profile");
        }
        [HttpGet]
        public async Task<OkObjectResult> GetAddress([FromRoute] int id) 
        {
            var result = await orderService.GetAddressByIdAsync(id);

            return Ok(result);
        }
    }
}
