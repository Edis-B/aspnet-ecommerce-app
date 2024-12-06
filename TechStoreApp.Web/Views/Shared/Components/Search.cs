using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Search;

namespace TechStoreApp.Web.Views.Shared.Components
{
    [ViewComponent]
    public class Search : ViewComponent
    {
        private readonly ISearchService searchService;
        public Search(ISearchService _searchService)
        {
            searchService = _searchService;
        }

        public async Task<IViewComponentResult> InvokeAsync(SearchViewModel model)
        {
            return View("FilterSearch", modelWithNewResults);
        }
    }
}
