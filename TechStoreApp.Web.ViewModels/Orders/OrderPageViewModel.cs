using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.Cart;

namespace TechStoreApp.Web.ViewModels.Orders
{
    public class OrderPageViewModel
    {
        public List<AddressViewModel> UserAddresses { get; set; } = new List<AddressViewModel>();
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
        public AddressFormModel Address { get; set; }
        public decimal TotalCost { get; set; }
    }
}
