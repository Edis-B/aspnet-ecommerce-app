using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Common;

namespace TechStoreApp.Data.Models
{
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }

        [Required]
        [MinLength(EntityValidationConstraints.Status.minDescriptionStringLength)]
        [MaxLength(EntityValidationConstraints.Status.maxDescriptionStringLength)]
        public string Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
