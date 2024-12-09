using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.ApiViewModels.Orders;
using TechStoreApp.Web.ViewModels.Orders;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IOrderService
    {
        Task<OrderPageViewModel> GetOrderViewModelAsync(int? addressId);
        Task<OrderFinalizedPageViewModel> GetOrderFinalizedModelAsync(OrderPageViewModel model);
        Task<bool> SendOrderAsync(OrderFinalizedPageViewModel model);
        Task<UserOrdersListViewModel> GetUserOrdersListViewModelAsync(string? userId = null);
        Task<UserOrderSingleViewModel> GetDetailsOfOrder(int orderId);
        Task<bool> PayForOrder(int orderId);

        // Api
        Task<IEnumerable<OrderApiViewModel>> ApiGetAllOrders();
        Task<IEnumerable<OrderApiViewModel>> ApiGetAllOrdersFromQuery(string userId, string userName);

    }
}
