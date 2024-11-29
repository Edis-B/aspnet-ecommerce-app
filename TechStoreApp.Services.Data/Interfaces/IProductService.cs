using Microsoft.Data.SqlClient.DataClassification;
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
        Task<int> AddProductAsync(AddProductViewModel model);
        Task EditProductAsync(EditProductViewModel model);
        Task CreateAndAddReviewToDBAsync(ReviewFormModel model);
        AddProductViewModel GetAddProductViewModel();

        IEnumerable<ProductViewModel> GetAllProducts();
        IEnumerable<ReviewViewModel> GetProductReviews(int productId);
    }
}
