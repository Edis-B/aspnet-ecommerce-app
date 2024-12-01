using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Models;

namespace TechStoreApp.Web.ViewModels.Orders
{
    public class UserOrderSingleViewModel
    {
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public double TotalPrice { get; set; }
        public KeyValuePair<string, int> CurrentStatus { get; set; }
        public List<KeyValuePair<string, int>> Statuses { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
