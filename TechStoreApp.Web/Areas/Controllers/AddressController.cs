using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechStoreApp.Data.Models;
using TechStoreApp.Data;
using TechStoreApp.Web.ViewModels.Orders;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Web.Areas.Controllers
{
    public class AddressController : Controller
    {
        private readonly TechStoreDbContext context;
        private readonly IAddressService addressService;
        public AddressController(TechStoreDbContext _context, IAddressService _addressService)
        {
            addressService = _addressService;
            context = _context;
        }
        [HttpPost]
        public async Task<IActionResult> SaveAddress(OrderViewModel model)
        {
            await addressService.SaveAddressAsync(model);

            return RedirectToAction("Order", "Order");
        }
    }
}
