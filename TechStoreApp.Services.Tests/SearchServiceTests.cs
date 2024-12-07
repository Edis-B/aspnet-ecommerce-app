using MockQueryable;
using Moq;
using NUnit.Framework;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Search;

namespace TechStoreApp.Services.Tests
{
    public class SearchServiceTests
    {
        private SearchService searchService;
        private Mock<IRepository<Product, int>> mockProductRepository;
        private Mock<IRepository<Favorited, object>> mockFavoritedRepository;
        private Mock<IRepository<Category, int>> mockCategoryRepository;
        private Mock<IUserService> mockUserService;

        private List<Product> testProducts;
        private List<Favorited> testFavorites;
        private Guid userId;

        [SetUp]
        public void SetUp()
        {
            // Initialize mocks and test data
            mockProductRepository = new Mock<IRepository<Product, int>>();
            mockFavoritedRepository = new Mock<IRepository<Favorited, object>>();
            mockCategoryRepository = new Mock<IRepository<Category, int>>();
            mockUserService = new Mock<IUserService>();

            userId = Guid.NewGuid();

            testProducts = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "Product 1",
                    Category = new Category { Description = "Computer" },
                    Price = 100,
                    Stock = 10,
                    ImageUrl = "product1.jpg"
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Product 2",
                    Category = new Category { Description = "HardDisks" },
                    Price = 50,
                    Stock = 20,
                    ImageUrl = "product2.jpg"
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Product 3",
                    Category = new Category { Description = "Computer" },
                    Price = 150,
                    Stock = 5,
                    ImageUrl = "product3.jpg"
                }
            };

            testFavorites = new List<Favorited>
            {
                new Favorited { ProductId = 1, UserId = userId },
                new Favorited { ProductId = 2, UserId = userId }
            };

            mockUserService
                .Setup(us => us.GetUserId())
                .Returns(userId);

            mockProductRepository
                .Setup(r => r.GetAllAttached())
                .Returns(testProducts.AsQueryable().BuildMock());

            mockFavoritedRepository
                .Setup(r => r.GetAllAttached())
                .Returns(testFavorites.AsQueryable().BuildMock());

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
                Category = "Computer",
                Orderby = "default",
                Query = ""
            };

            // Act
            InitializeSearchService();
            var result = await searchService.GetSearchViewModel(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Products, Has.Count.EqualTo(2));
                Assert.That(result.Products[0].CategoryId, Is.EqualTo(testProducts[0].CategoryId));
                Assert.That(result.Products[1].CategoryId, Is.EqualTo(testProducts[2].CategoryId));
            });
        }
    }
}
