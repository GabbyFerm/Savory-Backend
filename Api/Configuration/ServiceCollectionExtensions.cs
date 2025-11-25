using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Api.Configuration;

/// <summary>
/// Extension methods for configuring API-specific services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures Swagger/OpenAPI
    /// </summary>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Clean Architecture Boilerplate API",
                Version = "v1",
                Description = "A production-ready boilerplate Web API following Clean Architecture principles with CQRS, MediatR, and FluentValidation",
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            // Enable XML comments in Swagger
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }

    /// <summary>
    /// Configures CORS policies
    /// </summary>
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            // Development policy - allow everything
            options.AddPolicy("Development", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });

            // Production policy - restrictive
            options.AddPolicy("Production", policy =>
            {
                policy.WithOrigins("https://your-frontend-domain.com")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        return services;
    }
}