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
        Task<JsonResult> AddToCart(AddToCartViewModel model);

        Task<CartViewModel> Cart();
        Task<JsonResult> IncreaseCount(CartFormModel model);
        Task<JsonResult> DecreaseCount(CartFormModel model);
        Task<JsonResult> GetCartItemsCount();
        Task<JsonResult> RemoveFromCart(RemoveFromCartViewModel model);
        Task<JsonResult> ClearCart();
    }
}
