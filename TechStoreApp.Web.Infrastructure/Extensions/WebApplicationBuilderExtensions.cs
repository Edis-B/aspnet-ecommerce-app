using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechStoreApp.Data;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Web.Infrastructure.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> SeedDefaultData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var seedDataService = scope.ServiceProvider.GetRequiredService<ISeedDataService>();
                await seedDataService.SeedAllMissingData();
            }

            return app;
        }

        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

            TechStoreDbContext dbContext = serviceScope
                .ServiceProvider
                .GetRequiredService<TechStoreDbContext>()!;

            var pendingMigrations = dbContext.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }

            return app;
        }
    }
}
