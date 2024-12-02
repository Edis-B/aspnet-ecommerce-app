using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Web.ViewModels.Address;
using NUnit.Framework;
using MockQueryable;
using Moq;

namespace TechStoreApp.Services.Tests
{
    [TestFixture]
    public class AddressServiceTests
    {
        private Guid userId;
        private Mock<IRepository<Address, int>> mockAddressRepository;
        private Mock<IUserService> mockUserService;
        private AddressService addressService;

        [SetUp]
        public void Setup()
        {
            userId = Guid.NewGuid();
            mockUserService = new Mock<IUserService>();
            mockUserService
                .Setup(x => x.GetUserId())
                .Returns(userId);

            mockAddressRepository = new Mock<IRepository<Address, int>>();
            addressService = new AddressService(mockUserService.Object, mockAddressRepository.Object);
        }

        [Test]
        public async Task SaveAddressAsync_ShouldSaveNewAddress()
        {
            // Arrange
            var model = new AddressFormModel
            {
                City = "Test City",
                Country = "Test Country",
                PostalCode = "12345",
                Details = "Test details"
            };

            // Act
            await addressService.SaveAddressAsync(model);

            // Assert
            mockAddressRepository.Verify(r => r.AddAsync(It.Is<Address>(a =>
                a.City == "Test City" &&
                a.Country == "Test Country" &&
                a.PostalCode == 12345 &&
                a.Details == "Test details" &&
                a.UserId == userId
            )), Times.Once);
        }

        [Test]
        public async Task GetAddressesByUserId_ShouldReturnAddresses_ForValidUserId()
        {
            // Arrange
            var address = new Address
            {
                AddressId = 1,
                City = "Test City",
                Country = "Test Country",
                PostalCode = 1234,
                Details = "Test details",
                UserId = userId,
                User = new ApplicationUser { Id = userId, UserName = "testuser" }
            };

            // Mock IQueryable using MockQueryable.Moq
            var mockQueryable = new List<Address> { address }.AsQueryable().BuildMock();

            // Setup repository to return the mock IQueryable
            mockAddressRepository
                .Setup(x => x.GetAllAttached())
                .Returns(mockQueryable);

            // Act
            var result = await addressService.GetAddressesByUserId(userId.ToString());

            // Assert
            var addressResult = result.First();
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(addressResult.City, Is.EqualTo("Test City"));
            Assert.That(addressResult.Country, Is.EqualTo("Test Country"));
            Assert.That(addressResult.PostalCode, Is.EqualTo(1234));
            Assert.That(addressResult.Details, Is.EqualTo("Test details"));
            Assert.That(addressResult.UserId, Is.EqualTo(userId.ToString()));
            Assert.That(addressResult.UserName, Is.EqualTo("testuser"));
        }

        [Test]
        public async Task GetAddressByIdAsync_ShouldReturnAddress_ForValidId()
        {
            // Arrange
            var address = new Address
            {
                AddressId = 1,
                City = "Test City",
                Country = "Test Country",
                PostalCode = 12345,
                Details = "Test details",
                UserId = userId,
                User = new ApplicationUser { Id = userId, UserName = "testuser" }
            };


            // Mock IQueryable using MockQueryable.Moq
            var mockQueryable = new List<Address> { address }.AsQueryable().BuildMock();

            mockAddressRepository
                .Setup(x => x.GetAllAttached())
                .Returns(mockQueryable);

            // Act
            var result = await addressService.GetAddressByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.City, Is.EqualTo("Test City"));
            Assert.That(result.Country, Is.EqualTo("Test Country"));
            Assert.That(result.PostalCode, Is.EqualTo("12345"));
            Assert.That(result.Details, Is.EqualTo("Test details"));
        }
    }
}
