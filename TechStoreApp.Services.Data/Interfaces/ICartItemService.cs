using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface ICartItemService
    {
        Task<JsonResult> IncreaseCountAsync(ProductIdFormModel model);
        Task<JsonResult> DecreaseCountAsync(ProductIdFormModel model);
        Task<JsonResult> GetCartItemsCountAsync();
        Task<JsonResult> RemoveFromCartAsync(ProductIdFormModel model);
    }
}
