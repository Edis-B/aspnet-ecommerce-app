using System.Text.Json.Serialization;
using TechStoreApp.Web.ViewModels.Category;

namespace TechStoreApp.Web.ViewModels.Products
{
    public class AddProductViewModel
    {
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
        [JsonIgnore]
        public IEnumerable<CategoryViewModel>? Categories { get; set; }

    }
}
