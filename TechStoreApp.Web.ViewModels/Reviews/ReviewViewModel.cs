using System.ComponentModel.DataAnnotations;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Web.ViewModels.Reviews
{
    public class ReviewViewModel
    {
        public int ProductId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public UserViewModel Author { get; set; }
    }
}
