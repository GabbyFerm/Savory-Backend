using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Tests.InfrastructureTests;

/// <summary>
/// Custom WebApplicationFactory for integration testing with in-memory database
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Add test JWT configuration
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "ThisIsATestSecretKeyForJWTTokensMinimum32Characters!",
                ["Jwt:Issuer"] = "SavoryAPI",
                ["Jwt:Audience"] = "SavoryAPI",
                ["Jwt:ExpiresInMinutes"] = "60"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            // Add in-memory database
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });

            // Reconfigure JWT authentication to use test settings
            var jwtDescriptor = services.SingleOrDefault(
                d => d.ServiceType.Name == "IConfigureOptions`1" &&
                     d.ImplementationType?.Name.Contains("JwtBearer") == true);

            if (jwtDescriptor != null)
            {
                services.Remove(jwtDescriptor);
            }

            // Re-add JWT authentication with test key
            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "SavoryAPI",
                    ValidAudience = "SavoryAPI",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("ThisIsATestSecretKeyForJWTTokensMinimum32Characters!")),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Build service provider and ensure database is created
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();
        });
    }
}