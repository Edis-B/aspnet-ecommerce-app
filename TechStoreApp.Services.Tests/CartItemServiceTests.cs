using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Products;

using NUnit.Framework;
using MockQueryable;
using Moq;

namespace TechStoreApp.Services.Tests
{
    [TestFixture]
    public class CartItemServiceTests
    {
        private Guid userId;
        private Mock<IRepository<CartItem, object>> mockCartItemRepository;
        private Mock<IRepository<Product, int>> mockProductRepository;
        private Mock<IRepository<ApplicationUser, Guid>> mockUserRepository;
        private Mock<IUserService> mockUserService;
        private CartItemService cartItemService;

        private Product testProduct;
        private Product testProduct2;
        private List<CartItem> testCartItems;
        private List<Cart> testCarts;
        private List<ApplicationUser> testUsers;

        void InitializeCartItemService()
        {
            cartItemService = new CartItemService(mockCartItemRepository.Object,
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


            // Data
            testProduct = new Product()
            {
                ProductId = 1,
                CategoryId = 1,
                Name = "TestName",
                Description = "TestDescription",
                Price = 1,
                Stock = 10,
                ImageUrl = "TestImage"
            };

            testProduct2 = new Product()
            {
                ProductId = 2,
                CategoryId = 2,
                Name = "TestName2",
                Description = "TestDescription2",
                Price = 2,
                Stock = 20,
                ImageUrl = "TestImage2"
            };

            testCartItems = new List<CartItem>() {
                new CartItem()
                {
                    CartId = 1,
                    ProductId = 1,
                    Quantity = 2,
                    Product = testProduct,
                },
                new CartItem()
                {
                    CartId = 1,
                    ProductId = 2,
                    Quantity = 2,
                    Product = testProduct2,
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
        }

        [Test]
        public async Task IncreaseCountAsync_ShouldIncreaseCountProperly()
        {
            // Arrange
            mockProductRepository
                .Setup(pr => pr.GetByIdAsync(1))
                .ReturnsAsync(testProduct);

            var mockQueryableUsers = testUsers.AsQueryable().BuildMock();

            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(mockQueryableUsers);

            // Act
            InitializeCartItemService();
            await cartItemService.IncreaseCountAsync(new ProductIdFormModel() { ProductId = 1 });

            // Assert
            mockCartItemRepository.Verify(cr => cr.UpdateAsync(It.Is<CartItem>(ci =>
                ci.CartId == 1 &&
                ci.ProductId == 1 &&
                ci.Quantity == 3
                )), Times.Once);
        }

        [Test]
        public async Task DecreaseCountAsync_ShouldDecreaseProperly()
        {
            // Arrange
            var mockQueryableUsers = testUsers.AsQueryable().BuildMock();

            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(mockQueryableUsers);

            // Act
            InitializeCartItemService();
            await cartItemService.DecreaseCountAsync(new ProductIdFormModel() { ProductId = 1 });

            // Assert
            mockCartItemRepository.Verify(cr => cr.UpdateAsync(It.Is<CartItem>(ci =>
                ci.CartId == 1 &&
                ci.ProductId == 1 &&
                ci.Quantity == 1
                )), Times.Once);
        }


        [Test]
        public async Task GetCartItemsCountAsync_ShouldReturnProperly()
        {
            // Arrange
            var mockQueryableUsers = testUsers.AsQueryable().BuildMock();

            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(mockQueryableUsers);

            // Act
            InitializeCartItemService();
            var result = await cartItemService.GetCartItemsCountAsync();

            int resultCount = result?.Value as int? ?? 0; // Access the Value or Data property as needed

            // Assert
            int expected = testCartItems.Sum(ci => ci.Quantity);
            Assert.That(resultCount, Is.EqualTo(expected));
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldRemoveProperly()
        {
            // Arrange
            var mockQueryableCartItems = testCartItems.AsQueryable().BuildMock();

            mockCartItemRepository
                .Setup(cir => cir.GetAllAttached())
                .Returns(mockQueryableCartItems);

            // Act
            InitializeCartItemService();
            await cartItemService.RemoveFromCartAsync(new ProductIdFormModel() { ProductId = 1 });

            // Assert
            mockCartItemRepository.Verify(cr => cr.DeleteAsync(testCarts.First().CartId, 1), Times.Once);
        }
    }
}
