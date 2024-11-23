using System.ComponentModel.DataAnnotations;

namespace TechStoreApp.Web.ViewModels.Address
{
    public class AddressViewModel
    {
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public int? Id { get; set; } = null!;
    }
}
