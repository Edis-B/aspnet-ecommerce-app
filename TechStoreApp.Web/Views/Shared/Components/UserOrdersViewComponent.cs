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
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new MyOrderViewModel()
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate.ToString("dd/MM/yyyy"),
                    OrderDetails = o.OrderDetails
                        .Select(od => new OrderDetailViewModel()
                        {
                            ProductImageUrl = od.Product.ImageUrl,
                            ProductName = od.Product.Name,
                            Quantity = od.Quantity,
                            UnitPrice = od.UnitPrice
                        })
                        .ToList(),
                    ShippingAddress = o.ShippingAddress
                })
                .ToListAsync();

            var orderModel = new DisplayOrderPageViewModel();
            orderModel.Orders = orders;

            return View("UserOrders", orderModel);
        }
    }
}
