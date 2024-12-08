using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Products;
using NUnit.Framework;
using MockQueryable;
using Moq;
using Newtonsoft.Json;

namespace TechStoreApp.Services.Tests
{
    [TestFixture]
    public class CartItemServiceTests : TestBase
    {
        void InitializeCartItemService()
        {
            cartItemService = new CartItemService(mockCartItemRepository.Object,
                mockProductRepository.Object,
                mockUserRepository.Object,
                mockUserService.Object);
        }

        [SetUp]
        public void SetUp()
        {
            ResetTestData();
        }

        [Test]
        public async Task IncreaseCountAsync_ShouldIncreaseCountProperly()
        {
            // Arrange
            int testProductId = 1;

            mockProductRepository
                .Setup(pr => pr.GetByIdAsync(testProductId))
                .ReturnsAsync(testProducts.First(p => p.ProductId == testProductId));

            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

            // Act
            InitializeCartItemService();
            await cartItemService.IncreaseCountAsync(new ProductIdFormModel() { ProductId = testProductId });

            // Assert
            mockCartItemRepository.Verify(cr => cr.UpdateAsync(It.Is<CartItem>(ci =>
                ci.CartId == 1 &&
                ci.ProductId == testProductId &&
                ci.Quantity == 3
                )), Times.Once);
        }

        [Test]
        public async Task DecreaseCountAsync_ShouldDecreaseProperly()
        {
            // Arrange
            int testProductId = 1;

            mockProductRepository
                .Setup(pr => pr.GetByIdAsync(testProductId))
                .ReturnsAsync(testProducts.First(p => p.ProductId == testProductId));

            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

            // Act
            InitializeCartItemService();
            await cartItemService.DecreaseCountAsync(new ProductIdFormModel() { ProductId = testProductId });

            // Assert
            mockCartItemRepository.Verify(cr => cr.UpdateAsync(It.Is<CartItem>(ci =>
                ci.CartId == 1 &&
                ci.ProductId == testProductId &&
                ci.Quantity == 1
                )), Times.Once);
        }

        [Test]
        public async Task GetCartItemsCountAsync_ShouldReturnProperly()
        {
            // Arrange
            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

            // Act
            InitializeCartItemService();
            JsonResult? result = await cartItemService.GetCartItemsCountAsync();

            // Extract the Value property from the JsonResult and cast it
            var resultCount = result?.Value as dynamic;
            var resultNum = resultCount!.Total;

            // Assert
            int expected = testCartItems.Sum(ci => ci.Quantity);
            Assert.That(resultNum, Is.EqualTo(expected));
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldRemoveProperly()
        {
            // Arrange
            int testProductId = 1;
            mockCartItemRepository
                .Setup(cir => cir.GetAllAttached())
                .Returns(testCartItems.AsQueryable().BuildMock());

            // Act
            InitializeCartItemService();
            await cartItemService.RemoveFromCartAsync(new ProductIdFormModel() { ProductId = testProductId });

            // Assert
            mockCartItemRepository.Verify(cr => cr.DeleteAsync(1, testProductId), Times.Once);
        }
    }
}
