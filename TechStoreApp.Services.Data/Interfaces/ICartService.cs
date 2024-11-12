using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Web.ViewModels;
using TechStoreApp.Web.ViewModels.Cart;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface ICartService
    {
        Task<JsonResult> AddToCartAsync(AddToCartViewModel model);
        Task<CartViewModel> GetCartItemsAsync();
        Task<JsonResult> IncreaseCountAsync(CartFormModel model);
        Task<JsonResult> DecreaseCountAsync(CartFormModel model);
        Task<JsonResult> GetCartItemsCountAsync();
        Task<JsonResult> RemoveFromCartAsync(RemoveFromCartViewModel model);
        Task<JsonResult> ClearCartAsync();
    }
}
