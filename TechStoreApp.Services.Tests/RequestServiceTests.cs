using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Services.Data;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Services.Tests
{
    [TestFixture]
    public class RequestServiceTests
    {
        private RequestService requestService;

        [SetUp]
        public void SetUp()
        {
            requestService = new RequestService();
        }
        [Test]
        public async Task GetProductIdFromRequest_ReturnsDeserializedObject()
        {
            // Arrange 
            var mockRequest = new Mock<HttpRequest>();
            var testProduct = new ProductIdFormModel { ProductId = 123 };
            string jsonContent = JsonConvert.SerializeObject(testProduct);

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent));
            mockRequest
                .Setup(r => r.Body)
                .Returns(memoryStream);

            var result = await requestService.GetProductIdFromRequest<dynamic>(mockRequest.Object);
            var product = result.ToObject<ProductIdFormModel>();

            // Act
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                int n = result.ProductId;
                Assert.That(n, Is.EqualTo(123));
            });
        }

        [Test]
        public async Task IsAjaxRequest_ReturnsHeadersCorrectly()
        {
            // Arrange 
            var mockRequest = new Mock<HttpRequest>();

            IHeaderDictionary headers = new HeaderDictionary();
            headers.Append("Content-Type", "application/json");

            mockRequest
                .Setup(r => r.Headers)
                .Returns(headers);

            var result = requestService.IsAjaxRequest(mockRequest.Object);

            // Assert
            Assert.That(result);
        }
    }
}