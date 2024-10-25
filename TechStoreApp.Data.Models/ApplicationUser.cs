using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStoreApp.Data.Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfilePictureUrl { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
