using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.Cart;
using TechStoreApp.Web.ViewModels.Orders;
using TechStoreApp.Web.ViewModels.Products;
using MockQueryable;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Components.Forms;

namespace TechStoreApp.Services.Tests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IRepository<Order, int>> mockOrderRepository;
        private Mock<IRepository<ApplicationUser, Guid>> mockUserRepository;
        private Mock<IRepository<Product, int>> mockProductRepository;
        private Mock<IRepository<OrderDetail, int>> mockOrderDetailRepository;
        private Mock<IRepository<Cart, int>> mockCartRepository;
        private Mock<IRepository<CartItem, object>> mockCartItemRepository;
        private Mock<IRepository<Status, int>> mockStatusRepository;
        private Mock<IUserService> mockUserService;
        private OrderService orderService;

        private Guid userId;
        private List<ApplicationUser> testUsers;
        private List<Order> testOrders;
        private List<Cart> testCarts;
        private List<CartItem> testCartItems;
        private List<Product> testProducts;
        private List<OrderDetail> testOrderDetails;
        private List<Address> testAddresses;

        void InitializeOrderService()
        {
            // Initialize service
            orderService = new OrderService(
                mockOrderRepository.Object,
                mockUserRepository.Object,
                mockProductRepository.Object,
                mockOrderDetailRepository.Object,
                mockCartRepository.Object,
                mockCartItemRepository.Object,
                mockUserService.Object,
                mockStatusRepository.Object
            );
        }

        [SetUp]
        public void Setup()
        {
            // Setup test data
            userId = Guid.NewGuid();

            // Initialize mocks
            mockUserRepository = new Mock<IRepository<ApplicationUser, Guid>>();
            mockUserService = new Mock<IUserService>();
            mockUserService
                .Setup(x => x.GetUserId())
                .Returns(userId);

            mockOrderRepository = new Mock<IRepository<Order, int>>();
            mockProductRepository = new Mock<IRepository<Product, int>>();
            mockOrderDetailRepository = new Mock<IRepository<OrderDetail, int>>();
            mockCartRepository = new Mock<IRepository<Cart, int>>();
            mockCartItemRepository = new Mock<IRepository<CartItem, object>>();
            mockStatusRepository = new Mock<IRepository<Status, int>>();


            testProducts = new List<Product>
            {
                new Product { ProductId = 1, Name = "TestProduct1", Price = 10, Stock = 100, ImageUrl = "testimage1.jpg", Description = "Description1" },
                new Product { ProductId = 2, Name = "TestProduct2", Price = 20, Stock = 50, ImageUrl = "testimage2.jpg", Description = "Description2" },
            };

            testCartItems = new List<CartItem>
            {
                new CartItem { CartId = 1, ProductId = 1, Quantity = 2, Product = testProducts[0] },
                new CartItem { CartId = 1, ProductId = 2, Quantity = 1, Product = testProducts[1] }
            };

            testCarts = new List<Cart>
            {
                new Cart { CartId = 1, UserId = userId, CartItems = testCartItems }
            };

            testUsers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = userId, Cart = testCarts.FirstOrDefault() }
            };

            testOrders = new List<Order>
            {
                new Order { OrderId = 1, UserId = userId, TotalAmount = 50, ShippingAddress = "123TestSt" }
            };

            testOrderDetails = new List<OrderDetail>
            {
                new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 2, UnitPrice = 10 },
                new OrderDetail { OrderId = 1, ProductId = 2, Quantity = 1, UnitPrice = 20 }
            };

            testAddresses = new List<Address>
            {
                new Address { AddressId = 1, City = "TestCity", Country = "TestCountry", Details = "TestDetails", PostalCode = 1111, UserId = userId, User = testUsers[0] }
            };
        }

        [Test]
        public async Task GetOrderViewModelAsync_ShouldReturnCorrectOrderViewModel()
        {
            // Arrange
            var addressId = 1;
            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

            // Act
            InitializeOrderService();
            var result = await orderService.GetOrderViewModelAsync(addressId);
            
            var orderAddress =  result.Address;
            var orderAllOrderAddresses = result.AllUserAddresses;
            var orderTotalCost = result.TotalCost;
            var orderCartItems = result.CartItems;

            // Assert
            Assert.Multiple(() =>
            {
                // General result check
                Assert.That(result, Is.Not.Null);

                // Address properties
                Assert.That(orderAddress, Is.Null.Or.InstanceOf<AddressFormModel>());

                if (orderAddress != null)
                {
                    Assert.That(orderAddress.Country, Is.EqualTo("TestCountry"));
                    Assert.That(orderAddress.City, Is.EqualTo("TestCity"));
                    Assert.That(orderAddress.Details, Is.EqualTo("TestDetails"));
                }

                // AllUserAddresses
                Assert.That(orderAllOrderAddresses, Is.Not.Null);
                Assert.That(orderAllOrderAddresses.Count, Is.EqualTo(testUsers.First().Addresses.Count));

                // Total cost
                Assert.That(orderTotalCost, Is.EqualTo(40));

                // CartItems
                Assert.That(orderCartItems, Is.Not.Null);
                Assert.That(orderCartItems.Count, Is.EqualTo(2));

                // Individual cart item validation
                Assert.That(orderCartItems
                    .Any(ci => ci.ProductId == 1 && ci.Quantity == 2), Is.True);
                Assert.That(orderCartItems
                    .Any(ci => ci.ProductId == 2 && ci.Quantity == 1), Is.True);

                // Detailed cart item properties
                var firstCartItem = result.CartItems.First(ci => ci.ProductId == 1);
                Assert.That(firstCartItem.Product.Name, Is.EqualTo("TestProduct1"));
                Assert.That(firstCartItem.Product.Price, Is.EqualTo(10));
                Assert.That(firstCartItem.Product.ImageUrl, Is.EqualTo("testimage1.jpg"));
                Assert.That(firstCartItem.Product.Description, Is.EqualTo("Description1"));
            });
        }

        [Test]
        public async Task GetOrderFinalizedModelAsync_ShouldReturnCorrectFinalizedModel()
        {
            // Arrange
            var addressModel = new AddressFormModel { Country = "Test Country", City = "Test City", Details = "Test Details", PostalCode = "2222" };

            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

            // Act
            InitializeOrderService();
            var result = await orderService.GetOrderFinalizedModelAsync(addressModel);

            // Assert
            Assert.Multiple(() =>
            {
                // General checks
                Assert.That(result, Is.Not.Null);

                // Address checks
                Assert.That(result.Address, Is.Not.Null);
                Assert.That(result.Address.Country, Is.EqualTo(addressModel.Country));
                Assert.That(result.Address.City, Is.EqualTo(addressModel.City));
                Assert.That(result.Address.Details, Is.EqualTo(addressModel.Details));
                Assert.That(result.Address.PostalCode, Is.EqualTo(addressModel.PostalCode));

                // TotalSum check
                Assert.That(result.TotalSum, Is.EqualTo(40));

                // Cart checks
                Assert.That(result.Cart, Is.Not.Null);
                Assert.That(result.Cart.CartItems, Is.Not.Null);
                Assert.That(result.Cart.CartItems.Count, Is.EqualTo(2));

                // Detailed cart item checks
                var firstCartItem = result.Cart.CartItems.First(ci => ci.ProductId == 1);
                Assert.That(firstCartItem.Quantity, Is.EqualTo(2));
                Assert.That(firstCartItem.ProductId, Is.EqualTo(1));
                Assert.That(firstCartItem.Product.Name, Is.EqualTo("TestProduct1"));
                Assert.That(firstCartItem.Product.Price, Is.EqualTo(10));
                Assert.That(firstCartItem.Product.ImageUrl, Is.EqualTo("testimage1.jpg"));
                Assert.That(firstCartItem.Product.Description, Is.EqualTo("Description1"));

                var secondCartItem = result.Cart.CartItems.First(ci => ci.ProductId == 2);
                Assert.That(secondCartItem.Quantity, Is.EqualTo(1));
                Assert.That(secondCartItem.ProductId, Is.EqualTo(2));
                Assert.That(secondCartItem.Product.Name, Is.EqualTo("TestProduct2"));
                Assert.That(secondCartItem.Product.Price, Is.EqualTo(20));
                Assert.That(secondCartItem.Product.ImageUrl, Is.EqualTo("testimage2.jpg"));
                Assert.That(secondCartItem.Product.Description, Is.EqualTo("Description2"));
            });
        }

        [Test]
        public async Task SendOrderAsync_ShouldCreateOrderAndRemoveCartItems()
        {
            // Arrange
            var sendOrderModel = new SendOrderViewModel
            {
                Address = new AddressFormModel { Country = "Test Country", City = "Test City", Details = "Test Details", PostalCode = "1234" }
            };

            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

            mockProductRepository
                .Setup(pr => pr.GetByIdAsync(1))
                .ReturnsAsync(testProducts[0]);

            mockProductRepository
                .Setup(pr => pr.GetByIdAsync(2))
                .ReturnsAsync(testProducts[1]);

            // Act
            InitializeOrderService();
            await orderService.SendOrderAsync(sendOrderModel);

            // Assert
            Assert.Multiple(() =>
            {
                // Verify the order creation
                mockOrderRepository.Verify(or => or.AddAsync(It.Is<Order>(o =>
                    o.UserId == userId &&
                    o.TotalAmount == 40 && // 2 * 10 + 1 * 20
                    o.ShippingAddress == "Test Country, Test City (1234), Test Details" &&
                    o.StatusId == 1
                )), Times.Once);

                // Verify order details creation
                mockOrderDetailRepository.Verify(odr => odr.AddAsync(It.Is<OrderDetail>(od =>
                    od.ProductId == 1 &&
                    od.Quantity == 2 &&
                    od.UnitPrice == 10
                )), Times.Once);

                mockOrderDetailRepository.Verify(odr => odr.AddAsync(It.Is<OrderDetail>(od =>
                    od.ProductId == 2 &&
                    od.Quantity == 1 &&
                    od.UnitPrice == 20
                )), Times.Once);

                // Verify stock updates
                mockProductRepository.Verify(pr => pr.UpdateAsync(It.Is<Product>(p =>
                    p.ProductId == 1 &&
                    p.Stock == 98 // Original 100 - 2
                )), Times.Once);

                mockProductRepository.Verify(pr => pr.UpdateAsync(It.Is<Product>(p =>
                    p.ProductId == 2 &&
                    p.Stock == 49 // Original 50 - 1
                )), Times.Once);

                // Verify cart removal
                mockCartItemRepository.Verify(cr => cr.RemoveRangeAsync(It.Is<IEnumerable<CartItem>>(ci =>
                    ci.Count() == 2 && // All cart items removed
                    ci.Any(c => c.ProductId == 1 && c.Quantity == 2) &&
                    ci.Any(c => c.ProductId == 2 && c.Quantity == 1)
                )), Times.Once);

                mockCartRepository.Verify(cr => cr.DeleteAsync(It.Is<int>(id =>
                    id == testUsers.First().Cart!.CartId
                )), Times.Once);
            });
        }

        [Test]
        public async Task GetUserOrdersListViewModelAsync_ShouldReturnUserOrders()
        {
            // Arrange
            mockUserService.Setup(us => us.GetUserId()).Returns(userId);
            mockOrderRepository
                .Setup(or => or.GetAllAttached())
                .Returns(testOrders.AsQueryable().BuildMock());

            // Act
            InitializeOrderService();
            var result = await orderService.GetUserOrdersListViewModelAsync();

            // Assert
            Assert.Multiple(() =>
            {
                // Check the overall result is not null and contains the expected number of orders
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Orders.Count, Is.EqualTo(testOrders.Count(o => o.UserId == userId)));

                foreach (var expectedOrder in testOrders.Where(o => o.UserId == userId))
                {
                    var actualOrder = result.Orders
                        .FirstOrDefault(o => o.OrderId == expectedOrder.OrderId);

                    // Check that each order exists in the result
                    Assert.That(actualOrder, Is.Not.Null);

                    // Check the order's basic properties
                    Assert.That(actualOrder.OrderId, Is.EqualTo(expectedOrder.OrderId));
                    Assert.That(actualOrder.ShippingAddress, Is.EqualTo(expectedOrder.ShippingAddress));
                    Assert.That(actualOrder.OrderDate, Is.EqualTo(expectedOrder.OrderDate.ToString("dd/MM/yyyy")));

                    // Check order details
                    Assert.That(actualOrder.OrderDetails.Count, Is.EqualTo(expectedOrder.OrderDetails.Count));

                    foreach (var expectedDetail in expectedOrder.OrderDetails)
                    {
                        var actualDetail = actualOrder.OrderDetails.FirstOrDefault(od => od.ProductId == expectedDetail.ProductId);

                        // Check that each order detail exists and matches
                        Assert.That(actualDetail, Is.Not.Null);
                        Assert.That(actualDetail.ProductId, Is.EqualTo(expectedDetail.ProductId));
                        Assert.That(actualDetail.ProductName, Is.EqualTo(expectedDetail.Product.Name));
                        Assert.That(actualDetail.ProductImageUrl, Is.EqualTo(expectedDetail.Product.ImageUrl ?? string.Empty));
                        Assert.That(actualDetail.Quantity, Is.EqualTo(expectedDetail.Quantity));
                        Assert.That(actualDetail.UnitPrice, Is.EqualTo(expectedDetail.UnitPrice));
                    }
                }
            });
        }
    }
}
