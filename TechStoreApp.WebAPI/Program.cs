using TechStoreApp.Services.Data.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string? techstoreWebAppOrigin = builder.Configuration.GetValue<string>("Client Origins:TechStoreApp");

        // Add services to the container.
        builder.Services.AddApplicationDatabase(builder.Configuration);
        builder.Services.AddApplicationIdentity(builder.Configuration);

        builder.Services.AddApplicationServices(typeof(ICartService));
        builder.Services.AddApplicationServicesExtra(builder.Configuration);

        builder.Services.AddControllers();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(cfg =>
        {
            cfg.AddPolicy("AllowAll", builder =>
            {
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });

            if (!String.IsNullOrWhiteSpace(techstoreWebAppOrigin))
            {
                cfg.AddPolicy("AllowMyServer", policyBld =>
                {
                    policyBld
                        .WithOrigins(techstoreWebAppOrigin)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            }
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        if (!String.IsNullOrWhiteSpace(techstoreWebAppOrigin))
        {
            app.UseCors("AllowMyServer");
        }

        app.MapControllers();

        app.Run();
    }
}