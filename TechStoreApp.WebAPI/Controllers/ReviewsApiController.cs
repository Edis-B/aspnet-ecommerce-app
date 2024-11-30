using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.ApiViewModels.Reviews;

namespace TechStoreApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewsApiController : ControllerBase
    {
        private const string action = "[action]";
        private readonly IReviewService reviewService;
        public ReviewsApiController(IReviewService _reviewService)
        {
            reviewService = _reviewService;
        }

        [HttpGet(action)]
        [ProducesResponseType((typeof(IEnumerable<ReviewApiViewModel>)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllReviewsForProduct(int productId)
        {
            var reviews = reviewService.GetProductReviews(productId);

            if (!reviews.Any())
            {
                return Ok("Products doesn't have any reviews yet.");
            }

            return Ok(reviews);
        }
    }
}
