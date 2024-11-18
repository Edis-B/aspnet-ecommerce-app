using System.ComponentModel.DataAnnotations;
using static TechStoreApp.Common.EntityValidationConstraints.Address;

namespace TechStoreApp.Web.ViewModels.Address
{
    public class AddressFormModel
    {
        [Required(ErrorMessage = "Country field is required!")]
        public string Country { get; set; }
        [Required(ErrorMessage = "City field is required!")]
        public string City { get; set; }
        [Required(ErrorMessage = "Details field is required!")]
        [MaxLength(500)]
        public string Details { get; set; }
        [Required(ErrorMessage = "Postal code field is required!")]
        [Range(minPostalCode, maxPostalCode, ErrorMessage = "Postal code must be between 0 and 10000!")]
        public int PostalCode { get; set; }
        public int? Id { get; set; }
    }
}
