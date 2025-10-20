using CarDealership.service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CarDealership.service
{
    public class DatabaseStartupService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseStartupService> _logger;

        public DatabaseStartupService(IServiceProvider serviceProvider, ILogger<DatabaseStartupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting database initialization...");
                
                using var scope = _serviceProvider.CreateScope();
                var initializationService = scope.ServiceProvider.GetRequiredService<IDatabaseInitializationService>();
                
                await initializationService.InitializeAsync();
                
                _logger.LogInformation("Database initialization completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize database");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Database startup service stopped.");
            return Task.CompletedTask;
        }
    }
}
