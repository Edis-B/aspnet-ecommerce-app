using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.Cart;

namespace TechStoreApp.Web.ViewModels.Orders
{
    public class OrderViewModel
    {
        public List<AddressViewModel> UserAddresses { get; set; }
        public List<CartItemViewModel> CartItems { get; set; }
        public AddressFormModel Address { get; set; }
        public decimal TotalCost { get; set; }
    }
}
