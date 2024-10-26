using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Data.Models
{
    public class Favorited
    {
        [Key]
        public string UserId { get; set; }
        [Key]
        public int ProductId { get; set; }
        public DateTime FavoritedAt { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}
