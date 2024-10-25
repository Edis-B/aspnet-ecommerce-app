using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStoreApp.Data.Models;

public partial class OrderDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderDetailId { get; set; }
    [Required]
    public int? OrderId { get; set; }
    [Required]
    public int? ProductId { get; set; }

    public int Quantity { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }
    [ForeignKey(nameof(OrderId))]
    public virtual Order? Order { get; set; }
    [ForeignKey(nameof(ProductId))]
    public virtual Product? Product { get; set; }
}
