using Microsoft.EntityFrameworkCore;
using PharmaCareBlazor.Components;
using PharmaCareBlazor.Data;
using PharmaCareBlazor.Services.Implementations;
using PharmaCareBlazor.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Database configuration with SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                      ?? "Data Source=pharmacare.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Register Application Services (Dependency Injection - SOLID)
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReportService, ReportService>();

var app = builder.Build();

// Automatically ensure database is created and seed initial sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.InitializeAsync(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
