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
        public async Task<IActionResult> Search(string? query, string? category, int currentPage = 1)
        {
            var model = new SearchViewModel
            {
                Category = category,
                CurrentPage = currentPage,
                Query = query
            };

            return View(model);
        }
        [HttpPost]
        public JsonResult SearchByCategory([FromBody] SearchCategoryViewModel request)
        {
            string url = Url.Action("Search", new { query = request.Query, category = request.CategoryId, currentPage = 1 });
            return Json(new { redirectUrl = url });
        }
    }


}
