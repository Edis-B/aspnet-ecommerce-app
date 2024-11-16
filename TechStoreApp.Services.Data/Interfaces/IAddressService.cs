using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.Orders;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IAddressService
    {
        Task SaveAddressAsync(OrderViewModel model);

        Task<Address> GetAddressByIdAsync(int id);
    }
}
