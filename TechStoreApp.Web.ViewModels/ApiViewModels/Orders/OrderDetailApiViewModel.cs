using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Web.ViewModels.ApiViewModels.Orders
{
    public class OrderDetailApiViewModel
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
