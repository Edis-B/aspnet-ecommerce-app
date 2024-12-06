using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TechStoreApp.Common.EntityValidationConstraints.PaymentDetail;
namespace TechStoreApp.Data.Models
{
    public class PaymentDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }
        [Required]
        [MinLength(minPaymentTypeStringLength)]
        [MaxLength(maxPaymentTypeStringLength)]
        public string PaymentType { get; set; }
        [MinLength(minDescriptionTypeStringLength)]
        [MaxLength(maxDescriptionTypeStringLength)]
        public string Description { get; set; }

    }
}
