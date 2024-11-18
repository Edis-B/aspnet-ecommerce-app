using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.Orders;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IOrderService
    {
        Task<OrderViewModel> GetOrderViewModelAsync();
        Task<OrderFinalizedModel> GetOrderFinalizedModelAsync(OrderViewModel model);
        Task<UserOrdersListViewModel> GetUserOrdersListViewModelAsync();
        Task SendOrderAsync(SendOrderViewModel model);
    }
}
