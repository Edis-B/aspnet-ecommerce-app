using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TechStoreApp.Common.EntityValidationConstraints.Product;

namespace TechStoreApp.Data.Seeding.DataTransferObjects
{
    public class ImportProductDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MinLength(minNameStringLength)]
        [MaxLength(maxNameStringLength)]
        public string Name { get; set; }

        [MinLength(minDescriptionStringLength)]
        [MaxLength(maxDescriptionStringLength)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Range(minPrice, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(minStock, int.MaxValue)]
        public int Stock { get; set; }

        [MaxLength(maxImageUrlStringLength)]
        public string? ImageUrl { get; set; }
        public bool IsFeatured { get; set; } = false;
    }
}
