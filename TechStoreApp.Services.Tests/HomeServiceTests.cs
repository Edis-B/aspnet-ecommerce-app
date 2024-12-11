using MockQueryable;
using NUnit.Framework;
using TechStoreApp.Services.Data;

namespace TechStoreApp.Services.Tests
{
    [TestFixture]
    public class HomeServiceTests : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            ResetTestData();
        }
        void InitializeHomeService()
        {
            // Initialize service
            homeService = new HomeService(
                mockProductRepository.Object,
                mockReviewRepository.Object,
                mockCategoryRepository.Object,
                mockUserService.Object
            );
        }

        [Test]
        public async Task GetHomeViewModel_ShouldReturnFeaturedProductsAndCategories()
        {
            // Arrange
            mockUserService.Setup(us => us.GetUserId()).Returns(userId);

            mockProductRepository.Setup(pr => pr.GetAllAttached())
                .Returns(testProducts.AsQueryable().BuildMock());

            mockCategoryRepository.Setup(cr => cr.GetAllAttached())
                .Returns(testCategories.AsQueryable().BuildMock());

            // Act
            InitializeHomeService();
            var homeViewModel = await homeService.GetHomeViewModel();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(homeViewModel.FeaturedProducts, Has.Count.EqualTo(1));
                Assert.That(homeViewModel.FeaturedProducts[0].ProductId, Is.EqualTo(2));
                
                Assert.That(homeViewModel.Categories, Has.Count.EqualTo(2)); 

                Assert.That(homeViewModel.Categories[0].Id, Is.EqualTo(1));
                Assert.That(homeViewModel.Categories[0].Description, Is.EqualTo("TestCategoryDescription1"));

                Assert.That(homeViewModel.Categories[1].Id, Is.EqualTo(2)); 
                Assert.That(homeViewModel.Categories[1].Description, Is.EqualTo("TestCategoryDescription2")); 
            });

        }

    }
}
