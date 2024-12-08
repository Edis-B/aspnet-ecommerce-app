using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Services.Data.Interfaces;
using MockQueryable.Moq;
using MockQueryable;

namespace TechStoreApp.Services.Tests
{
    public class StatusServiceTests : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            ResetTestData();
        }

        void InitializeStatusService()
        {
            statusService = new StatusService(mockOrderRepository.Object);
        }

        [Test]
        public async Task EditStatusOfOrder_ShouldReturnTrue_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var statusId = 2;

            var orders = new List<Order>() 
            {
                new Order()
                {
                    OrderId = orderId,
                    StatusId = 1
                } 
            };

            mockOrderRepository
                .Setup(repo => repo.GetAllAttached())
                .Returns(orders.AsQueryable().BuildMock());

            // Act
            InitializeStatusService();
            var result = await statusService.EditStatusOfOrder(orderId, statusId);

            // Assert
            Assert.That(result);
            Assert.That(statusId, Is.EqualTo(orders.First().StatusId)); // The order's status should be updated
        }
    }
}