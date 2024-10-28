using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.Orders;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Web.ViewModels.Header
{
    public class UserModel
    {
        public ProfileViewModel User { get; set; }
        public List<OrderViewModel> MyProperty { get; set; }
    }
}
