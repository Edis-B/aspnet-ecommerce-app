using CinemaApp.Web.Infrastructure.Attributes;
using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.ApiViewModels.Products;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsApiController : ControllerBase
    {
        private const string action = "[action]";
        private readonly IProductService productService;
        public ProductsApiController(IProductService _productService)
        {
            productService = _productService;
        }

        [HttpGet(action)]
        [ProducesResponseType((typeof(IEnumerable<ProductApiViewModel>)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult GetAllProducts()
        {
            var products = productService.GetAllProducts();

            return Ok(products);
        }

        [HttpGet(action)]
        [ProducesResponseType((typeof(IEnumerable<ProductApiViewModel>)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult GetAllProductsByQuery(string? productName, int? categoryId)
        {
            var products = productService.GetAllProductsByQuery(productName, categoryId);

            return Ok(products);
        }

        //[HttpPost(action)]
        //[AdminCookieOnly]
        //[ProducesResponseType((typeof(IEnumerable<ProductApiViewModel>)), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]

        //public IActionResult AddNewProductToStore(string? productName, int? categoryId)
        //{
        //    var products = productService.GetAllProductsByQuery(productName, categoryId);

        //    return Ok(products);
        //}
    }
}
