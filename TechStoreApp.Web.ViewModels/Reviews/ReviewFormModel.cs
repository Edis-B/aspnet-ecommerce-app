using System.ComponentModel.DataAnnotations;

namespace TechStoreApp.Web.ViewModels.Reviews
{
    public class ReviewFormModel
    {
        [MinLength(10, ErrorMessage = "Comment must be at least 10 characters long.")]
        [Required(ErrorMessage = "Comment must be at least 10 characters long.")]
        public string Comment { get; set; }
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Rating is required")]
        public int Rating { get; set; }
    }
}
