using Moq;
using NUnit.Framework;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Web.ViewModels.Products;
using Microsoft.AspNetCore.Routing.Constraints;
using MockQueryable;

namespace TechStoreApp.Services.Tests
{
    [TestFixture]
    public class FavoriteServiceTests
    {
        private Guid userId;
        private Mock<IRepository<Favorited, object>> mockFavoritedRepository;
        private Mock<IUserService> mockUserService;
        private FavoriteService favoriteService;

        private Product testProductSeperate;
        private List<Product> testProducts;
        private List<CartItem> testCartItems;
        private List<Cart> testCarts;
        private List<ApplicationUser> testUsers;
        private List<Favorited> testFavorited;

        void InitializeCartItemService()
        {
            favoriteService = new FavoriteService(mockFavoritedRepository.Object,
                mockUserService.Object);
        }

        [SetUp]
        public void Setup()
        {
            userId = Guid.NewGuid();
            mockUserService = new Mock<IUserService>();
            mockUserService
                .Setup(x => x.GetUserId())
                .Returns(userId);

            mockFavoritedRepository = new Mock<IRepository<Favorited, object>>();

            // Data
            testProductSeperate = new Product()
            {
                ProductId = 3,
                CategoryId = 3,
                Name = "TestName3",
                Description = "TestDescription3",
                Price = 3,
                Stock = 30,
                ImageUrl = "TestImage3"
            };

            testProducts = new List<Product>() {
                new Product()
                {
                    ProductId = 1,
                    CategoryId = 1,
                    Name = "TestName1",
                    Description = "TestDescription1",
                    Price = 1,
                    Stock = 10,
                    ImageUrl = "TestImage1"
                },
                new Product()
                {
                    ProductId = 2,
                    CategoryId = 2,
                    Name = "TestName2",
                    Description = "TestDescription2",
                    Price = 2,
                    Stock = 20,
                    ImageUrl = "TestImage2"
                }
            };

            testCartItems = new List<CartItem>() {
                new CartItem()
                {
                    CartId = 1,
                    ProductId = 1,
                    Quantity = 2,
                    Product = testProducts[0],
                },
                new CartItem()
                {
                    CartId = 1,
                    ProductId = 2,
                    Quantity = 2,
                    Product = testProducts[1],
                }
            };

            testCarts = new List<Cart> {
                new Cart()
                {
                    CartId = 1,
                    UserId = userId,
                    CartItems = testCartItems
                }
            };

            testUsers = new List<ApplicationUser>() {
                new ApplicationUser()
                {
                    Id = userId,
                    Cart = testCarts[0]
                }
            };

            testFavorited = new List<Favorited>() {
                new Favorited()
                {
                    UserId = userId,
                    ProductId = testProducts[0].ProductId,
                    Product = testProducts[0],
                    User = testUsers[0]
                }
            };

            testCartItems[0].Cart = testCarts[0];
            testCartItems[1].Cart = testCarts[0];
        }

        [Test]
        public async Task AddToFavoritesAsync_ShouldAddProductToFavorites()
        {
            // Arrange
            int productIdToAdd = testProducts[1].ProductId;

            // Act
            InitializeCartItemService();
            var result = await favoriteService.AddToFavoritesAsync(new ProductIdFormModel() { ProductId = productIdToAdd });

            // Assert
            mockFavoritedRepository.Verify(fr => fr.AddAsync(It.Is<Favorited>(f => 
                f.ProductId == productIdToAdd &&
                f.UserId == userId
                )), Times.Once);
        }

        [Test]
        public async Task RemoveFromFavoritesAsync_ShouldRemoveProductFromFavorites()
        {
            // Arrange
            var productIdToRemove = testProducts[0].ProductId;

            // Act
            InitializeCartItemService();
            var result = await favoriteService.RemoveFromFavoritesAsync(new ProductIdFormModel { ProductId = productIdToRemove });

            // Assert
            mockFavoritedRepository.Verify(fr => fr.DeleteAsync(userId, productIdToRemove), Times.Once);
        }

        [Test]
        public async Task GetUserFavoritesAsync_ShouldReturnUserFavoriteProducts()
        {
            // Arrange
            var mockQueryableFavorited = testFavorited.AsQueryable().BuildMock();

            mockFavoritedRepository
                .Setup(fr => fr.GetAllAttached())
                .Returns(mockQueryableFavorited);

            // Act
            InitializeCartItemService();
            var result = await favoriteService.GetUserFavoritesAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Products, Is.Not.Null);
                Assert.That(result.Products.Count, Is.EqualTo(testFavorited.Count));

                // Validate first favorite product
                var productModel = result.Products.First();
                Assert.That(productModel.ProductId, Is.EqualTo(testFavorited.First().ProductId));
                Assert.That(productModel.Name, Is.EqualTo(testFavorited.First().Product.Name));
                Assert.That(productModel.ImageUrl, Is.EqualTo(testFavorited.First().Product.ImageUrl));
                Assert.That(productModel.DateLiked, Is.EqualTo(testFavorited.First().FavoritedAt.ToString("dd/MM/yyyy")));
            });
        }

    }
}