using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Models;

namespace TechStoreApp.Data.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }
        [Key]
        public string UserId { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public int PostalCode { get; set; }
        [Required]
        [MaxLength(500)]
        public string Details { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}
