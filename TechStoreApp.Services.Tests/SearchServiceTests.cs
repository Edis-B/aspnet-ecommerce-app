using MockQueryable;
using NUnit.Framework;
using TechStoreApp.Services.Data;
using TechStoreApp.Web.ViewModels.Search;

namespace TechStoreApp.Services.Tests
{
    public class SearchServiceTests : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            ResetTestData();

            mockUserService
                .Setup(us => us.GetUserId())
                .Returns(userId);
        }

        void InitializeSearchService()
        {
            searchService = new SearchService(
                mockProductRepository.Object,
                mockFavoritedRepository.Object,
                mockCategoryRepository.Object,
                mockUserService.Object
            );
        }

        [Test]
        public async Task GetSearchViewModel_ShouldFilterByCategory()
        {
            // Arrange
            var model = new SearchViewModel
            {
                CurrentPage = 1,
                Category = "Category1", // Matches testProducts[0]'s category
                Orderby = "default",
                Query = ""
            };

            mockProductRepository
                .Setup(r => r.GetAllAttached())
                .Returns(testProducts.AsQueryable().BuildMock());

            mockCategoryRepository
                .Setup(r => r.GetById(1))
                .Returns(testCategories.Where(c => c.CategoryId == 1).First());

            // Act
            InitializeSearchService();
            var result = await searchService.GetSearchViewModel(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Products, Has.Count.EqualTo(1));
                Assert.That(result.Products[0].CategoryId, Is.EqualTo(testProducts[0].Category.CategoryId));
                Assert.That(result.Products[0].Name, Is.EqualTo(testProducts[0].Name));
            });
        }

    }
}
