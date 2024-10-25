using TechStoreApp.Data.Models.Models;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Web.ViewModels.Search
{
    public class SearchViewModel
    {
        public string Category { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public string Query { get; set; } = null!;
        public List<ResultViewModel> Results { get; set; }
    }
}
