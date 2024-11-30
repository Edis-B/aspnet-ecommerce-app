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
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public double TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public IEnumerable<OrderDetailApiViewModel> Products { get; set; }
    }
}
