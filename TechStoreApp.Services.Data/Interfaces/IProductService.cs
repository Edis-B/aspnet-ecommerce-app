using Microsoft.Data.SqlClient.DataClassification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Web.ViewModels.ApiViewModels.Products;
using TechStoreApp.Web.ViewModels.Products;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IProductService
    {
        Task<ProductViewModel> GetProductViewModelAsync(int productId);

        // Edit product
        Task<EditProductViewModel> GetEditProductViewModelAsync(int productId);
        Task EditProductAsync(EditProductViewModel model);

        // Add product
        AddProductViewModel GetAddProductViewModel();
        Task<int> AddProductAsync(AddProductViewModel model);

        
        // Api
        IEnumerable<ProductApiViewModel> GetAllProducts();
        IEnumerable<ProductApiViewModel> GetAllProductsByQuery(string? productName, int? categoryId);
    }
}
