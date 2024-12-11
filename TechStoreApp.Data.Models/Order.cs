using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TechStoreApp.Common.EntityValidationConstraints.Order;
using TechStoreApp.Data.Models;

namespace TechStoreApp.Data.Models;

public partial class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    public bool HasBeenPaidFor {  get; set; }

    [Required]
    public bool IsFinished { get; set; }

    [Required]
    public int PaymentTypeId { get; set; } = 1;

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(minTotalAmount, double.MaxValue)]
    public decimal TotalAmount { get; set; }

    [Required]
    [MinLength(minShippingAddressStringLength)]
    [MaxLength(maxShippingAddressStringLength)]
    public string? ShippingAddress { get; set; }

    public int StatusId { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }

    [ForeignKey(nameof(PaymentTypeId))]
    public virtual PaymentDetail PaymentDetail { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
