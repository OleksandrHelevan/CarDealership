using CarDealership.config;
using CarDealership.service;
using CarDealership.service.impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CarDealership.service
{
    public interface IDatabaseInitializationService
    {
        Task InitializeAsync();
    }

    public class DatabaseInitializationService : IDatabaseInitializationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseInitializationService> _logger;

        public DatabaseInitializationService(IServiceProvider serviceProvider, ILogger<DatabaseInitializationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                _logger.LogInformation("Starting database initialization...");

                using var scope = _serviceProvider.CreateScope();
                var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationService>();

                // Ensure database exists
                await migrationService.EnsureDatabaseCreatedAsync();

                // Apply any pending migrations
                await migrationService.ApplyMigrationsAsync();

                // Seed initial data if needed
                await migrationService.SeedInitialDataAsync();

                _logger.LogInformation("Database initialization completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during database initialization");
                throw;
            }
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
        {
            // Add DbContext
            services.AddDbContext<DealershipContext>(options =>
            {
                options.UseNpgsql("Host=localhost;Port=5432;Database=car_dealership;Username=postgres;Password=1234qwer");
                options.EnableSensitiveDataLogging(false);
                options.EnableServiceProviderCaching();
                options.EnableDetailedErrors(false);
            });

            // Add services
            services.AddScoped<IMigrationService, MigrationService>();
            services.AddScoped<IDatabaseInitializationService, DatabaseInitializationService>();

            return services;
        }
    }
}
