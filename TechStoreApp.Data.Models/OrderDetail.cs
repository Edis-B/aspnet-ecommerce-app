using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechStoreApp.Common;

namespace TechStoreApp.Data.Models;

public partial class OrderDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderDetailId { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Range(EntityValidationConstraints.OrderDetail.minQuantityCount /*0*/, int.MaxValue)]
    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    [Range(EntityValidationConstraints.OrderDetail.minUnitPrice /*0*/, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    [ForeignKey(nameof(OrderId))]
    public virtual Order? Order { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product? Product { get; set; }
}
