using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Models;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.ApiViewModels.Addresses;
using TechStoreApp.Web.ViewModels.Orders;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IAddressService
    {
        Task SaveAddressAsync(AddressFormModel model);
        Task<Address> GetAddressByIdAsync(int id);

        // Api
        Task<IEnumerable<AddressApiViewModel>> GetAddressesByUser(string userId);
    }
}
