using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Common;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Products;
using TechStoreApp.Web.ViewModels.Reviews;
using static TechStoreApp.Common.GeneralConstraints;
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

        [HttpPost]
        public async Task<JsonResult> CreateReview([FromBody] ReviewFormModel model)
        {
            await productService.CreateAndAddReviewToDBAsync(model);

            return Json(new { message = "Successfully added" });
        }
    }
}
