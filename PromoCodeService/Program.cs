using Microsoft.EntityFrameworkCore;
using PromoCodeService.Data;
using PromoCodeService.Data.Repositories;
using PromoCodeService.Hubs;
using PromoCodeService.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PromoCodeDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IPromoCodeRepository, PromoCodeRepository>();
builder.Services.AddSingleton<IPromoCodeValidator, PromoCodeValidator>();
builder.Services.AddSingleton<IPromoCodeGenerator, PromoCodeGenerator>();
builder.Services.AddScoped<IPromoCodeService, PromoCodeCreateService>();

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration["Cors:ClientUrl"])
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<PromoCodeDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error applying migrations: {ex.Message}");
    }
}

app.UseCors();

app.MapHub<PromoCodeGenerationHub>("/promoCodeGenerationHub");
app.MapHub<PromoCodeUsageHub>("/promoCodeUsageHub");

app.Run();
