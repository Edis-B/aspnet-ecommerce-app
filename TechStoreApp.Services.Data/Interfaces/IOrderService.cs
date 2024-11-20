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
        Task<OrderPageViewModel> GetOrderViewModelAsync();
        Task<OrderFinalizedPageViewModel> GetOrderFinalizedModelAsync(OrderPageViewModel model);
        Task<UserOrdersListViewModel> GetUserOrdersListViewModelAsync();
        Task<UserOrderSingleViewModel> GetDetailsOfOrder(int orderId);
        Task SendOrderAsync(SendOrderViewModel model);
    }
}
