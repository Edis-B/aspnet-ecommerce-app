using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.Infrastructure;

namespace TechStoreApp.Web.Controllers
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

            if (model == null) return View("Error", default);

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
