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

        Task<CartViewModel> Cart();
        Task<JsonResult> IncreaseCount(CartFormModel model);
        Task<JsonResult> DecreaseCount(CartFormModel model);
        Task<JsonResult> GetCartItemsCountAsync();
        Task<JsonResult> RemoveFromCart(RemoveFromCartViewModel model);
        Task<JsonResult> ClearCart();
    }
}
