using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Products;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductsApiController(IProductService _productService)
        {
            productService = _productService;
        }

        [HttpGet("[action]")]
        [ProducesResponseType((typeof(IEnumerable<ProductViewModel>)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult GetAllProducts()
        {
            var products = productService.GetAllProducts();

            return Ok(products);
        }

        [HttpGet("[action]")]
        [ProducesResponseType((typeof(IEnumerable<ReviewViewModel>)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllReviewsForProduct(int productId)
        {
            var reviews = productService.GetProductReviews(productId);

            return Ok(reviews);
        }
    }
}
