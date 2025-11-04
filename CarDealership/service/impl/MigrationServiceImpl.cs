using CarDealership.repo;

namespace CarDealership.service.impl
{
    public class MigrationServiceImpl : IMigrationService
    {
        private readonly IProductRepository _productRepository;

        public MigrationServiceImpl(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
