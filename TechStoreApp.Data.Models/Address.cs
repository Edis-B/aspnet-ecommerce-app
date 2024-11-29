using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TechStoreApp.Common.EntityValidationConstraints.Address;
namespace TechStoreApp.Data.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }

        public Guid UserId { get; set; }

        [Required]
        [MinLength(minCountryStringLength)]
        [MaxLength(maxCountryStringLength)]
        public string Country { get; set; }

        [Required]
        [MinLength(minCityStringLength)]
        [MaxLength(maxCityStringLength)]
        public string City { get; set; }

        [Required]
        [Range(minPostalCode, maxPostalCode)]
        public int PostalCode { get; set; }

        [Required]
        [MinLength(minAddressStringLength)]
        [MaxLength(maxAddressStringLength)]
        public string Details { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}
