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
    public class ProductServiceTests
    {
        private Mock<IRepository<Product, int>> mockProductRepository;
        private Mock<IRepository<Review, int>> mockReviewRepository;
        private Mock<IRepository<Category, int>> mockCategoryRepository;
        private Mock<IUserService> mockUserService;
        private ProductService productService;

        private Guid userId;
        private List<ApplicationUser> testUsers;
        private List<Product> testProducts;
        private List<Review> testReviews;
        private List<Category> testCategories;
        [SetUp]
        public void SetUp()
        {
            userId = Guid.NewGuid();
            mockUserService = new Mock<IUserService>();
            mockUserService
                .Setup(ur => ur.GetUserId())
                .Returns(userId);

            mockProductRepository = new Mock<IRepository<Product, int>>();
            mockReviewRepository = new Mock<IRepository<Review, int>>();
            mockCategoryRepository = new Mock<IRepository<Category, int>>();

            testCategories = new List<Category>()
            {
                new Category() { CategoryId = 1, Description = "TestCategoryDescription1"}
            };

            testUsers = new List<ApplicationUser>()
            {
                new ApplicationUser() { Id = userId, UserName = "TestUserName" },
            };

            testReviews = new List<Review>()
            {
                new Review { ReviewId = 1, ProductId = 1,  UserId = userId, Rating = 4, Comment = "TestComment1", User = testUsers.First() },

                new Review { ReviewId = 2, ProductId = 2,  UserId = userId, Rating = 3, Comment = "TestComment2", User = testUsers.First() }

            };

            testProducts = new List<Product>
            {
                new Product { ProductId = 1, Name = "TestProduct1", Price = 10, Stock = 100, ImageUrl = "testimage1.jpg", Description = "Description1", Reviews = testReviews.Where(x => x.ProductId == 1).ToList(), Category = testCategories.First() },

                new Product { ProductId = 2, Name = "TestProduct2", Price = 20, Stock = 50, ImageUrl = "testimage2.jpg", Description = "Description2", Reviews = testReviews.Where(x => x.ProductId == 2).ToList() },
            };
        }

        void InitializeProductService()
        {
            // Initialize service
            productService = new ProductService(
                mockUserService.Object,
                mockReviewRepository.Object,
                mockCategoryRepository.Object,
                mockProductRepository.Object
            );
        }
        [Test]
        public async Task GetProductViewModelAsync_ShouldReturnCorrectProduct()
        {
            // Arrange
            int testProductId = 1;
            mockProductRepository
                .Setup(pr => pr.GetAllAttached())
                .Returns(testProducts.AsQueryable().BuildMock());

            // Act
            InitializeProductService();
            var result = await productService.GetProductViewModelAsync(testProductId);
            var expected = testProducts.FirstOrDefault();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);

                Assert.That(result.ProductId, Is.EqualTo(expected!.ProductId));
                Assert.That(result.Name, Is.EqualTo(expected.Name));
                Assert.That(result.Description, Is.EqualTo(expected.Description));
                Assert.That(result.Price, Is.EqualTo(expected.Price));
                Assert.That(result.Stock, Is.EqualTo(expected.Stock));
                Assert.That(result.ImageUrl, Is.EqualTo(expected.ImageUrl));
                Assert.That(result.TotalLikes, Is.EqualTo(expected.Favorites?.Count ?? 0));

                // CheckedString assertion
                bool isFavorite = expected.Favorites?.Any(f => f.UserId == userId) ?? false;
                string expectedCheckedString = isFavorite ? "checked" : "unchecked";
                Assert.That(result.CheckedString, Is.EqualTo(expectedCheckedString));

                // Review assertions
                var resultReviews = result.Reviews.ToList();
                var expectedReviews = expected.Reviews.ToList();

                for (int i = 0; i < expectedReviews.Count; i++)
                {
                    Assert.That(resultReviews[i].Comment, Is.EqualTo(expectedReviews[i].Comment), $"Review {i + 1} Comment does not match");
                    Assert.That(resultReviews[i].Rating, Is.EqualTo(expectedReviews[i].Rating), $"Review {i + 1} Rating does not match");
                    Assert.That(resultReviews[i].ProductId, Is.EqualTo(expectedReviews[i].ProductId), $"Review {i + 1} ProductId does not match");
                    Assert.That(resultReviews[i].Author, Is.EqualTo(expectedReviews[i].User?.UserName ?? "Error with UserName"), $"Review {i + 1} Author does not match");
                }

            });
        }


        [Test]
        public async Task GetEditProductViewModelAsync_ShouldReturnCorrectProduct()
        {
            // Arrange
            int testProductId = 1;
            mockProductRepository
                .Setup(pr => pr.GetAllAttached())
                .Returns(testProducts.AsQueryable().BuildMock());

            mockCategoryRepository
                .Setup(pr => pr.GetAllAttached())
                .Returns(testCategories.AsQueryable().BuildMock());

            // Act
            InitializeProductService();
            var result = await productService.GetEditProductViewModelAsync(testProductId);
            var expected = testProducts.FirstOrDefault();

            // Assert
            Assert.Multiple(() =>
            {
                // Check if the result is not null
                Assert.That(result, Is.Not.Null);

                Assert.That(result.ProductId, Is.EqualTo(expected!.ProductId));
                Assert.That(result.ProductName, Is.EqualTo(expected.Name));
                Assert.That(result.Description, Is.EqualTo(expected.Description));
                Assert.That(result.Price, Is.EqualTo(expected.Price));
                Assert.That(result.Stock, Is.EqualTo(expected.Stock));
                Assert.That(result.ImageUrl, Is.EqualTo(expected.ImageUrl));

                Assert.That(result.CategoryId, Is.EqualTo(expected.CategoryId));
                Assert.That(result.CategoryName, Is.EqualTo(expected.Category.Description));

                Assert.That(result.Categories, Is.Not.Empty);
                Assert.That(result.Categories.Count, Is.GreaterThan(0));

                foreach (var category in result.Categories)
                {
                    var expectedCategory = testCategories.FirstOrDefault(c => c.CategoryId == category.Id);
                    Assert.That(category.Description, Is.EqualTo(expectedCategory?.Description));
                }
            });
        }

        [Test]
        public async Task EditProductAsync_ShouldEditTheCorrectProduct()
        {
            // Arrange
            int testProductId = 1;
            var productToEdit = testProducts.First();

            var editedProduct = new EditProductViewModel()
            {
                ProductId = testProductId,
                ProductName = "EditedName",
                CategoryId = 2,  // Assuming you change the category ID
                CategoryName = "EditedCategory",
                Description = "Edited description",
                Price = 15.99m,  // Modified price
                Stock = 200,     // Modified stock
                ImageUrl = "editedImageUrl.jpg"
            };

            mockProductRepository
                .Setup(pr => pr.GetByIdAsync(testProductId))
                .ReturnsAsync(productToEdit);

            // Act
            InitializeProductService();
            await productService.EditProductAsync(editedProduct);

            // Assert
            Assert.Multiple(() =>
            {
                mockProductRepository.Verify(pr => pr.UpdateAsync(It.Is<Product>(p =>
                    p.ProductId == testProductId &&
                    p.Name == editedProduct.ProductName &&
                    p.CategoryId == editedProduct.CategoryId &&
                    p.Description == editedProduct.Description &&
                    p.Price == editedProduct.Price &&
                    p.Stock == editedProduct.Stock &&
                    p.ImageUrl == editedProduct.ImageUrl
                )), Times.Once);

                Assert.That(productToEdit.ProductId, Is.EqualTo(testProductId));
                Assert.That(productToEdit.Name, Is.EqualTo(editedProduct.ProductName));
                Assert.That(productToEdit.CategoryId, Is.EqualTo(editedProduct.CategoryId));
                Assert.That(productToEdit.Description, Is.EqualTo(editedProduct.Description));
                Assert.That(productToEdit.Price, Is.EqualTo(editedProduct.Price));
                Assert.That(productToEdit.Stock, Is.EqualTo(editedProduct.Stock));
                Assert.That(productToEdit.ImageUrl, Is.EqualTo(editedProduct.ImageUrl));
            });
        }

        [Test]
        public async Task AddProductAsync_ShouldAddTheCorrectProduct()
        {
            // Arrange
            int testProductId = 1;
            var productToEdit = testProducts.First();

            var addedProduct = new AddProductViewModel()
            {
                ProductName = "NewTestProduct",
                Description = "This is a new test product",
                Price = 29.99m,
                Stock = 150,
                ImageUrl = "newtestimage.jpg",
                CategoryId = 1
            };

            // Act
            InitializeProductService();
            var result = await productService.AddProductAsync(addedProduct);

            // Assert
            Assert.Multiple(() =>
            {
                mockProductRepository.Verify(pr => pr.AddAsync(It.Is<Product>(p =>
                    p.Name == addedProduct.ProductName &&
                    p.Description == addedProduct.Description &&
                    p.Price == addedProduct.Price &&
                    p.Stock == addedProduct.Stock &&
                    p.ImageUrl == addedProduct.ImageUrl &&
                    p.CategoryId == addedProduct.CategoryId
                )), Times.Once);
            });
        }


        [Test]
        public void GetAddProductViewModel_ShouldReturnANewAddProductViewModel()
        {
            // Arrange
            mockCategoryRepository
                .Setup(cr => cr.GetAllAttached())
                .Returns(testCategories.AsQueryable().BuildMock());

            // Act
            InitializeProductService();
            var result = productService.GetAddProductViewModel();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Categories, Is.Not.Null, "Categories should not be null");
                Assert.That(result.Categories.Count(), Is.EqualTo(testCategories.Count), "Categories count does not match");

                for (int i = 0; i < testCategories.Count; i++)
                {
                    Assert.That(result.Categories.ElementAt(i).Description, Is.EqualTo(testCategories[i].Description));
                    Assert.That(result.Categories.ElementAt(i).Id, Is.EqualTo(testCategories[i].CategoryId));
                }
            });
        }

        [Test]
        public void GetAllProduct_ShouldReturnAllProductsCorrectly()
        {
            // Arrange
            mockProductRepository
                .Setup(cr => cr.GetAllAttached())
                .Returns(testProducts.AsQueryable().BuildMock());

            // Act
            InitializeProductService();
            var result = productService.GetAllProducts();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(testProducts.Count));

                for (int i = 0; i < testProducts.Count; i++)
                {
                    var resultProduct = result.ElementAt(i);
                    var expectedProduct = testProducts[i];

                    Assert.That(resultProduct.ProductId, Is.EqualTo(expectedProduct.ProductId));
                    Assert.That(resultProduct.CategoryId, Is.EqualTo(expectedProduct.CategoryId));
                    Assert.That(resultProduct.Name, Is.EqualTo(expectedProduct.Name));
                    Assert.That(resultProduct.Description, Is.EqualTo(expectedProduct.Description));
                    Assert.That(resultProduct.Price, Is.EqualTo(expectedProduct.Price));
                    Assert.That(resultProduct.Stock, Is.EqualTo(expectedProduct.Stock));
                    Assert.That(resultProduct.ImageUrl, Is.EqualTo(expectedProduct.ImageUrl));

                    // Assert that TotalLikes is calculated correctly (assuming Favorites is correctly set in test data)
                    Assert.That(resultProduct.TotalLikes, Is.EqualTo(expectedProduct.Favorites.Count));
                }
            });
        }

        [Test]
        public void GetAllProductsByQuery_ShouldReturnProperly()
        {
            // Arrange
            var productNameFilter = "TestProduct"; // Example product name filter
            var categoryIdFilter = 1; // Example category ID filter

            mockProductRepository
                .Setup(pr => pr.GetAllAttached())
                .Returns(testProducts.AsQueryable().BuildMock());

            // Act
            InitializeProductService();
            var resultWithoutFilters = productService.GetAllProductsByQuery();

            var resultWithNameFilter = productService.GetAllProductsByQuery(productName: productNameFilter);

            var resultWithCategoryFilter = productService.GetAllProductsByQuery(categoryId: categoryIdFilter);

            var resultWithBothFilters = productService.GetAllProductsByQuery(productName: productNameFilter, categoryId: categoryIdFilter);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultWithoutFilters, Is.Not.Null);
                Assert.That(resultWithoutFilters.Count(), Is.EqualTo(testProducts.Count));

                Assert.That(resultWithNameFilter, Is.Not.Null);
                Assert.That(resultWithNameFilter.All(p => p.Name.Contains(productNameFilter)));

                Assert.That(resultWithCategoryFilter, Is.Not.Null);
                Assert.That(resultWithCategoryFilter.All(p => p.CategoryId == categoryIdFilter));

                Assert.That(resultWithBothFilters, Is.Not.Null);
                Assert.That(resultWithBothFilters.All(p => p.Name.Contains(productNameFilter) && p.CategoryId == categoryIdFilter));
            });
        }
    }
}
