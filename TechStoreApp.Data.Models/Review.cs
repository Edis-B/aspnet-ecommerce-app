using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechStoreApp.Data.Models;

namespace TechStoreApp.Data.Models;

public partial class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReviewId { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public int Rating { get; set; }
    [Required]
    [MinLength(10)]
    public string Comment { get; set; }

    public DateTime ReviewDate { get; set; }
    [ForeignKey(nameof(ProductId))]
    public virtual Product? Product { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? User { get; set; }
}
