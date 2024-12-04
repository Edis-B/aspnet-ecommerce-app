using Moq;
using NUnit.Framework;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Reviews;

namespace TechStoreApp.Services.Tests
{
    public class ReviewServiceTests
    {
        private ReviewService reviewService;
        private Mock<IRepository<Review, int>> mockReviewRepository;
        private Mock<IUserService> mockUserService;

        private List<Review> testReviews;
        private Guid userId;

        [SetUp]
        public void SetUp()
        {
            userId = Guid.NewGuid();
            mockReviewRepository = new Mock<IRepository<Review, int>>();
            mockUserService = new Mock<IUserService>();


            testReviews = new List<Review>
            {
                new Review
                {
                    ReviewDate = DateTime.Now,
                    Rating = 5,
                    ProductId = 1,
                    Comment = "Great product!",
                    UserId = userId,
                    User = new ApplicationUser { Id = userId, UserName = "User1" }
                },
                new Review
                {
                    ReviewDate = DateTime.Now,
                    Rating = 4,
                    ProductId = 1,
                    Comment = "Good quality.",
                    UserId = userId,
                    User = new ApplicationUser { Id = userId, UserName = "User2" }
                }
            };

            mockUserService
                .Setup(us => us.GetUserId())
                .Returns(userId);

            mockReviewRepository
                .Setup(r => r.GetAllAttached())
                .Returns(testReviews.AsQueryable());

        }


        void InitializeReviewService()
        {
            reviewService = new ReviewService(mockReviewRepository.Object, mockUserService.Object);
        }
        

        [Test]
        public async Task CreateAndAddReviewToDBAsync_ShouldCreateAndAddReview()
        {
            // Arrange
            var model = new ReviewFormModel
            {
                ProductId = 1,
                Rating = 5,
                Comment = "Excellent product!"
            };

            // Act
            InitializeReviewService();
            await reviewService.CreateAndAddReviewToDBAsync(model);

            // Assert
            mockReviewRepository.Verify(r => r.AddAsync(It.Is<Review>(r =>
                r.ProductId == model.ProductId &&
                r.Rating == model.Rating &&
                r.Comment == model.Comment &&
                r.UserId == userId)), Times.Once);
        }

        [Test]
        public void GetProductReviews_ShouldReturnCorrectReviews()
        {
            // Arrange
            int productId = 1;

            // Act
            InitializeReviewService();
            var result = reviewService.ApiGetProductReviews(productId).ToList();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2));
                Assert.That(result[0].ProductId, Is.EqualTo(productId));
                Assert.That(result[0].Author, Is.EqualTo("User1"));
                Assert.That(result[0].Rating, Is.EqualTo(5));
                Assert.That(result[0].Comment, Is.EqualTo("Great product!"));

                Assert.That(result[1].ProductId, Is.EqualTo(productId));
                Assert.That(result[1].Author, Is.EqualTo("User2"));
                Assert.That(result[1].Rating, Is.EqualTo(4));
                Assert.That(result[1].Comment, Is.EqualTo("Good quality."));
            });
        }
    }
}
