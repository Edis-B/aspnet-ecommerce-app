using TechStoreApp.Web.Models;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Web.ViewModels.Products
{
    public class ProductViewModel
    {
        public List<ReviewViewModel> Reviews { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
        public string CheckedString { get; set; }
        public ReviewFormModel Review { get; set; }

    }
}
