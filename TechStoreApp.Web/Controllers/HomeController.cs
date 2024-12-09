using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.Infrastructure;
using TechStoreApp.Web.ViewModels;

namespace TechStoreApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService _homeService)
        {
            homeService = _homeService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await homeService.GetHomeViewModel();

            return View(model);
        }
        public IActionResult Error(ErrorViewModel? model)
        {
            model = TempDataUtility.GetTempData<ErrorViewModel>(TempData, "ErrorViewModel") ?? new ErrorViewModel();

            model.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            model.StatusCode = HttpContext.Response.StatusCode == default ? model.StatusCode : HttpContext.Response.StatusCode;

            return View("Error", model);
        }
    }
}
