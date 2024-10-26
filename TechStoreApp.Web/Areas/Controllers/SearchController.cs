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
        private readonly TechStoreDbContext context;

        public SearchController(TechStoreDbContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public IActionResult Search(string? query, string? category, int currentPage = 1)
        {
            var model = new SearchFormModel
            {
                Category = category,
                CurrentPage = currentPage,
                Query = query
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search([FromBody] SearchFormModel model)
        {
            var searchModel = new SearchFormModel
            {
                Category = model.Category,
                CurrentPage = model.CurrentPage,
                Query = model.Query
            };

            return View(searchModel);
        }
    }
}
