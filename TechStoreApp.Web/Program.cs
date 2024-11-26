using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.Infrastructure.Extensions;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddApplicationDatabase(builder.Configuration);
        builder.Services.AddApplicationIdentity(builder.Configuration);

        builder.Services.AddApplicationServices(typeof(ICartService));
        builder.Services.AddApplicationServicesExtra(builder.Configuration);

        builder.Services.AddRazorPages();

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.ApplyMigrations();
  await app.SeedDefaultData();

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}"
        );

        app.MapControllerRoute(
            name: "Default",
            pattern: "{controller=Home}/{action=Index}"
        );

        app.MapRazorPages();

        app.Run();
    }
}