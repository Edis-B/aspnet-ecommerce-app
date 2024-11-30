using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.ApiViewModels.Orders;
using TechStoreApp.Web.ViewModels.Orders;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IOrderService
    {
        Task<OrderPageViewModel> GetOrderViewModelAsync(int? addressId);
        Task<OrderFinalizedPageViewModel> GetOrderFinalizedModelAsync(AddressFormModel model);
        Task SendOrderAsync(SendOrderViewModel model);
        Task<UserOrdersListViewModel> GetUserOrdersListViewModelAsync(string? userId = null);
        Task<UserOrderSingleViewModel> GetDetailsOfOrder(int orderId);

        // Api
        Task<IEnumerable<OrderApiViewModel>> GetAllOrders();
        Task<IEnumerable<OrderApiViewModel>> GetAllOrdersByUserId(string userId);

    }
}
