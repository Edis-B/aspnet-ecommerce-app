using Microsoft.AspNetCore.Identity;
using TechStoreApp.Data.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Common;

namespace TechStoreApp.Services.Data
{
    public class SeedDataService : ISeedDataService
    {
        private readonly IRepository<Category, int> categoryRepository;
        private readonly IRepository<Product, int> productRepository;
        private readonly IRepository<Status, int> statusRepository;
        private readonly IRepository<PaymentDetail, int> paymentDetailRepository;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        public SeedDataService(IRepository<Category, int> _categoryRepository,
            IRepository<Product, int> _productRepository,
            IRepository<Status, int> _statusRepository,
            IRepository<PaymentDetail, int> _paymentDetailRepository,
            RoleManager<ApplicationRole> _roleManager,
            UserManager<ApplicationUser> _userManager,
            IUserService _userService)
        {
            categoryRepository = _categoryRepository;
            productRepository = _productRepository;
            statusRepository = _statusRepository;
            paymentDetailRepository = _paymentDetailRepository;
            roleManager = _roleManager;
            userManager = _userManager;
            userService = _userService;
        }
        public async Task SeedAllMissingData()
        {
            await SeedRoles();
            await SeedAccounts();
            await SeedCategories();
            await SeedProducts();
            await SeedStatuses();
            await SeedPaymentDetails();
        }
        public async Task SeedPaymentDetails()
        {
            if (!paymentDetailRepository.GetAll().Any())
            {
                await paymentDetailRepository.AddRangeAsync(SeedDataPaymentTypes.GetPaymentTypes());
            }
        }
        public async Task SeedRoles()
        {
            if (!roleManager.Roles.Any())
            {
                var roles = new List<ApplicationRole>
                {
                    new ApplicationRole { Name = GeneralConstraints.AdminRoleName },
                    new ApplicationRole { Name = "User" },
                    new ApplicationRole { Name = "Moderator" }
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }
        public async Task SeedAccounts()
        {
            var Admins = await userManager.GetUsersInRoleAsync(GeneralConstraints.AdminRoleName);

            if (!Admins.Any())
            {
                var user = userService.CreateUser();

                user.UserName = "Administrator";
                user.Email = "example@gmail.com";
                user.ProfilePictureUrl = "https://i.pinimg.com/originals/c0/27/be/c027bec07c2dc08b9df60921dfd539bd.webp";

                var result = await userManager.CreateAsync(user, "Administrator");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, GeneralConstraints.AdminRoleName);
                }
            }
        }

        public async Task SeedCategories()
        {
            if (!categoryRepository.GetAll().Any())
            {
                await categoryRepository.AddRangeAsync(SeedDataCategories.GetCategories());
            }
        }

        public async Task SeedProducts()
        {
            if (!productRepository.GetAll().Any())
            {
                await productRepository.AddRangeAsync(SeedDataProducts.GetProducts());
            }
        }

        public async Task SeedStatuses()
        {
            if (!statusRepository.GetAll().Any())
            {
                await statusRepository.AddRangeAsync(SeedDataStatuses.GetStatuses());
            }
        }

    }
}
