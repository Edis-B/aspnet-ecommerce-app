using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data;
using TechStoreApp.Services.Data.Interfaces;
using static TechStoreApp.Common.GeneralConstraints;
namespace TechStoreApp.Web.Areas.Admin.Controllers
{
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class StatusController : Controller
    {
        private readonly IStatusService statusService;
        public StatusController(IStatusService _statusService)
        {
            statusService = _statusService;
        }
        [HttpPost]
        public async Task<IActionResult> EditStatusOfOrder(int orderId, int statusId)
        {
            var result = await statusService.EditStatusOfOrder(orderId, statusId);

            if (!result)
            {
                return View("Error");
            }

            return RedirectToAction("CompletedOrder", "Order", new { orderId = orderId });
        }
    }
}
