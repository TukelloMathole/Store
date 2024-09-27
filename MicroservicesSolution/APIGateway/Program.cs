using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.AddConsole();

// Load Ocelot configuration from the ocelot.json file
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add Ocelot services to the DI container
builder.Services.AddOcelot();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()        // You can restrict to specific origins if needed
               .AllowAnyMethod()        // Allow GET, POST, PUT, DELETE, etc.
               .AllowAnyHeader();       // Allow any headers
    });
});

var app = builder.Build();

// Enable CORS
app.UseCors("CorsPolicy");

// Optional: Enable HTTPS redirection
app.UseHttpsRedirection();

// Use Ocelot middleware
await app.UseOcelot();

// Run the application
app.Run();
