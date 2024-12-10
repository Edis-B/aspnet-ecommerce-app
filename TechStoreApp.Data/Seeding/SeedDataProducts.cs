using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Seeding.DataTransferObjects;

namespace TechStoreApp.Data.Seeding
{
    public static class SeedDataProducts
    {
        public static async Task<List<Product>> GetProductsAsync(string jsonPath)
        {
            try
            {
                string jsonInput = await File
                    .ReadAllTextAsync(jsonPath, Encoding.ASCII, CancellationToken.None);

                ImportProductDTO[] productDtos =
                    JsonSerializer.Deserialize<ImportProductDTO[]>(jsonInput)!;

                var result = new List<Product>();

                foreach (ImportProductDTO productDto in productDtos)
                {
                    if (!IsValidObject(productDto))
                    {
                        continue;
                    }

                    Product product = new()
                    {
                        ProductId = productDto.ProductId,
                        CategoryId = productDto.CategoryId,
                        Name = productDto.Name,
                        Description = productDto.Description,
                        Price = productDto.Price,
                        Stock = productDto.Stock,
                        ImageUrl = productDto.ImageUrl,
                        IsFeatured = productDto.IsFeatured,
                        AddedDate = DateTime.UtcNow
                    };

                    result.Add(product);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while seeding the products in the database! - {ex.Message}");
            }

            return new List<Product>();
        }
        private static bool IsValidObject(object obj)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            var context = new ValidationContext(obj);
            var isValid = Validator.TryValidateObject(obj, context, validationResults);
            return isValid;
        }
    }
}
