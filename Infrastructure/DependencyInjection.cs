using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        // Using InMemory database for easy setup
        // For production, uncomment and use SQL Server:
        // services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseSqlServer(
        //         configuration.GetConnectionString("DefaultConnection"),
        //         b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("BoilerplateDb"));

        // Register repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Add more infrastructure services here:
        // services.AddScoped<IEmailService, EmailService>();
        // services.AddScoped<IFileStorageService, FileStorageService>();

        return services;
    }
}