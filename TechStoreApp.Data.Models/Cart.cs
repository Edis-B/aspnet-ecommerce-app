using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechStoreApp.Data.Models;

namespace TechStoreApp.Data.Models;

public partial class Cart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CartId { get; set; }
    [Required]
    public string UserId { get; set; }

    public DateTime UpdateDate { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public virtual ApplicationUser User { get; set; }
}
