using MockQueryable;
using Moq;
using NUnit.Framework;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Services.Tests
{

    [TestFixture]
    public class CartServiceTests : TestBase
    {
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
            ResetTestData();
        }

        [Test]
        public async Task AddToCartAsync_ShouldAddNewProductIfItsNotAlreadyAdded()
        {
            // Arrange
            var testProductSeperate = new Product()
            {
                ProductId = 3,
                CategoryId = 3,
                Name = "TestName3",
                Description = "TestDescription3",
                Price = 3,
                Stock = 30,
                ImageUrl = "TestImage3"
            };

            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

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
            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

            mockProductRepository
                .Setup(pr => pr.GetAllAttached())
                .Returns(testProducts.AsQueryable().BuildMock());

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
                Assert.That(modelCartItems1.Product.ProductId, Is.EqualTo(modelExpected1.Product!.ProductId));
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
                Assert.That(modelCartItems2.Product.ProductId, Is.EqualTo(modelExpected2.Product!.ProductId));
                Assert.That(modelCartItems2.Product.ImageUrl, Is.EqualTo(modelExpected2.Product.ImageUrl));
                Assert.That(modelCartItems2.Product.Name, Is.EqualTo(modelExpected2.Product.Name));
            });
        }

        [Test]
        public async Task ClearCartAsync_ShouldClearCartSuccessfully()
        {
            // Arrange
            mockCartRepository
                .Setup(cr => cr.GetAllAttached())
                .Returns(testCarts.AsQueryable().BuildMock());

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
