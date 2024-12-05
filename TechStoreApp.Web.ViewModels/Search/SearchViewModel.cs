using System.Diagnostics.CodeAnalysis;
using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Web.ViewModels.Search
{
    public class SearchViewModel
    {
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int ProductsPerPage { get; set; } = 12;
        public int CategoryId { get; set; }

        [AllowNull]
        public string Category { get; set; } = "All";
        [AllowNull]
        public string Query { get; set; } = null!;
        [AllowNull]
        public string Orderby { get; set; } = "default";

        public List<ProductViewModel> Products { get; set; }

        public List<(string Text, string OrderBy)> SortOptions { get; set; } = new()
        {
            ("By default", "default"),
            ("Price descending", "priceDesc"),
            ("Price ascending", "priceAsc"),
            ("Likes descending", "likesDesc"),
        };

    }
}
