using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Models;

namespace TechStoreApp.Data.Data
{
    public static class SeedDataCategories
    {
        public static List<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category { Description = "Prebuilt Configuration", IsFeatured = true },
                new Category { Description = "Graphics Card" },
                new Category { Description = "Processor", IsFeatured = true },
                new Category { Description = "Processor Cooler" },
                new Category { Description = "PC Case", IsFeatured = true },
                new Category { Description = "Motherboard" },
                new Category { Description = "RAM" },
                new Category { Description = "SSD" },
                new Category { Description = "HDD" },
                new Category { Description = "Powersupply" }
            };
        }
    }
}
