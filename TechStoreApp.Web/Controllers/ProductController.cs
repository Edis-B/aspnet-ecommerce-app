using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }

        [Route("Product/RedirectToDetails/{productId:int}")]
        public async Task<IActionResult> RedirectToDetails(int productId)
        {
            var product = await productService.GetProductViewModelAsync(productId);

            return View("Product", product);
        }
    }
}
