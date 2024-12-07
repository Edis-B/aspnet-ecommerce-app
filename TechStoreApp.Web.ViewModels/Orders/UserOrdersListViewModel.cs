using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Web.ViewModels.Orders
{
    public class UserOrdersListViewModel
    {
        public List<UserOrderSingleViewModel> Orders { get; set; }
    }
}
