using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Load Ocelot configuration
builder.Configuration.AddJsonFile("./Configuration/ocelot.json", optional: false, reloadOnChange: true);

// Add Ocelot services
builder.Services.AddOcelot();

// Enable gRPC for services requiring HTTP/2
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true; // Needed for gRPC
});

// Add Authentication (if IdentityService is handling authentication)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://identityservice"; // Internal container hostname
        options.RequireHttpsMetadata = false;
        options.Audience = "ecommerce_api";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Enable authentication and authorization before using Ocelot
app.UseAuthentication();
app.UseAuthorization();
app.UseOcelot().Wait();

app.Run();
