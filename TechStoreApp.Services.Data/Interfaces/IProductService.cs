using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Web.ViewModels.Products;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IProductService
    {
        Task<ProductViewModel> GetProductViewModelAsync(int productId);
        Task<EditProductViewModel> GetEditProductViewModelAsync(int productId);
        Task EditProductAsync(EditProductViewModel model);
        Task<AddProductViewModel> GetAddProductViewModelAsync(int productId);
        Task<AddProductViewModel> AddProductAsync(AddProductViewModel model);
        Task CreateAndAddReviewToDBAsync(ReviewFormModel model);
    }
}
