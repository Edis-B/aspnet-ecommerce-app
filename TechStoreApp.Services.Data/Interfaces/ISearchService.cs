using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Web.ViewModels.Search;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface ISearchService
    {
        Task<SearchViewModel> GetSearchViewModel(string category, string query, int currentPage, int pageSize);
    }
}
