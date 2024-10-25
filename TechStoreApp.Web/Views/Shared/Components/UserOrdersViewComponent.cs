using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Web.ViewModels.Orders;

namespace TechStoreApp.Web.Views.Shared.Components
{
    public class UserOrdersViewComponent : ViewComponent
    {
        private readonly TechStoreDbContext context;
        public UserOrdersViewComponent(TechStoreDbContext _context)
        {
            context = _context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId =  HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .ToListAsync();

            var orderModel = new DisplayOrderViewModel()
            {
                Orders = orders
            };

            return View("UserOrders", orderModel);
        }
    }
}
