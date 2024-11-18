using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Orders;

namespace TechStoreApp.Web.Views.Shared.Components
{
    public class UserOrdersViewComponent : ViewComponent
    {
        private readonly IOrderService orderService;
        public UserOrdersViewComponent(IOrderService _orderService)
        {
            orderService = _orderService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await orderService.GetUserOrdersListViewModelAsync();

            return View("UserOrders", model);
        }
    }
}
