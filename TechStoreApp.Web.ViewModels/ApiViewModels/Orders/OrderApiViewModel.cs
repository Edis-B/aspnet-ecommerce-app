using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Models;

namespace TechStoreApp.Web.ViewModels.ApiViewModels.Orders
{
    public class OrderApiViewModel
    {
        public OrderApiViewModel() { }
        public OrderApiViewModel(Order o)
        {
            OrderId = o.OrderId;
            UserId = o.UserId.ToString();
            UserName = o.User.UserName!;
            TotalPrice = (double)o.TotalAmount;
            OrderStatus = o.Status.Description;
            DeliveryAddress = o.ShippingAddress!;

            Products = o.OrderDetails.Select(od => new OrderDetailApiViewModel()
            {
                OrderDetailId = od.OrderDetailId,
                ProductId = od.ProductId,
                ProductName = od.Product!.Name,
                Quantity = od.Quantity,
                UnitPrice = (double)od.UnitPrice
            });
        }
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public double TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string DeliveryAddress { get; set; }
        public IEnumerable<OrderDetailApiViewModel> Products { get; set; }
    }
}
