using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Orders;

namespace TechStoreApp.Services.Data
{
    public class AddressService : IAddressService
    {
        private readonly IRepository<Address, int> addressRepository;
        private readonly IUserService userService;
        public AddressService(IUserService _userService,
            IRepository<Address, int> _addressRepository)
        {
            userService = _userService;
            addressRepository = _addressRepository;
        }
        public async Task SaveAddressAsync(OrderViewModel model)
        {
            var userId = userService.GetUserId();

            var newAddress = new Address
            {
                UserId = userId,
                City = model.Address.City,
                Country = model.Address.Country,
                Details = model.Address.Details,
                PostalCode = model.Address.PostalCode,
            };

            await addressRepository.AddAsync(newAddress);
        }
    }
}
