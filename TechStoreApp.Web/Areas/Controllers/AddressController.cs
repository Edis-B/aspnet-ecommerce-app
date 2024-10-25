using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechStoreApp.Data.Models;
using TechStoreApp.Data;
using TechStoreApp.Web.ViewModels.Orders;

namespace TechStoreApp.Web.Areas.Controllers
{
    public class AddressController : Controller
    {
        private readonly TechStoreDbContext context;
        public AddressController(TechStoreDbContext _context)
        {
            context = _context;
        }
        [HttpPost]
        public async Task<IActionResult> SaveAddress(OrderViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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

            return RedirectToAction("Order", "Order");
        }
    }
}
