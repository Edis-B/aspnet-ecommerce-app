using MockQueryable;
using Moq;
using NUnit.Framework;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Products;

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

            cartItemService = new CartItemService(mockCartItemRepository.Object,
                mockProductRepository.Object,
                mockUserRepository.Object,
                mockUserService.Object);
        }

        [Test]
        public async Task IncreaseCountAsync_ShouldIncreaseCountProperly()
        {
            // Assert
            var testProduct = new Product()
            {
                ProductId = 1,
                CategoryId = 1,
                Name = "TestName",
                Description = "TestDescription",
                Price = 1,
                Stock = 10,
                ImageUrl = "TestImage"
            };


            var testCartItems = new List<CartItem>() {
                new CartItem()
                {
                    CartId = 1,
                    ProductId = 1,
                    Quantity = 2,
                    Product = testProduct,
                }
            };

            var testCarts = new List<Cart> {
                new Cart()
                {
                    CartId = 1,
                    CartItems = testCartItems
                }
            };

            var testUsers = new List<ApplicationUser>() { 
                new ApplicationUser() 
                { 
                    Id = userId,
                    Cart = testCarts[0]
                } 
            };

            testCartItems[0].Cart = testCarts[0];

            mockProductRepository
                .Setup(pr => pr.GetByIdAsync(1))
                .ReturnsAsync(testProduct);

            var mockQueryableUsers = testUsers.AsQueryable().BuildMock();

            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(mockQueryableUsers);

            // Act
            await cartItemService.IncreaseCountAsync(new ProductIdFormModel() { ProductId = 1 });

            // Assert
            mockCartItemRepository.Verify(cr => cr.UpdateAsync(It.Is<CartItem>(ci =>
                ci.CartId == 1 &&
                ci.ProductId == 1 &&
                ci.Quantity == 3
                )), Times.Once);
        }
    }
}
