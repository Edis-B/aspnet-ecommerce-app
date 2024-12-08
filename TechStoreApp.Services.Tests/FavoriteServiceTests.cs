using Moq;
using NUnit.Framework;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data;
using TechStoreApp.Web.ViewModels.Products;
using MockQueryable;

namespace TechStoreApp.Services.Tests
{
    [TestFixture]
    public class FavoriteServiceTests : TestBase
    {
        void InitializeCartItemService()
        {
            favoriteService = new FavoriteService(mockFavoritedRepository.Object,
                mockProductRepository.Object,
                mockUserService.Object);
        }

        [SetUp]
        public void SetUp()
        {
            ResetTestData();
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
            mockFavoritedRepository
                .Setup(fr => fr.GetAllAttached())
                .Returns(testFavorited.AsQueryable().BuildMock());

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