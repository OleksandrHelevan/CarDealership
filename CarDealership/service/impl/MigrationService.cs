using CarDealership.config;
using CarDealership.service;
using CarDealership.entity;
using CarDealership.enums;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.service.impl
{
    public class MigrationService : IMigrationService
    {
        private readonly DealershipContext _context;

        public MigrationService(DealershipContext context)
        {
            _context = context;
        }

        public async Task EnsureDatabaseCreatedAsync()
        {
            try
            {
                await _context.Database.EnsureCreatedAsync();
                Console.WriteLine("✅ Database created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error creating database: {ex.Message}");
                throw;
            }
        }

        public async Task ApplyMigrationsAsync()
        {
            try
            {
                var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();

                if (pendingMigrations.Any())
                {
                    Console.WriteLine($"⚙️ Applying {pendingMigrations.Count()} pending migrations...");
                    await _context.Database.MigrateAsync();
                    Console.WriteLine("✅ Migrations applied successfully.");
                }
                else
                {
                    Console.WriteLine("✅ No pending migrations.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error applying migrations: {ex.Message}");
                throw;
            }
        }

        public async Task SeedInitialDataAsync()
        {
            try
            {
                if (await _context.Users.AnyAsync())
                {
                    Console.WriteLine("ℹ️ Database already seeded, skipping.");
                    return;
                }

                Console.WriteLine("🌱 Seeding initial data...");

                // === Admin User ===
                var adminUser = new User
                {
                    Login = "admin",
                    PasswordHash = CarDealership.config.decoder.DealershipPasswordEncoder.Encode("admin123"),
                    AccessRight = AccessRight.Admin,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Users.AddAsync(adminUser);
                await _context.SaveChangesAsync();

                // === Engines ===
                var gasolineEngine = new GasolineEngine
                {
                    Power = 150.5m,
                    FuelType = FuelType.Petrol,
                    FuelConsumption = 8.5m,
                    Displacement = 2.0m,
                    Cylinders = 4,
                    CreatedAt = DateTime.UtcNow
                };

                var electroEngine = new ElectroEngine
                {
                    Power = 200.0m,
                    BatteryCapacity = 75.0m,
                    Range = 400,
                    MotorType = ElectroMotorType.Asynchronous,
                    ChargingTime = 8.5m,
                    MaxChargingPower = 11.0m,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.GasolineEngines.AddAsync(gasolineEngine);
                await _context.ElectroEngines.AddAsync(electroEngine);
                await _context.SaveChangesAsync();

                // === Cars ===
                var gasolineCar = new GasolineCar
                {
                    Brand = "Toyota",
                    ModelName = "Camry",
                    Color = Color.White,
                    Mileage = 50000,
                    Price = 25000.00m,
                    Weight = 1500,
                    DriveType = DriveType.FWD,
                    Transmission = TransmissionType.Automatic,
                    Year = 2020,
                    NumberOfDoors = 4,
                    BodyType = CarBodyType.Sedan,
                    CarType = CarType.Gasoline,
                    EngineId = gasolineEngine.Id,
                    CreatedAt = DateTime.UtcNow
                };

                var electroCar = new ElectroCar
                {
                    Brand = "Tesla",
                    ModelName = "Model 3",
                    Color = Color.Black,
                    Mileage = 30000,
                    Price = 45000.00m,
                    Weight = 1800,
                    DriveType = DriveType.RWD,
                    Transmission = TransmissionType.Automatic,
                    Year = 2021,
                    NumberOfDoors = 4,
                    BodyType = CarBodyType.Sedan,
                    CarType = CarType.Electro,
                    EngineId = electroEngine.Id,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Cars.AddRangeAsync(gasolineCar, electroCar);
                await _context.SaveChangesAsync();

                // === Products ===
                var products = new List<Product>
                {
                    new Product
                    {
                        ProductNumber = "PROD-001",
                        CountryOfOrigin = "Japan",
                        InStock = true,
                        AvailableFrom = DateTime.UtcNow,
                        CarId = gasolineCar.Id,
                        CarType = CarType.Gasoline,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        ProductNumber = "PROD-002",
                        CountryOfOrigin = "USA",
                        InStock = true,
                        AvailableFrom = DateTime.UtcNow,
                        CarId = electroCar.Id,
                        CarType = CarType.Electro,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                await _context.Products.AddRangeAsync(products);
                await _context.SaveChangesAsync();

                Console.WriteLine("✅ Initial data seeded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error seeding initial data: {ex.Message}");
                throw;
            }
        }

        // === Migration Utilities ===

        public bool MigrateGasolineCarsToProducts()
        {
            try
            {
                var gasolineCars = _context.Cars.OfType<GasolineCar>().ToList();
                int migratedCount = 0;

                foreach (var car in gasolineCars)
                {
                    bool exists = _context.Products.Any(p => p.CarId == car.Id && p.CarType == CarType.Gasoline);
                    if (!exists)
                    {
                        _context.Products.Add(new Product
                        {
                            ProductNumber = $"PROD-GAS-{car.Id:D5}-{DateTime.UtcNow:yyyyMMdd}",
                            CountryOfOrigin = "Unknown",
                            InStock = true,
                            AvailableFrom = DateTime.UtcNow,
                            CarId = car.Id,
                            CarType = CarType.Gasoline,
                            CreatedAt = DateTime.UtcNow
                        });
                        migratedCount++;
                    }
                }

                _context.SaveChanges();
                Console.WriteLine($"✅ Migrated {migratedCount} gasoline cars to products.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error migrating gasoline cars: {ex.Message}");
                return false;
            }
        }

        public bool MigrateElectroCarsToProducts()
        {
            try
            {
                var electroCars = _context.Cars.OfType<ElectroCar>().ToList();
                int migratedCount = 0;

                foreach (var car in electroCars)
                {
                    bool exists = _context.Products.Any(p => p.CarId == car.Id && p.CarType == CarType.Electro);
                    if (!exists)
                    {
                        _context.Products.Add(new Product
                        {
                            ProductNumber = $"PROD-ELEC-{car.Id:D5}-{DateTime.UtcNow:yyyyMMdd}",
                            CountryOfOrigin = "Unknown",
                            InStock = true,
                            AvailableFrom = DateTime.UtcNow,
                            CarId = car.Id,
                            CarType = CarType.Electro,
                            CreatedAt = DateTime.UtcNow
                        });
                        migratedCount++;
                    }
                }

                _context.SaveChanges();
                Console.WriteLine($"✅ Migrated {migratedCount} electro cars to products.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error migrating electro cars: {ex.Message}");
                return false;
            }
        }
    }
}
