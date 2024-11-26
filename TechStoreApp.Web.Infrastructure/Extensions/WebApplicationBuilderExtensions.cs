using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Web.Infrastructure.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static async Task<IServiceProvider> SeedDefaultData(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var seedDataService = scope.ServiceProvider.GetRequiredService<ISeedDataService>();
                await seedDataService.SeedAllMissingData();
            }

            return services;
        }
    }
}
