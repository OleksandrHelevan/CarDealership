using CarDealership.entity;
using CarDealership.enums;
using CarDealership.repo;

namespace CarDealership.service.impl
{
    public class MigrationServiceImpl : IMigrationService
    {
        private readonly IProductRepository _productRepository;
        private readonly IGasolineCarRepository _gasolineCarRepository;
        private readonly IElectroCarRepository _electroCarRepository;

        public MigrationServiceImpl(
            IProductRepository productRepository,
            IGasolineCarRepository gasolineCarRepository,
            IElectroCarRepository electroCarRepository)
        {
            _productRepository = productRepository;
            _gasolineCarRepository = gasolineCarRepository;
            _electroCarRepository = electroCarRepository;
        }

        public bool MigrateGasolineCarsToProducts()
        {
            try
            {
                var gasolineCars = _gasolineCarRepository.GetAll();
                int migratedCount = 0;

                foreach (var car in gasolineCars)
                {
                    // Check if product already exists for this car
                    var existingProduct = _productRepository.GetAll()
                        .FirstOrDefault(p => p.GasolineCarId == car.Id);

                    if (existingProduct == null)
                    {
                        var product = new Product
                        {
                            Number = GenerateProductNumber(car.Id, "GAS"),
                            CountryOfOrigin = "Україна",
                            InStock = true,
                            AvailableFrom = DateTime.Now,
                            GasolineCarId = car.Id
                        };

                        _productRepository.Add(product);
                        migratedCount++;
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Migrated {migratedCount} gasoline cars to products");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Migration error: {ex.Message}");
                return false;
            }
        }

        public bool MigrateElectroCarsToProducts()
        {
            try
            {
                var electroCars = _electroCarRepository.GetAll();
                int migratedCount = 0;

                foreach (var car in electroCars)
                {
                    // Check if product already exists for this car
                    var existingProduct = _productRepository.GetAll()
                        .FirstOrDefault(p => p.ElectroCarId == car.Id);

                    if (existingProduct == null)
                    {
                        var product = new Product
                        {
                            Number = GenerateProductNumber(car.Id, "ELEC"),
                            CountryOfOrigin = "Україна",
                            InStock = true,
                            AvailableFrom = DateTime.Now,
                            ElectroCarId = car.Id
                        };

                        _productRepository.Add(product);
                        migratedCount++;
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Migrated {migratedCount} electro cars to products");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Migration error: {ex.Message}");
                return false;
            }
        }

        private string GenerateProductNumber(int carId, string prefix)
        {
            return $"PROD-{prefix}-{carId:D6}-{DateTime.Now:yyyyMMdd}";
        }
    }
}
