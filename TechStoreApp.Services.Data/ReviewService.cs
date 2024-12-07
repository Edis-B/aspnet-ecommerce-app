using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Reviews;
using TechStoreApp.Web.ViewModels.User;

namespace TechStoreApp.Services.Data
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review, int> reviewRepository;
        private readonly IUserService userService;
        public ReviewService(IRepository<Review, int> _reviewRepository, 
            IUserService _userService)
        {
            reviewRepository = _reviewRepository;
            userService = _userService;

        }

        public async Task CreateAndAddReviewToDBAsync(ReviewFormModel model)
        {
            var userId = userService.GetUserId();

            var newReview = new Review
            {
                ReviewDate = DateTime.Now,
                Rating = model.Rating,
                ProductId = model.ProductId,
                Comment = model.Comment,
                UserId = userId
            };

            await reviewRepository.AddAsync(newReview);
        }
        public IEnumerable<ReviewViewModel> ApiGetProductReviews(int productId)
        {
            var result = reviewRepository.GetAllAttached()
                .Where(r => r.ProductId == productId)
                .Select(r => new ReviewViewModel()
                {
                    Comment = r.Comment,
                    ProductId = productId,
                    Rating = r.Rating,
                    Author = new UserViewModel()
                    {
                        Name = r.User!.UserName!,
                        PictureUrl = r.User.ProfilePictureUrl!
                    }
                });

            return result;
        }
    }
}
