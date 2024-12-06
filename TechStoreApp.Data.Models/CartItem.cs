using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TechStoreApp.Common.EntityValidationConstraints.CartItem;

namespace TechStoreApp.Data.Models;

public partial class CartItem
{
    [Key]
    public int CartId { get; set; }

    [Key]
    public int ProductId { get; set; }

    [Range(minQuantityCount, int.MaxValue)]
    public int Quantity { get; set; }

    [ForeignKey(nameof(CartId))]
    public virtual Cart? Cart { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product? Product { get; set; }
}
