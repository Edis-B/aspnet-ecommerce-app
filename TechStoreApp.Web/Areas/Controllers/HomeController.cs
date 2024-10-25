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
        private readonly TechStoreDbContext context;

        public HomeController(ILogger<HomeController> logger, TechStoreDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(SeedDataCategories.GetCategories());
                context.SaveChanges();

            }
            if (!context.Products.Any())
            {
                context.Products.AddRange(SeedDataProducts.GetProducts());
                context.SaveChanges();
            }
            if (!context.Statuses.Any())
            {
                context.Statuses.AddRange(SeedDataStatuses.GetStatuses());
                context.SaveChanges();
            }

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
