using CarDealership.config;
using CarDealership.entity;
using CarDealership.enums;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.repo.impl
{
    public class ProductRepositoryImpl : IProductRepository
    {
        private readonly DealershipContext _context;

        public ProductRepositoryImpl(DealershipContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            // EF сам підвантажить GasolineCar або ElectroCar через дискримінатор
            return _context.Products
                .Include(p => p.Car)
                .ThenInclude(c => (c as GasolineCar).Engine)
                .Include(p => p.Car)
                .ThenInclude(c => (c as ElectroCar).Engine)
                .AsSplitQuery() // уникнення картезіанського вибуху
                .AsNoTracking()
                .ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products
                .Include(p => p.Car)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public bool ExistsByNumber(string number)
        {
            return _context.Products.Any(p => p.ProductNumber == number);
        }

        public GasolineCar? GetGasolineCarById(int id)
        {
            // У TPH — усі авто в Cars, тому використовуємо OfType
            return _context.Cars
                .OfType<GasolineCar>()
                .Include(c => c.Engine)
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == id);
        }

        public ElectroCar? GetElectroCarById(int id)
        {
            return _context.Cars
                .OfType<ElectroCar>()
                .Include(c => c.Engine)
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Product> GetByVehicleIds(List<int> vehicleIds, CarType carType)
        {
            return _context.Products
                .Include(p => p.Car)
                .Where(p => p.CarType == carType && vehicleIds.Contains(p.CarId))
                .AsSplitQuery()
                .AsNoTracking()
                .ToList();
        }
    }
}
