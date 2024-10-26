using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using TechStoreApp.Data;
using TechStoreApp.Web.ViewModels.Header;
using TechStoreApp.Web.ViewModels.Products;
using TechStoreApp.Web.ViewModels.Search;

namespace TechStoreApp.Web.Views.Shared.Components
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly TechStoreDbContext context;
        public SearchViewComponent(TechStoreDbContext _context)
        {
            context = _context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string category, string query, int currentPage, int pageSize = 12)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var _favoritedForUser = context.Favorited
                .Where(f => f.UserId == userId);

            var productsQuery = context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                if (category != "All")
                {
                    productsQuery = productsQuery.Where(p => p.Category.Description == category);
                }
            }

            if (!string.IsNullOrEmpty(query))
            {
                productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(query.ToLower()));
            }

            int totalItems = productsQuery.Count();

            List<ResultViewModel> results = await productsQuery
               .Select(p => new ResultViewModel
               {
                   Product = new ProductViewModel()
                   {
                       ProductId = p.ProductId,
                       CategoryId = p.CategoryId,
                       Name = p.Name,
                       Description = p.Description,
                       Price = p.Price,
                       Stock = p.Stock,
                       ImageUrl = p.ImageUrl,
                   },
                   CheckedString = context.Favorited.FirstOrDefault(f => f.ProductId == p.ProductId && f.UserId == userId) != null ? "checked" : "unchecked"
               })
               .Skip((currentPage - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var model = new SearchFormModel
            {
                Query = query,
                Results = results,
                TotalPages = totalPages,
                CurrentPage = currentPage,
            };

            return View("FilterSearch", model);
        }
    }
}
