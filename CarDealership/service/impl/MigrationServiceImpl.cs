using CarDealership.repo;

namespace CarDealership.service.impl
{
    public class MigrationServiceImpl : IMigrationService
    {

        public MigrationServiceImpl()
        {
        }

        public bool MigrateGasolineCarsToProducts()
        {
            return true;
        }

        public bool MigrateElectroCarsToProducts()
        {
            return true;
        }
        
    }
}
