using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Web.Areas.Admin.Views.Shared.Components
{
    [ViewComponent]
    public class UserOrders : ViewComponent
    {
        private readonly IOrderService orderService;
        public UserOrders(IOrderService _orderService)
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
