using TechStoreApp.Web.ViewModels.Category;
using TechStoreApp.Web.ViewModels.Products;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Web.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }
        public List<ProductViewModel> FeaturedProducts { get; set; }
        public List<ReviewViewModel> CustomerReviews { get; set; } = new List<ReviewViewModel>();
    }
}
