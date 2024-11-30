using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Web.ViewModels.ApiViewModels.Reviews
{
    public class ReviewApiViewModel
    {
        public int ProductId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public string Author { get; set; }
    }
}
