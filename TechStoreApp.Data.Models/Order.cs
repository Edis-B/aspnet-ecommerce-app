using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Models.Models;

namespace TechStoreApp.Data.Models;

public partial class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int OrderId { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public DateTime OrderDate { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }
    [Required]
    public string? ShippingAddress { get; set; }

    public int StatusId { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; }
}
