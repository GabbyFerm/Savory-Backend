using Api.Configuration;
using Api.Middleware;
using Application;
using Infrastructure;
using Serilog;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure logging
            LoggingConfiguration.ConfigureSerilog();

            try
            {
                Log.Information("Starting Savory API");

                var builder = WebApplication.CreateBuilder(args);

                // Configure Serilog
                builder.Host.UseSerilog();

                // Add services to the container
                ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

                var app = builder.Build();

                // Configure the HTTP request pipeline
                ConfigurePipeline(app);

                Log.Information("Savory API started successfully");

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
                throw; // Re-throw to see the actual error
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            // API Layer - Controllers and API-specific services
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerConfiguration();
            services.AddCorsConfiguration();

            // Application Layer - MediatR, FluentValidation, Behaviours
            services.AddApplicationServices();

            // Infrastructure Layer - Database, Repositories, External Services
            services.AddInfrastructureServices(configuration);
        }

        private static void ConfigurePipeline(WebApplication app)
        {
            // Development-only middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Global exception handling (must be early in pipeline)
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Standard middleware
            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();

            // CORS - use appropriate policy based on environment
            var corsPolicy = app.Environment.IsDevelopment() ? "Development" : "Production";
            app.UseCors(corsPolicy);

            // Authentication & Authorization 
            app.UseAuthentication();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            // Health check endpoint
            app.MapGet("/health", () => Results.Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                environment = app.Environment.EnvironmentName
            }))
                .ExcludeFromDescription();
        }
    }
}