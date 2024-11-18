using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Web.ViewModels.Search
{
    public class SearchFormModel
    {
        [AllowNull]
        public string Category { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; } = 1;
        [AllowNull]
        public string Query { get; set; }
        public string Orderby { get; set; }
    }
}
