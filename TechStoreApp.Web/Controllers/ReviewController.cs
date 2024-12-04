using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Web.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService reviewService;
        public ReviewController(IReviewService _reviewService)
        {
            reviewService = _reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("RedirectToDetails", "Product", new { productId = model.ProductId });
            }

            await reviewService.CreateAndAddReviewToDBAsync(model);

            return RedirectToAction("RedirectToDetails", "Product", new { productId = model.ProductId });
        }
    }
}
