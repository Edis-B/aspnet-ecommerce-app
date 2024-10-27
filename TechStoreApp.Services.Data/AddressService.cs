using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Orders;

namespace TechStoreApp.Services.Data
{
    public class AddressService : IAddressService
    {
        private readonly TechStoreDbContext context;
        private readonly IUserService userService;
        public AddressService(TechStoreDbContext _context, IUserService _userService)
        {
            context = _context;
            userService = _userService;

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

            await context.AddAsync(newAddress);
            await context.SaveChangesAsync();
        }
    }
}
