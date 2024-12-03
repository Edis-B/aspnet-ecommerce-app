using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System.Net.Sockets;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Services.Tests
{
    public class ProfileServiceTests
    {
        private ProfileService profileService;

        private Mock<IRepository<ApplicationUser, Guid>> mockUserRepository;
        private Mock<RoleManager<ApplicationRole>> mockRoleManager;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private Mock<IUserService> mockUserService;

        private Guid userId;
        private Guid roleId;
        private List<ApplicationUser> testUsers;
        private List<ApplicationRole> testRoles;

        [SetUp]
        public void SetUp()
        {
            testRoles = new List<ApplicationRole>()
            {
                new ApplicationRole { Id = roleId, Name = "Admin" }
            };

            userId = Guid.NewGuid();

            testUsers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = userId, UserName = "TestUserName", Email = "testemail@example.com", ProfilePictureUrl = "TestImage.jpeg" },
                new ApplicationUser { Id = Guid.NewGuid(), UserName = "TestUserName2", Email = "testemail@example.com", ProfilePictureUrl = "TestImage.jpeg" }
            };

            var mocker = new AutoMocker();

            roleId = Guid.NewGuid();
            mockUserService = new Mock<IUserService>();
            mockUserService
                .Setup(ur => ur.GetUserId())
            .Returns(userId);

            mockUserManager = mocker.GetMock<UserManager<ApplicationUser>>();
            mockRoleManager = mocker.GetMock<RoleManager<ApplicationRole>>();

            mockUserRepository = new Mock<IRepository<ApplicationUser, Guid>>();

        }

        void InitializeProfileService()
        {
            // Initialize service
            profileService = new ProfileService(
                mockUserRepository.Object,
                mockUserManager.Object,
                mockRoleManager.Object,
                mockUserService.Object
            );
        }

        [Test]
        public async Task GetAllUsersPageAsync_FiltersAndPaginatesUsersCorrectly()
        {
            // Arrange
            string userName = "TestUserName";
            string email = "testemail@example.com";
            int page = 1;
            int itemsPerPage = 1;

            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

            mockRoleManager
                .Setup(rm => rm.Roles)
                .Returns(testRoles.AsQueryable().BuildMock());

            mockUserManager
                .Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(testRoles.Select(r => r.ToString()).ToList());

            // Act
            InitializeProfileService();
            var result = await profileService.GetAllUsersPageAsync(userName, email, page, itemsPerPage);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);

                Assert.That(result.Users.Count, Is.EqualTo(1));
                Assert.That(result.Users[0].UserName, Is.EqualTo("TestUserName"));
                Assert.That(result.Users[0].Email, Is.EqualTo("testemail@example.com"));

                Assert.That(result.Page, Is.EqualTo(page));
                Assert.That(result.ItemsPerPage, Is.EqualTo(itemsPerPage));
                Assert.That(result.TotalPages, Is.EqualTo(2));

                Assert.That(result.EmailQuery, Is.EqualTo(email));
                Assert.That(result.UserNameQuery, Is.EqualTo(userName));

                var user = result.Users[0];
                Assert.That(user.UserRoles, Is.EqualTo(testRoles.Select(r => r.ToString()).ToList()));
                Assert.That(user.MissingRoles, Is.Empty);
            });
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsAllUsersCorrectly()
        {
            // Arrange
            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

            mockUserManager
                .Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(testRoles.Select(r => r.ToString()).ToList());

            // Act
            InitializeProfileService();
            var result = await profileService.GetAllUsersAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(testUsers.Count));

                Assert.That(result.First().UserId, Is.EqualTo(testUsers[0].Id.ToString()));
                Assert.That(result.First().UserName, Is.EqualTo(testUsers[0].UserName));
                Assert.That(result.First().Email, Is.EqualTo(testUsers[0].Email));
                Assert.That(result.First().ProfilePictureUrl, Is.EqualTo(testUsers[0].ProfilePictureUrl));

                
                Assert.That(result.Last().UserId, Is.EqualTo(testUsers[1].Id.ToString()));
                Assert.That(result.Last().UserName, Is.EqualTo(testUsers[1].UserName));
                Assert.That(result.Last().Email, Is.EqualTo(testUsers[1].Email));
                Assert.That(result.Last().ProfilePictureUrl, Is.EqualTo(testUsers[1].ProfilePictureUrl));

                // Ensure roles are correctly assigned to the users
                var firstUserRoles = result.First().Roles;
                Assert.That(firstUserRoles, Is.Not.Null);
                Assert.That(firstUserRoles.Count, Is.EqualTo(testRoles.Count));
                Assert.That(firstUserRoles, Does.Contain(testRoles[0].Name));

                var secondUserRoles = result.Last().Roles;
                Assert.That(secondUserRoles, Is.Not.Null);
                Assert.That(secondUserRoles.Count, Is.EqualTo(testRoles.Count));
                Assert.That(secondUserRoles, Does.Contain(testRoles[0].Name));
            });
        }

        [Test]
        public async Task GetUserProfilePictureUrlAsync_ShouldReturnProperly()
        {
            // Arrange
            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

            // Act
            InitializeProfileService();
            var result = await profileService.GetUserProfilePictureUrlAsync();

            // Assert
            Assert.Multiple(() =>
            {
                result.ProfilePictureUrl = testUsers!
                    .FirstOrDefault(u => u.Id == userId)!
                    .ProfilePictureUrl!;
            });
        }

        [Test]
        public async Task GetUserFromIdAsync_ReturnsCorrectUser()
        {
            // Arrange
            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

            // Act
            InitializeProfileService();
            var result = await profileService.GetUserFromIdAsync(userId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(userId));

                var expectedUser = testUsers.FirstOrDefault(u => u.Id == userId);

                Assert.That(result.UserName, Is.EqualTo(expectedUser!.UserName));
                Assert.That(result.Email, Is.EqualTo(expectedUser.Email));
                Assert.That(result.ProfilePictureUrl, Is.EqualTo(expectedUser.ProfilePictureUrl));
            });

        }

        [Test]
        public async Task GetUserByTheirIdAsync_ReturnsCorrectUser()
        {
            // Arrange
            mockUserRepository
                .Setup(ur => ur.GetAllAttached())
                .Returns(testUsers.AsQueryable().BuildMock());

            mockUserManager
                .Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(testRoles.Select(x => x.Name!.ToString()).ToList());

            // Act
            InitializeProfileService();
            var result = await profileService.GetUserByTheirIdAsync(userId.ToString());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);

                var expectedUser = testUsers.FirstOrDefault(u => u.Id == userId);
                Assert.That(result.UserId, Is.EqualTo(expectedUser!.Id.ToString()));
                Assert.That(result.UserName, Is.EqualTo(expectedUser.UserName));
                Assert.That(result.Email, Is.EqualTo(expectedUser.Email));
                Assert.That(result.ProfilePictureUrl, Is.EqualTo(expectedUser.ProfilePictureUrl));

                Assert.That(result.Roles, Is.Not.Null.And.Not.Empty);
                Assert.That(result.Roles, Contains.Item("Admin"));

            });

        }
    }
}
