using Moq;
using NUnit.Framework;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.Orders;
using MockQueryable;

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
        private Mock<IRepository<PaymentDetail, int>> mockPaymentDetailRepository;
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
        private List<Status> testStatuses;
        private List<PaymentDetail> testPaymentDetail;
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
                mockPaymentDetailRepository.Object,
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
            mockPaymentDetailRepository = new Mock<IRepository<PaymentDetail, int>>();

            testPaymentDetail =
            [
                new PaymentDetail { PaymentId = 1, Description = "TestPayment1"}
            ];

            testProducts =
            [
                new Product { ProductId = 1, Name = "TestProduct1", Price = 10, Stock = 100, ImageUrl = "testimage1.jpg", Description = "Description1" },

                new Product { ProductId = 2, Name = "TestProduct2", Price = 20, Stock = 50, ImageUrl = "testimage2.jpg", Description = "Description2" },
            ];

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
                new ApplicationUser { UserName = "TestUserName", Id = userId, Cart = testCarts.FirstOrDefault() }
            };


            testOrderDetails = new List<OrderDetail>
            {
                new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 2, UnitPrice = 10, Product = testProducts[0] },
                new OrderDetail { OrderId = 1, ProductId = 2, Quantity = 1, UnitPrice = 20, Product = testProducts[1] }
            };

            testAddresses = new List<Address>
            {
                new Address { AddressId = 1, City = "TestCity", Country = "TestCountry", Details = "TestDetails", PostalCode = 1111, UserId = userId, User = testUsers[0] }
            };

            testStatuses = new List<Status>
            {
                new Status { StatusId = 1, Description = "Order Received" }
            };

            testOrders = new List<Order>
            {
                new Order { OrderId = 1, UserId = userId, User = testUsers.First(),  TotalAmount = 50, ShippingAddress = "123TestSt", Status = testStatuses.First(), OrderDetails = testOrderDetails}
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
            
            var resultAddress = result.Address;
            var resultAllAddresses = result.AllUserAddresses;
            var orderCartItems = result.CartItems;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);

                if (resultAddress != null)
                {
                    Assert.That(resultAddress.Country, Is.EqualTo("TestCountry"));
                    Assert.That(resultAddress.City, Is.EqualTo("TestCity"));
                    Assert.That(resultAddress.Details, Is.EqualTo("TestDetails"));
                }

                // AllUserAddresses
                Assert.That(resultAllAddresses.Count, Is.EqualTo(testUsers.First().Addresses.Count));

                // Total cost
                Assert.That(result.TotalCost, Is.EqualTo(40));

                // CartItems
                Assert.That(orderCartItems, Is.Not.Null);
                Assert.That(orderCartItems.Count, Is.EqualTo(2));

                // Individual cart item validation
                Assert.That(orderCartItems
                    .Any(ci => ci.ProductId == 1 && ci.Quantity == 2), Is.True);
                Assert.That(orderCartItems
                    .Any(ci => ci.ProductId == 2 && ci.Quantity == 1), Is.True);

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

            var model = new OrderPageViewModel()
            {
                Address = addressModel,
                PaymentId = 1,
            };

            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

            mockPaymentDetailRepository 
                .Setup(ur => ur.GetById(1))
                .Returns(testPaymentDetail.FirstOrDefault()!);

            // Act
            InitializeOrderService();
            var result = await orderService.GetOrderFinalizedModelAsync(model);

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

                // PaymentId check
                Assert.That(result.PaymentId, Is.EqualTo(model.PaymentId));

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
            var sendOrderModel = new OrderFinalizedPageViewModel
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
            mockOrderRepository
                .Setup(or => or.GetAllAttached())
                .Returns(testOrders.AsQueryable().BuildMock());

            // Act
            InitializeOrderService();
            var result = await orderService.GetUserOrdersListViewModelAsync();

            // Assert
            Assert.Multiple(() =>
            {
                // Check the overall result
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Orders.Count, Is.EqualTo(testOrders.Count(o => o.UserId == userId)));

                foreach (var expectedOrder in testOrders.Where(o => o.UserId == userId))
                {
                    var actualOrder = result.Orders
                        .FirstOrDefault(o => o.OrderId == expectedOrder.OrderId);

                    // Check that each order exists in the result
                    Assert.That(actualOrder, Is.Not.Null);

                    // Check the order's basic properties
                    Assert.That(actualOrder!.OrderId, Is.EqualTo(expectedOrder.OrderId));
                    Assert.That(actualOrder.ShippingAddress, Is.EqualTo(expectedOrder.ShippingAddress));
                    Assert.That(actualOrder.OrderDate, Is.EqualTo(expectedOrder.OrderDate.ToString("dd/MM/yyyy")));

                    // Check order details
                    Assert.That(actualOrder.OrderDetails.Count, Is.EqualTo(expectedOrder.OrderDetails.Count));

                    foreach (var expectedDetail in expectedOrder.OrderDetails)
                    {
                        var actualDetail = actualOrder.OrderDetails.FirstOrDefault(od => od.ProductId == expectedDetail.ProductId);

                        // Check that each order detail exists and matches
                        Assert.That(actualDetail, Is.Not.Null);
                        Assert.That(actualDetail!.ProductId, Is.EqualTo(expectedDetail.ProductId));
                        Assert.That(actualDetail.ProductName, Is.EqualTo(expectedDetail.Product!.Name));
                        Assert.That(actualDetail.ProductImageUrl, Is.EqualTo(expectedDetail.Product.ImageUrl ?? string.Empty));
                        Assert.That(actualDetail.Quantity, Is.EqualTo(expectedDetail.Quantity));
                        Assert.That(actualDetail.UnitPrice, Is.EqualTo(expectedDetail.UnitPrice));
                    }
                }
            });
        }
        [Test]
        public async Task GetDetailsOfOrder_ReturnsCorrectOrderModel()
        {
            // Arrange
            mockOrderRepository
                .Setup(or => or.GetAllAttached())
                .Returns(testOrders.AsQueryable().BuildMock());

            mockStatusRepository
                .Setup(or => or.GetAllAttached())
                .Returns(testStatuses.AsQueryable().BuildMock());

            // Act
            InitializeOrderService();
            var result = await orderService.GetDetailsOfOrder(1);

            // Assert
            Assert.Multiple(() =>
            {
                // Ensure the result is not null
                Assert.That(result, Is.Not.Null);

                // Verify basic order details
                Assert.That(result.OrderId, Is.EqualTo(1));
                Assert.That(result.ShippingAddress, Is.EqualTo("123TestSt"));
                Assert.That(result.OrderDate, Is.Not.Null.And.Matches(@"\d{2}/\d{2}/\d{4}")); // Format dd/MM/yyyy
                Assert.That(result.TotalPrice, Is.EqualTo(50)); // 2 * 10 + 1 * 20

                // Verify the order details count
                Assert.That(result.OrderDetails.Count, Is.EqualTo(2));

                // Verify details for the first product
                var detail1 = result.OrderDetails.FirstOrDefault(d => d.ProductId == 1);
                Assert.That(detail1, Is.Not.Null);
                Assert.That(detail1!.ProductId, Is.EqualTo(1));
                Assert.That(detail1.ProductName, Is.EqualTo("TestProduct1"));
                Assert.That(detail1.ProductImageUrl, Is.EqualTo("testimage1.jpg"));
                Assert.That(detail1.Quantity, Is.EqualTo(2));
                Assert.That(detail1.UnitPrice, Is.EqualTo(10));

                // Verify details for the second product
                var detail2 = result.OrderDetails.FirstOrDefault(d => d.ProductId == 2);

                Assert.That(detail2, Is.Not.Null);
                Assert.That(detail2!.ProductId, Is.EqualTo(2));
                Assert.That(detail2.ProductName, Is.EqualTo("TestProduct2"));
                Assert.That(detail2.ProductImageUrl, Is.EqualTo("testimage2.jpg"));
                Assert.That(detail2.Quantity, Is.EqualTo(1));
                Assert.That(detail2.UnitPrice, Is.EqualTo(20));

                // Status check (if part of the result)
                Assert.That(result.CurrentStatus.Key, Is.EqualTo("Order Received"));
                Assert.That(result.CurrentStatus.Value, Is.EqualTo(1));
                
            });

        }

        [Test]
        public async Task GetAllOrders_ReturnsAllOrdersCorrectly()
        {
            // Arrange
            mockOrderRepository
                .Setup(or => or.GetAllAttached())
                .Returns(testOrders.AsQueryable().BuildMock());

            // Act
            InitializeOrderService();
            var results = await orderService.ApiGetAllOrders();
            var result = results.First();
            var expected = testOrders.First();
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.OrderId, Is.EqualTo(expected.OrderId));
                Assert.That(result.UserId, Is.EqualTo(expected.UserId.ToString()));
                Assert.That(result.UserName, Is.EqualTo(expected.User.UserName));
                Assert.That(result.TotalPrice, Is.EqualTo((double)expected.TotalAmount));
                Assert.That(result.OrderStatus, Is.EqualTo(expected.Status.Description));
                Assert.That(result.DeliveryAddress, Is.EqualTo(expected.ShippingAddress));

                var productsResult = result.Products;
                var productsExpected = expected.OrderDetails;

                foreach (var expectedDetail in productsExpected)
                {
                    var actualDetail = productsResult.FirstOrDefault(p => p.ProductId == expectedDetail.ProductId);
                    Assert.That(actualDetail, Is.Not.Null);
                    Assert.That(actualDetail!.ProductName, Is.EqualTo(expectedDetail.Product!.Name));
                    Assert.That(actualDetail.Quantity, Is.EqualTo(expectedDetail.Quantity));
                    Assert.That(actualDetail.UnitPrice, Is.EqualTo((double)expectedDetail.UnitPrice));
                }
            });

        }

        [Test]
        public async Task GetAllOrdersByUserId_ReturnsOrdersCorrectly()
        {
            // Arrange
            mockOrderRepository
                .Setup(or => or.GetAllAttached())
                .Returns(testOrders.AsQueryable().BuildMock());

            // Act
            InitializeOrderService();
            var results = await orderService.ApiGetAllOrdersFromUserId(userId.ToString());
            var result = results.First();
            var expected = testOrders.First();
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.OrderId, Is.EqualTo(expected.OrderId));
                Assert.That(result.UserId, Is.EqualTo(expected.UserId.ToString()));
                Assert.That(result.UserName, Is.EqualTo(expected.User.UserName));
                Assert.That(result.TotalPrice, Is.EqualTo((double)expected.TotalAmount));
                Assert.That(result.OrderStatus, Is.EqualTo(expected.Status.Description));
                Assert.That(result.DeliveryAddress, Is.EqualTo(expected.ShippingAddress));

                var productsResult = result.Products;
                var productsExpected = expected.OrderDetails;

                foreach (var expectedDetail in productsExpected)
                {
                    var actualDetail = productsResult.FirstOrDefault(p => p.ProductId == expectedDetail.ProductId);
                    Assert.That(actualDetail, Is.Not.Null);
                    Assert.That(actualDetail!.ProductName, Is.EqualTo(expectedDetail.Product!.Name));
                    Assert.That(actualDetail.Quantity, Is.EqualTo(expectedDetail.Quantity));
                    Assert.That(actualDetail.UnitPrice, Is.EqualTo((double)expectedDetail.UnitPrice));
                }
            });

        }
    }
}
