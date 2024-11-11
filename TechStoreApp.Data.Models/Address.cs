using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Common;
using TechStoreApp.Data.Models;

namespace TechStoreApp.Data.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }

        public string UserId { get; set; }

        [Required]
        [MinLength(EntityValidationConstraints.Address.minCountryStringLength)]
        [MaxLength(EntityValidationConstraints.Address.maxCountryStringLength)]
        public string Country { get; set; }

        [Required]
        [MinLength(EntityValidationConstraints.Address.minCityStringLength)]
        [MaxLength(EntityValidationConstraints.Address.maxCityStringLength)]
        public string City { get; set; }

        [Required]
        [Range(EntityValidationConstraints.Address.minPostalCode, EntityValidationConstraints.Address.maxPostalCode)]
        public int PostalCode { get; set; }

        [Required]
        [MinLength(EntityValidationConstraints.Address.minAddressStringLength)]
        [MaxLength(EntityValidationConstraints.Address.maxAddressStringLength)]
        public string Details { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}
