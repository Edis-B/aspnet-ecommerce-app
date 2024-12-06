using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Data.Repository;
using TechStoreApp.Data;
using TechStoreApp.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using static TechStoreApp.Common.GeneralConstraints;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDatabase(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<TechStoreDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;

                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<TechStoreDbContext>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.SlidingExpiration = true; // Enable sliding expiration
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(SlidingExpirationInMinutes); // Set expiration window to 30 minutes
                    
                });

            services.AddScoped<IRepository<Address, int>, Repository<Address, int>>();
            services.AddScoped<IRepository<ApplicationUser, Guid>, Repository<ApplicationUser, Guid>>();
            services.AddScoped<IRepository<Cart, int>, Repository<Cart, int>>();
            services.AddScoped<IRepository<CartItem, object>, Repository<CartItem, object>>();
            services.AddScoped<IRepository<Category, int>, Repository<Category, int>>();
            services.AddScoped<IRepository<Favorited, object>, Repository<Favorited, object>>();
            services.AddScoped<IRepository<Order, int>, Repository<Order, int>>();
            services.AddScoped<IRepository<OrderDetail, int>, Repository<OrderDetail, int>>();
            services.AddScoped<IRepository<Product, int>, Repository<Product, int>>();
            services.AddScoped<IRepository<Review, int>, Repository<Review, int>>();
            services.AddScoped<IRepository<Status, int>, Repository<Status, int>>();
            services.AddScoped<IRepository<PaymentDetail, int>, Repository<PaymentDetail, int>>();

            return services;
        }

        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.Name = MyCookieName;
                options.Cookie.Domain = DomainStr;
                options.Cookie.Path = "/";

                options.LoginPath = $"/Account/Login";
            });

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"C:\Keys"))
                .DisableAutomaticKeyGeneration()
                .SetApplicationName("MyTechStoreApp"); ;

            return services;
        }

        public static IServiceCollection AddApplicationServicesExtra(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IEmailSender<ApplicationUser>, MyEmailSender>();

            return services;
        }

        public static void AddApplicationServices(this IServiceCollection services, Type serviceType)
        {
            Assembly? serviceAssembly = Assembly.GetAssembly(serviceType);
            if (serviceAssembly == null)
            {
                throw new InvalidOperationException("Invalid service type provided!");
            }

            Type[] implementationTypes = serviceAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
                .ToArray();

            foreach (Type implementationType in implementationTypes)
            {
                Type? interfaceType = implementationType
                    .GetInterface($"I{implementationType.Name}");
                if (interfaceType == null)
                {
                    throw new InvalidOperationException(
                        $"No interface is provided for the service with name: {implementationType.Name}");
                }

                services.AddScoped(interfaceType, implementationType);
            }
        }
    }
}
