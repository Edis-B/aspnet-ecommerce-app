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

            mockProductRepository
                .Setup(r => r.GetAllAttached())
                .Returns(testProducts.AsQueryable().BuildMock());

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
                Category = "TestCategoryDescription1", // Matches testProducts[0]'s category
                Orderby = "default",
                Query = ""
            };

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

        [Test]
        public async Task GetSearchViewModel_ShouldSortByPriceAscending()
        {
            // Arrange
            var model = new SearchViewModel
            {
                CurrentPage = 1,
                Category = "",
                Orderby = "price_asc", // Order by ascending price
                Query = ""
            };

            // Act
            InitializeSearchService();
            var result = await searchService.GetSearchViewModel(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Products[0].Price, Is.LessThan(result.Products[1].Price)); // First product is cheaper than the second one
            });
        }
        [Test]
        public async Task GetSearchViewModel_ShouldReturnEmptyResultsForNoMatch()
        {
            // Arrange
            var model = new SearchViewModel
            {
                CurrentPage = 1,
                Category = "NonExistentCategory", // No products belong to this category
                Orderby = "default",
                Query = "NonExistentProduct" // No products match this search term
            };
            // Act
            InitializeSearchService();
            var result = await searchService.GetSearchViewModel(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Products, Has.Count.EqualTo(0)); // No products match the criteria
            });
        }


    }
}
