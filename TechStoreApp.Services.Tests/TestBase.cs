using Moq;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Services.Data.Interfaces;
using MockQueryable;
using Microsoft.AspNetCore.Identity;
using Moq.AutoMock;

namespace TechStoreApp.Services.Tests
{
    public class TestBase
    {
        // Mock repositories
        protected Mock<IRepository<Address, int>> mockAddressRepository;
        protected Mock<IRepository<ApplicationUser, Guid>> mockUserRepository;
        protected Mock<IRepository<Cart, int>> mockCartRepository;
        protected Mock<IRepository<CartItem, object>> mockCartItemRepository;
        protected Mock<IRepository<Category, int>> mockCategoryRepository;
        protected Mock<IRepository<Favorited, object>> mockFavoritedRepository;
        protected Mock<IRepository<Order, int>> mockOrderRepository;
        protected Mock<IRepository<OrderDetail, int>> mockOrderDetailRepository;
        protected Mock<IRepository<Product, int>> mockProductRepository;
        protected Mock<IRepository<PaymentDetail, int>> mockPaymentDetailRepository;
        protected Mock<IRepository<Review, int>> mockReviewRepository;
        protected Mock<IRepository<Status, int>> mockStatusRepository;

        // Mock services
        protected Mock<IUserService> mockUserService;

        // Mock managers
        protected Mock<RoleManager<ApplicationRole>> mockRoleManager;
        protected Mock<UserManager<ApplicationUser>> mockUserManager;

        // Services
        protected AddressService addressService;
        protected CartItemService cartItemService;
        protected CartService cartService;
        protected CookieService cookieService;
        protected FavoriteService favoriteService;
        protected HomeService homeService;
        protected OrderService orderService;
        protected ProductService productService;
        protected ProfileService profileService;
        protected RequestService requestService;
        protected ReviewService reviewService;
        protected SearchService searchService;
        protected SeedDataService seedDataService;
        protected StatusService statusService;
        protected UserService userService;


        // Data for both OrderService and ProductService
        protected Guid userId;
        protected Guid roleId;
        protected List<ApplicationUser> testUsers;
        protected List<Order> testOrders;
        protected List<Cart> testCarts;
        protected List<CartItem> testCartItems;
        protected List<Favorited> testFavorited;
        protected List<Product> testProducts;
        protected List<OrderDetail> testOrderDetails;
        protected List<Address> testAddresses;
        protected List<Status> testStatuses;
        protected List<PaymentDetail> testPaymentDetail;
        protected List<Review> testReviews;
        protected List<Category> testCategories;
        protected List<ApplicationRole> testRoles;

        protected TestBase()
        {
            var mocker = new AutoMocker();
            mockUserManager = mocker.GetMock<UserManager<ApplicationUser>>();
            mockRoleManager = mocker.GetMock<RoleManager<ApplicationRole>>();

            roleId = Guid.NewGuid();
            userId = Guid.NewGuid();
            mockUserRepository = new Mock<IRepository<ApplicationUser, Guid>>();

            mockUserService = new Mock<IUserService>();
            mockUserService.Setup(x => x.GetUserId())
                .Returns(userId);

            mockAddressRepository = new Mock<IRepository<Address, int>>();
            mockOrderRepository = new Mock<IRepository<Order, int>>();
            mockProductRepository = new Mock<IRepository<Product, int>>();
            mockOrderDetailRepository = new Mock<IRepository<OrderDetail, int>>();
            mockCartRepository = new Mock<IRepository<Cart, int>>();
            mockCartItemRepository = new Mock<IRepository<CartItem, object>>();
            mockStatusRepository = new Mock<IRepository<Status, int>>();
            mockPaymentDetailRepository = new Mock<IRepository<PaymentDetail, int>>();
            mockReviewRepository = new Mock<IRepository<Review, int>>();
            mockCategoryRepository = new Mock<IRepository<Category, int>>();
            mockFavoritedRepository = new Mock<IRepository<Favorited, object>>();
        }

        protected void ResetTestData()
        {
            // Test data setup
            testCarts = new List<Cart>
            {
                new Cart { CartId = 1, UserId = userId, CartItems = new List<CartItem>() }
            };

            testUsers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = userId, UserName = "TestUserName", Cart = testCarts[0] }
            };

            testPaymentDetail = new List<PaymentDetail>
            {
                new PaymentDetail { PaymentId = 1, Description = "TestPayment1" }
            };

            testCategories = new List<Category>
            {
                new Category { CategoryId = 1, Description = "TestCategoryDescription1", IsFeatured = true },
                new Category { CategoryId = 2, Description = "TestCategoryDescription2", IsFeatured = true }
            };

            testProducts = new List<Product>
            {
                new Product { ProductId = 1,
                    Name = "TestProduct1",
                    Price = 10,
                    Stock = 100,
                    ImageUrl = "testimage1.jpg",
                    Description = "Description1",
                    Reviews = new List<Review>(),
                    CategoryId = 1,
                    Category = testCategories.First(c => c.CategoryId == 1)
                },
                new Product { ProductId = 2,
                    Name = "TestProduct2",
                    Price = 20,
                    Stock = 50,
                    ImageUrl = "testimage2.jpg",
                    Description = "Description2",
                    IsFeatured = true,
                    Reviews = new List<Review>(),
                    CategoryId = 2,
                    Category = testCategories.First(c => c.CategoryId == 2)
                }
            };

            testReviews = new List<Review>
            {
                new Review
                {
                    ReviewDate = DateTime.Now,
                    Rating = 5,
                    ProductId = 1,
                    Comment = "Great product!",
                    UserId = userId,
                    User = new ApplicationUser { Id = userId, UserName = "User1" }
                },
                new Review
                {
                    ReviewDate = DateTime.Now,
                    Rating = 4,
                    ProductId = 1,
                    Comment = "Good quality.",
                    UserId = userId,
                    User = new ApplicationUser { Id = userId, UserName = "User2" }
                }
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


            testCartItems = new List<CartItem>
            {
                new CartItem { CartId = 1, ProductId = 1, Quantity = 2, Product = testProducts[0], Cart = testCarts[0] },
                new CartItem { CartId = 1, ProductId = 2, Quantity = 1, Product = testProducts[1], Cart = testCarts[0] }
            };

            testOrders = new List<Order>
            {
                new Order { OrderId = 1, UserId = userId, User = testUsers[0], TotalAmount = 50, ShippingAddress = "123TestSt", Status = new Status { StatusId = 1, Description = "Order Received" }, OrderDetails = new List<OrderDetail>(testOrderDetails) }
            };

            testFavorited = new List<Favorited> 
            { 
                new Favorited
                {
                    ProductId = testProducts[0].ProductId,
                    UserId = userId,
                    Product = testProducts[0],
                    FavoritedAt = DateTime.UtcNow
                },
                new Favorited
                {
                    ProductId = testProducts[1].ProductId,
                    UserId = userId,
                    Product = testProducts[1],
                    FavoritedAt = DateTime.UtcNow
                }
            };

            testRoles = new List<ApplicationRole>()
            {
                new ApplicationRole { Id = roleId, Name = "Admin" }
            };

            testCarts[0].CartItems = testCartItems.Where(ci => ci.CartId == testCarts[0].CartId).ToList();
        }
    }
}
