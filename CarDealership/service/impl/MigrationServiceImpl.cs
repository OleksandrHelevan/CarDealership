using CarDealership.entity;
using CarDealership.enums;
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
            // Legacy migration removed — unified schema already in place
            return true;
        }

        public bool MigrateElectroCarsToProducts()
        {
            // Legacy migration removed — unified schema already in place
            return true;
        }

        private string GenerateProductNumber(int carId, string prefix)
        {
            return $"PROD-{prefix}-{carId:D6}-{DateTime.Now:yyyyMMdd}";
        }
    }
}
