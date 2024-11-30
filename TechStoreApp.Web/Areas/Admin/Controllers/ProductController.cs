using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Products;
using static TechStoreApp.Common.GeneralConstraints;

namespace TechStoreApp.Web.Areas.Admin.Controllers
{
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        public ProductController(IProductService _productService)
        {

            productService = _productService;

        }
        [HttpGet]
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
        public async Task<IActionResult> EditPost(EditProductViewModel model)
        {
            await productService.EditProductAsync(model);

            return RedirectToAction("RedirectToDetails", "Product", new { area = "", productId = model.ProductId });
        }

        [HttpGet]
        public IActionResult Add()
        {
            var newViewModel = productService.GetAddProductViewModel();

            return View("Add", newViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel model)
        {
            int newProductId = await productService.AddProductAsync(model);

            return RedirectToAction("RedirectToDetails", "Product", new { area = "", productId = newProductId });
        }
    }
}
