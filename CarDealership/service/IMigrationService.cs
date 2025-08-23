namespace CarDealership.service
{
    public interface IMigrationService
    {
        bool MigrateGasolineCarsToProducts();
        bool MigrateElectroCarsToProducts();
    }
}
