using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechStoreApp.Data;
using TechStoreApp.Data.Data;
using TechStoreApp.Web.ViewModels;

namespace TechStoreApp.Web.Areas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Favorites()
        {
            return View();
        }

        public IActionResult Register()
        {
            return Redirect("../Identity/Account/Register");
        }

        public IActionResult Login()
        {
            return Redirect("../Identity/Account/Login");
        }
        public IActionResult Logout()
        {
            return Redirect("../Identity/Account/Logout");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
