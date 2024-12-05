using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TechStoreApp.Web.ViewModels;
using System.Security.Claims;
using TechStoreApp.Web.ViewModels.Search;
using TechStoreApp.Data;

namespace TechStoreApp.Web.Controllers
{
    public class SearchController : Controller
    {

        public SearchController()
        {

        }

        [HttpGet]
        public IActionResult Search(string? query, string? category, string? orderBy, int currentPage)
        {
            var model = new SearchViewModel
            {
                Category = category ?? "All",
                CurrentPage = currentPage < 1 ? 1 : currentPage,
                Orderby = orderBy ?? "default",
                Query = query ?? null
            };

            return View(model);
        }
    }
}
