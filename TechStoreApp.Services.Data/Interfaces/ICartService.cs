using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Web.ViewModels;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.Cart;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface ICartService
    {
        Task<JsonResult> AddToCartAsync(ProductIdFormModel model);
        Task<CartViewModel> GetCartItemsAsync();
        Task<JsonResult> IncreaseCountAsync(ProductIdFormModel model);
        Task<JsonResult> DecreaseCountAsync(ProductIdFormModel model);
        Task<JsonResult> GetCartItemsCountAsync();
        Task<JsonResult> RemoveFromCartAsync(ProductIdFormModel model);
        Task<JsonResult> ClearCartAsync();
    }
}
