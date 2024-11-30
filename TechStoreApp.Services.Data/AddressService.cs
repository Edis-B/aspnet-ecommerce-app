using Microsoft.EntityFrameworkCore;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.ApiViewModels.Addresses;

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

        public async Task SaveAddressAsync(AddressFormModel model)
        {
            var userId = userService.GetUserId();

            var newAddress = new Address
            {
                UserId = userId,
                City = model.City,
                Country = model.Country,
                Details = model.Details,
                PostalCode = int.Parse(model.PostalCode),
            };

            await addressRepository.AddAsync(newAddress);
        }

        public async Task<Address> GetAddressByIdAsync(int id)
        {
            var address = await addressRepository.GetByIdAsync(id);

            if (address.UserId != userService.GetUserId())
            {
                return null!;
            }

            return address;
        }

        public async Task<IEnumerable<AddressApiViewModel>> GetAddressesByUser(string userId)
        {

            var addresses = await addressRepository
                .GetAllAttached()
                .Where(a => a.UserId.ToString() == userId)
                .Select(a => new AddressApiViewModel()
                {
                    City = a.City,
                    Country = a.Country,    
                    PostalCode= a.PostalCode,
                    Details = a.Details,
                    UserId = a.UserId.ToString(),
                    UserName = a.User.UserName!
                })
                .ToListAsync();

            return addresses;
        }
    }
}
