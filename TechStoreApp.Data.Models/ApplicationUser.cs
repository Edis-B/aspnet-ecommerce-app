using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechStoreApp.Common;

namespace TechStoreApp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(EntityValidationConstraints.User.maxPfpImageUrlStringLength)]
        public string? ProfilePictureUrl { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
