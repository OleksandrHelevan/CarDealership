namespace CarDealership.service
{
    public interface IMigrationService
    {
        Task EnsureDatabaseCreatedAsync();
        Task ApplyMigrationsAsync();
        Task SeedInitialDataAsync();
        
    }
}
