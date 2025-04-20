using InventoryHubAPI.Data; 
using System.Data.Common;
using InventoryHubAPI.Service.Implementations;
using InventoryHubAPI.Service.Interfaces;
using InventoryHubAPI.Reporitory.Implementations;
using InventoryHubAPI.Reporitory.Interfaces;
using InventoryHubAPI.Shared;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using InventoryHubAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Register the correct SQL Server provider (Microsoft.Data.SqlClient)
DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDatabaseConnection, DatabaseConnection>();
builder.Services.AddScoped<IProduct, Product>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<ILoggerService, LoggerService>();
builder.Services.AddScoped<ILoggerRepository, LoggerRepository>();

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings")
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", builder =>
    {
        builder.WithOrigins("http://localhost:5081/")  // URL of your Blazor client
               .AllowAnyMethod()                    // Allow any HTTP method (GET, POST, etc.)
               .AllowAnyHeader();                   // Allow any headers in the request
    });
});

var app = builder.Build();

#region This portion is for database connection, environment, and data provider checker or viewer for testing purposes only. It can be removed.
app.MapGet("/provider", (IOptions<AppSettings> appSettings) =>
{
    return $"Database Provider: {appSettings.Value.DatabaseProvider}";
});

app.MapGet("/env", (IWebHostEnvironment env) =>
{
    return $"Current Environment: {env.EnvironmentName}";
});

app.MapGet("/config-check", (IConfiguration config) =>
{
    var conn = config.GetConnectionString("DefaultConnection");
    return $"Current Connection String: {conn}";
});
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Display detailed errors in development
    app.UseSwagger();
    app.UseSwaggerUI();
}

DbProviderFactories.RegisterFactory("System.Data.SqlClient", Microsoft.Data.SqlClient.SqlClientFactory.Instance);


// Registered custom middleware in the pipeline. I use separation of concerns or SRP (Single Responsibility Principle) for each middleware.
app.UseMiddleware<SqlInjectionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.UseCors("AllowBlazorClient");

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS
app.UseAuthorization(); // Enables authorization middleware. This is also for [Authorize] attributes (can also add Authentication here)
app.MapControllers(); // Maps controller routes
app.Run();
