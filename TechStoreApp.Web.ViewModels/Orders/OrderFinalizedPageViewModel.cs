using TechStoreApp.Web.ViewModels.Address;

namespace TechStoreApp.Web.ViewModels.Orders
{
    public class OrderFinalizedPageViewModel
    {
        public CartViewModel Cart { get; set; }

        public decimal TotalSum { get; set; }

        public AddressFormModel Address { get; set; }
    }
}
