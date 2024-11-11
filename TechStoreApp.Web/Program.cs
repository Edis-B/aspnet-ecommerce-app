using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Services.Data;
using TechStoreApp.Data.Repository;
using TechStoreApp.Data.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<TechStoreDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
		options.Password.RequireDigit = false;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireUppercase = false;
	})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TechStoreDbContext>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();    
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddScoped<IRepository<Address, int>, Repository<Address, int>>();
builder.Services.AddScoped<IRepository<ApplicationUser, Guid>, Repository<ApplicationUser, Guid>>();
builder.Services.AddScoped<IRepository<Cart, int>, Repository<Cart, int>>();
builder.Services.AddScoped<IRepository<CartItem, object>, Repository<CartItem, object>>();
builder.Services.AddScoped<IRepository<Category, int>, Repository<Category, int>>();
builder.Services.AddScoped<IRepository<Favorited, object>, Repository<Favorited, object>>();
builder.Services.AddScoped<IRepository<Order, int>, Repository<Order, int>>();
builder.Services.AddScoped<IRepository<OrderDetail, int>, Repository<OrderDetail, int>>();
builder.Services.AddScoped<IRepository<Product, int>, Repository<Product, int>>();
builder.Services.AddScoped<IRepository<Review, int>, Repository<Review, int>>();
builder.Services.AddScoped<IRepository<Status, int>, Repository<Status, int>>();

builder.Services.AddScoped<ISeedDataService, SeedDataService>();

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

using (var scope = app.Services.CreateScope())
{
    var seedDataService = scope.ServiceProvider.GetRequiredService<ISeedDataService>();
    await seedDataService.SeedAllData();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "productDetails",
    pattern: "Product/RedirectToDetails/{productId:int}"
);

app.MapControllerRoute(
    name: "editProductDetails",
    pattern: "Product/Edit/{productId:int}/Edit"
);

app.MapRazorPages();

app.Run();

