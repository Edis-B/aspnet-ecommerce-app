using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.Cart;
using TechStoreApp.Web.ViewModels.PaymentDetail;

namespace TechStoreApp.Web.ViewModels.Orders
{
    public class OrderPageViewModel
    {
        public decimal TotalCost { get; set; }
        [Required(ErrorMessage = "Selecting a payment method is required!")]
        public int PaymentId { get; set; }
        public AddressFormModel Address { get; set; }
        [AllowNull]
        public List<PaymentViewModel> Payments { get; set; } = new List<PaymentViewModel>();
        public List<AddressViewModel> AllUserAddresses { get; set; } = new List<AddressViewModel>();
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
    }
}
