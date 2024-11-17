using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TechStoreApp.Web.ViewModels;
using System.Security.Claims;
using TechStoreApp.Web.ViewModels.Search;
using TechStoreApp.Data;

namespace TechStoreApp.Web.Areas.Controllers
{
    public class SearchController : Controller
    {

        public SearchController()
        {

        }

        [HttpGet]
        public IActionResult Search(string? query, string? category, int currentPage = 1)
        {
            var model = new SearchViewModel
            {
                Category = category,
                CurrentPage = currentPage,
                Query = query
            };

            return View(model);
        }
    }
}
