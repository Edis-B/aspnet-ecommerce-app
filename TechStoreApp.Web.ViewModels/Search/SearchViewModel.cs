using System.Diagnostics.CodeAnalysis;
using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Web.ViewModels.Search
{
    public class SearchViewModel
    {
        [AllowNull]
        public string Category { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; } = 1;
        [AllowNull]
        public string Query { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
