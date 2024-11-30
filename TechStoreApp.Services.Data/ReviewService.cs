using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Services.Data
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review, int> reviewRepository;
        public ReviewService(IRepository<Review, int> _reviewRepository)
        {
            reviewRepository = _reviewRepository;
        }
        public IEnumerable<ReviewViewModel> GetProductReviews(int productId)
        {
            var result = reviewRepository.GetAllAttached()
                .Where(r => r.ProductId == productId)
                .Select(r => new ReviewViewModel()
                {
                    Comment = r.Comment,
                    ProductId = productId,
                    Author = r.User!.UserName!,
                    Rating = r.Rating,
                });

            return result;
        }
    }
}
