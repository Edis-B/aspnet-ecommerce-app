using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Models.Models;

namespace TechStoreApp.Data.Models;

public partial class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int ProductId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string? ImageUrl { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; }
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Favorited> Favorites { get; set; } = new List<Favorited>();
}
