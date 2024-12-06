using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TechStoreApp.Common.EntityValidationConstraints.Status;

namespace TechStoreApp.Data.Models
{
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }

        [Required]
        [MinLength(minDescriptionStringLength)]
        [MaxLength(maxDescriptionStringLength)]
        public string Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
