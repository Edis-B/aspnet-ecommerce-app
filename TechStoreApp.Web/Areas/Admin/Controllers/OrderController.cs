using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;
using static TechStoreApp.Common.GeneralConstraints;
namespace TechStoreApp.Web.Areas.Admin.Controllers
{
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService _orderService)
        {
            orderService = _orderService;
        }

        [HttpGet]
        public IActionResult GetUserOrders(string userId)
        {
            return View("Order", userId);
        }
    }
}
