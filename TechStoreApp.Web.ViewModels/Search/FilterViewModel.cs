using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Web.ViewModels.Search
{
    public class FilterViewModel
    {
        public int ProductsPerPage { get; set; } = 12;
        public int CategoryId { get; set; } = 0;
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Category { get; set; } = "All";
        public string Query { get; set; } = null!;
        public string Orderby { get; set; } = "default";
        public List<(string Text, string OrderBy)> SortOptions { get; set; } = new()
        {
            ("By default", "default"),
            ("Price descending", "priceDesc"),
            ("Price ascending", "priceAsc"),
            ("Likes descending", "likesDesc"),
        };
    }
}
