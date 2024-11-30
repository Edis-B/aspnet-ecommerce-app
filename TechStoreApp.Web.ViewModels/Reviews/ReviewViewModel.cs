using System.ComponentModel.DataAnnotations;

namespace TechStoreApp.Web.ViewModels.Reviews
{
    public class ReviewViewModel
    {
        public int ProductId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public string Author { get; set; }
    }
}
