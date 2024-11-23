using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechStoreApp.Data.Models;
using TechStoreApp.Data;
using TechStoreApp.Web.ViewModels.Orders;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Web.ViewModels.Address;
using Newtonsoft.Json;
using TechStoreApp.Web.Utilities;

namespace TechStoreApp.Web.Areas.Controllers
{
    public class AddressController : Controller
    {
        private readonly IAddressService addressService;
        public AddressController(IAddressService _addressService)
        {
            addressService = _addressService;
        }
        [HttpPost]
        public async Task<IActionResult> SaveAddress(AddressFormModel model)
        {
            // TempData from SharedForm
            model = TempDataUtility.GetTempData<AddressFormModel>(TempData, "Model") ?? model;

            await addressService.SaveAddressAsync(model);

            return RedirectToAction("Order", "Order");
        }

        [HttpGet("Address/GetAddress/{id:int}")]
        public async Task<IActionResult> GetAddress(int id)
        {
            var result = await addressService.GetAddressByIdAsync(id);

            return Ok(result);
        }
    }
}
