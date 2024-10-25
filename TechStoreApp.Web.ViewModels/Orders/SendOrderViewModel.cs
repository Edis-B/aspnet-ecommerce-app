using TechStoreApp.Web.ViewModels.Address;

namespace TechStoreApp.Web.ViewModels.Orders
{
    public class SendOrderViewModel
    {
        public AddressFormModel Address { get; set; }
        public double TotalCost { get; set; }
    }
}
