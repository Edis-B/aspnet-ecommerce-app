using MockQueryable;
using Moq;
using NUnit.Framework;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Web.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;

namespace TechStoreApp.Services.Tests
{

    [TestFixture]
    public class CartServiceTests
    {
        private Guid userId;
        private Mock<IRepository<Cart, int>> mockCartRepository;
        private Mock<IRepository<CartItem, object>> mockCartItemRepository;
        private Mock<IRepository<Product, int>> mockProductRepository;
        private Mock<IRepository<ApplicationUser, Guid>> mockUserRepository;
        private Mock<IUserService> mockUserService;
        private CartService cartService;

        private Product testProductSeperate;
        private List<Product> testProducts;
        private List<CartItem> testCartItems;
        private List<Cart> testCarts;
        private List<ApplicationUser> testUsers;

        void InitializeCartService()
        {
            cartService = new CartService(mockCartRepository.Object,
                mockCartItemRepository.Object,
                mockProductRepository.Object,
                mockUserRepository.Object,
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

            mockCartItemRepository = new Mock<IRepository<CartItem, object>>();
            mockProductRepository = new Mock<IRepository<Product, int>>();
            mockUserRepository = new Mock<IRepository<ApplicationUser, Guid>>();
            mockCartRepository = new Mock<IRepository<Cart, int>>();

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

            testCartItems[0].Cart = testCarts[0];
            testCartItems[1].Cart = testCarts[0];
        }

        [Test]
        public async Task AddToCartAsync_ShouldAddNewProductIfItsNotAlreadyAdded()
        {
            // Arrange
            var mockQueryableUsers = testUsers.AsQueryable().BuildMock();

            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(mockQueryableUsers);

            mockProductRepository
                .Setup(ur => ur.GetByIdAsync(testProductSeperate.ProductId))
                .ReturnsAsync(testProductSeperate);

            // Act
            InitializeCartService();
            await cartService.AddToCartAsync(new ProductIdFormModel() { ProductId = testProductSeperate.ProductId });

            // Assert
            mockCartItemRepository.Verify(cr => cr.AddAsync(It.Is<CartItem>(ci =>
                ci.CartId == testCarts.First().CartId &&
                ci.ProductId == testProductSeperate.ProductId &&
                ci.Quantity == 1
                )), Times.Once);
        }

        [Test]
        public async Task GetCartItemsAsync_ShouldAddReturnProductsWhenAvailable()
        {
            // Arrange
            var mockQueryableUsers = testUsers.AsQueryable().BuildMock();

            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(mockQueryableUsers);

            var mockQueryableProducts = testProducts.AsQueryable().BuildMock();

            mockProductRepository
                .Setup(pr => pr.GetAllAttached())
                .Returns(mockQueryableProducts);

            // Act
            InitializeCartService();
            var result = await cartService.GetCartItemsAsync();
            
            // Assert
            var modelCartItems1 = result.CartItems[0];
            var modelExpected1 = testCartItems[0];
            Assert.Multiple(() =>
            {
                Assert.That(modelCartItems1.Quantity, Is.EqualTo(modelExpected1.Quantity));
                Assert.That(modelCartItems1.CartId, Is.EqualTo(modelExpected1.CartId));
                Assert.That(modelCartItems1.ProductId, Is.EqualTo(modelExpected1.ProductId));
                Assert.That(modelCartItems1.Product.ProductId, Is.EqualTo(modelExpected1.Product.ProductId));
                Assert.That(modelCartItems1.Product.ImageUrl, Is.EqualTo(modelExpected1.Product.ImageUrl));
                Assert.That(modelCartItems1.Product.Name, Is.EqualTo(modelExpected1.Product.Name));
            });

            var modelCartItems2 = result.CartItems[1];
            var modelExpected2 = testCartItems[1];
            Assert.Multiple(() =>
            {
                Assert.That(modelCartItems2.Quantity, Is.EqualTo(modelExpected2.Quantity));
                Assert.That(modelCartItems2.CartId, Is.EqualTo(modelExpected2.CartId));
                Assert.That(modelCartItems2.ProductId, Is.EqualTo(modelExpected2.ProductId));
                Assert.That(modelCartItems2.Product.ProductId, Is.EqualTo(modelExpected2.Product.ProductId));
                Assert.That(modelCartItems2.Product.ImageUrl, Is.EqualTo(modelExpected2.Product.ImageUrl));
                Assert.That(modelCartItems2.Product.Name, Is.EqualTo(modelExpected2.Product.Name));
            });
        }

        [Test]
        public async Task ClearCartAsync_ShouldClearCartSuccessfully()
        {
            // Arrange
            var mockQueryableCarts = testCarts.AsQueryable().BuildMock();

            mockCartRepository
                .Setup(cr => cr.GetAllAttached())
                .Returns(mockQueryableCarts);

            // Act
            InitializeCartService();
            var result = await cartService.ClearCartAsync();

            // Assert
            mockCartRepository.Verify(cr => cr.DeleteAsync(It.Is<int>(id =>
                id == testCarts.First().CartId
            )), Times.Once);
        }
    }
}
