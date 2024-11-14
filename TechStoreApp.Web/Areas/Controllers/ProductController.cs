using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Common;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Products;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Web.Areas.Controllers
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

        [HttpGet]
        [Authorize(Roles = GeneralConstraints.AdminRoleName)]
        [Route("Product/Edit/{productId:int}")]
        public async Task<IActionResult> Edit(int productId)
        {
            var viewModel = await productService.GetEditProductViewModelAsync(productId);

            if (viewModel == null)
            {
                return View("Error");
            }

            return View("Edit", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = GeneralConstraints.AdminRoleName)]
        public async Task<IActionResult> EditPost(EditProductViewModel model)
        {
            await productService.EditProductAsync(model);

            return RedirectToAction(nameof(RedirectToDetails), new { productId = model.ProductId });
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var newViewModel = productService.GetAddProductViewModel();

            return View("Add", newViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel model)
        {
            int newProductId = await productService.AddProductAsync(model);

            return RedirectToAction(nameof(RedirectToDetails), new { productId = newProductId });
        }


        [HttpPost]
        public async Task<JsonResult> CreateReview([FromBody] ReviewFormModel model)
        {
            await productService.CreateAndAddReviewToDBAsync(model);

            return Json(new { message = "Successfully added" });
        }
    }
}
