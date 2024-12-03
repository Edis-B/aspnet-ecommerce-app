using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IReviewService reviewService;
        public ProductController(IProductService _productService,
            IReviewService _reviewService)
        {
            productService = _productService;
            reviewService = _reviewService;
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
            await reviewService.CreateAndAddReviewToDBAsync(model);

            return Json(new { message = "Successfully added" });
        }
    }
}
