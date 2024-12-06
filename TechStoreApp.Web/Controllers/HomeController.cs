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
        public IActionResult Error(ErrorViewModel model = default!)
        {
            var data = TempDataUtility.GetTempData<ErrorViewModel>(TempData, "ErrorViewModel");

            if (data != null)
            {
                model = data;
            }

            model.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            return View("Error", model);
        }
    }
}
