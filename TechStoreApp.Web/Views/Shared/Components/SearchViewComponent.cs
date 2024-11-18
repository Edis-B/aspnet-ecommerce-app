using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Header;
using TechStoreApp.Web.ViewModels.Products;
using TechStoreApp.Web.ViewModels.Search;

namespace TechStoreApp.Web.Views.Shared.Components
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly ISearchService searchService;
        public SearchViewComponent(ISearchService _searchService)
        {
            searchService = _searchService;
        }

        public async Task<IViewComponentResult> InvokeAsync(SearchViewModel model)
        {
            var modelWithNewResults = await searchService.GetSearchViewModel(model);

            return View("FilterSearch", modelWithNewResults);
        }
    }
}
