using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechStoreApp.Common;

namespace TechStoreApp.Data.Models;

public partial class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryId { get; set; }

    [MinLength(EntityValidationConstraints.Category.minDescriptionStringLength)]
    [MaxLength(EntityValidationConstraints.Category.maxDescriptionStringLength)]
    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
