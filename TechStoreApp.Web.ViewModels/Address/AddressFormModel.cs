using System.ComponentModel.DataAnnotations;

namespace TechStoreApp.Web.ViewModels.Address
{
    public class AddressFormModel
    {
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [MaxLength(500)]
        public string Details { get; set; }
        [Required]
        public int PostalCode { get; set; }
        public int Id { get; set; }
    }
}
