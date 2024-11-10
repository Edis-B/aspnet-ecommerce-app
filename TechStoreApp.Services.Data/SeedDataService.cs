using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Services.Data
{
    public class SeedDataService : ISeedDataService
    {
        private readonly IRepository<Category, int> categoryRepository;
        private readonly IRepository<Product, int> productRepository;
        private readonly IRepository<Status, int> statusRepository;
        private readonly RoleManager<IdentityRole> roleManager;
        public SeedDataService(IRepository<Category, int> _categoryRepository,
            IRepository<Product, int> _productRepository,
            IRepository<Status, int> _statusRepository,
            RoleManager<IdentityRole> _roleManager)
        {
            categoryRepository = _categoryRepository;
            productRepository = _productRepository;
            statusRepository = _statusRepository;
            roleManager = _roleManager;
        }
        public async Task SeedAllData()
        {
            await SeedRoles();
            await SeedCategories();
            await SeedProducts();
            await SeedStatuses();
        }
        public async Task SeedRoles()
        {
            if (!roleManager.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                    new IdentityRole { Name = "User", NormalizedName = "USER" },
                    new IdentityRole { Name = "Moderator", NormalizedName = "MODERATOR" }
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }

        public async Task SeedCategories()
        {
            if (!categoryRepository.GetAll().Any())
            {
                categoryRepository.AddRangeAsync(SeedDataCategories.GetCategories());
            }
        }

        public async Task SeedProducts()
        {
            if (!productRepository.GetAll().Any())
            {
                productRepository.AddRangeAsync(SeedDataProducts.GetProducts());
            }
        }

        public async Task SeedStatuses()
        {
            if (!statusRepository.GetAll().Any())
            {
                statusRepository.AddRangeAsync(SeedDataStatuses.GetStatuses());
            }
        }
    }
}
