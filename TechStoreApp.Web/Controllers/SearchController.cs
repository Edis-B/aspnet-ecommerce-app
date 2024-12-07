using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Web.ViewModels.Search;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;
        public SearchController(ISearchService _searchService)
        {
            searchService = _searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? query, string? category, string? orderBy, int currentPage, int categoryId)
        {
            // Process and validate model
            var model = new SearchViewModel
            {
                Category = category ?? "All",
                CurrentPage = currentPage < 1 ? 1 : currentPage,
                Orderby = orderBy ?? "default",
                Query = query ?? null,
                CategoryId = categoryId
            };

            var modelWithNewResults = await searchService.GetSearchViewModel(model);

            return View(modelWithNewResults);
        }
    }
}
