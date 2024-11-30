using CinemaApp.Web.Infrastructure.Attributes;
using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.ApiViewModels.Orders;

namespace TechStoreApp.WebAPI.Controllers
{
    public class OrdersApiController : Controller
    {
        private const string action = "[action]";
        private readonly IOrderService orderService;
        public OrdersApiController(IOrderService _orderService)
        {
            orderService = _orderService;
        }

        [HttpGet(action)]
        [AdminCookieOnly]
        [ProducesResponseType((typeof(IEnumerable<OrderApiViewModel>)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await orderService.GetAllOrders();

            if (!orders.Any()) return NotFound("Orders not found!");

            return Ok(orders);
        }

        [HttpGet(action)]
        [AdminCookieOnly]
        [ProducesResponseType((typeof(IEnumerable<OrderApiViewModel>)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllOrdersByUser(string userId)
        {
            var orders = await orderService.GetAllOrdersByUserId(userId);

            if (!orders.Any()) return NotFound("User does not have any orders!");

            return Ok(orders);
        }
    }
}
