using CinemaApp.Web.Infrastructure.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.ApiViewModels.Addresses;

namespace TechStoreApp.WebAPI.Controllers
{
    public class AddressesApiController : Controller
    {
        private const string action = "[action]";
        private readonly IAddressService addressService;
        private readonly IUserService userService;
        public AddressesApiController(IAddressService _addressService, IUserService _userService)
        {
            addressService = _addressService;
            userService = _userService;
        }

        [AdminCookieOnly]
        [HttpGet(action)]
        [ProducesResponseType((typeof(IEnumerable<AddressApiViewModel>)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserAddressesByTheirId(string userId) 
        {
            if (!await userService.DoesUserExistId(userId))
            {
                return NotFound("User with such id does not exist");
            }

            var results = await addressService.ApiGetAddressesByUserId(userId);

            if (!results.Any())
            {
                return NotFound("User has no orders");
            }

            return Ok(results);
        }
    }
}
