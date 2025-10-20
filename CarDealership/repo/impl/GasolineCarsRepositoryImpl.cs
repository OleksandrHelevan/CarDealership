using CarDealership.entity;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.enums;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.repo.impl
{
    public class GasolineCarRepositoryImpl : IGasolineCarRepository
    {
        private readonly DealershipContext _context;

        public GasolineCarRepositoryImpl(DealershipContext context)
        {
            _context = context;
        }

        public IEnumerable<GasolineCar> GetAll()
        {
            return _context.Cars
                .OfType<GasolineCar>()
                .Include(c => c.Engine)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<GasolineCar> GetFiltered(GasolineCarFilterDto filter)
        {
            var query = _context.Cars
                .OfType<GasolineCar>()
                .Include(c => c.Engine)
                .AsQueryable();

            if (filter.Id.HasValue)
                query = query.Where(c => c.Id == filter.Id.Value);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
                query = query.Where(c =>
                    EF.Functions.ILike(c.Brand, $"%{filter.SearchText}%") ||
                    EF.Functions.ILike(c.ModelName, $"%{filter.SearchText}%"));

            if (filter.TransmissionType.HasValue)
                query = query.Where(c => c.Transmission == filter.TransmissionType.Value);

            if (filter.BodyType.HasValue)
                query = query.Where(c => c.BodyType == filter.BodyType.Value);

            if (filter.Color.HasValue)
                query = query.Where(c => c.Color == filter.Color.Value);

            if (filter.DriveType.HasValue)
                query = query.Where(c => c.DriveType == filter.DriveType.Value);

            if (filter.FuelType.HasValue)
                query = query.Where(c => c.Engine.FuelType == filter.FuelType.Value);

            if (filter.YearFrom.HasValue)
                query = query.Where(c => c.Year >= filter.YearFrom.Value);

            if (filter.YearTo.HasValue)
                query = query.Where(c => c.Year <= filter.YearTo.Value);

            if (filter.PriceFrom.HasValue)
                query = query.Where(c => c.Price >= (decimal)filter.PriceFrom.Value);

            if (filter.PriceTo.HasValue)
                query = query.Where(c => c.Price <= (decimal)filter.PriceTo.Value);

            if (filter.WeightFrom.HasValue)
                query = query.Where(c => c.Weight >= filter.WeightFrom.Value);

            if (filter.WeightTo.HasValue)
                query = query.Where(c => c.Weight <= filter.WeightTo.Value);

            if (filter.MileageFrom.HasValue)
                query = query.Where(c => c.Mileage >= filter.MileageFrom.Value);

            if (filter.MileageTo.HasValue)
                query = query.Where(c => c.Mileage <= filter.MileageTo.Value);

            if (filter.DoorsFrom.HasValue)
                query = query.Where(c => c.NumberOfDoors >= filter.DoorsFrom.Value);

            if (filter.DoorsTo.HasValue)
                query = query.Where(c => c.NumberOfDoors <= filter.DoorsTo.Value);

            return query.AsNoTracking().ToList();
        }

        public GasolineCar? GetById(int id)
        {
            return _context.Cars
                .OfType<GasolineCar>()
                .Include(c => c.Engine)
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == id);
        }

        public void Add(GasolineCar car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
        }

        public void Update(GasolineCar car)
        {
            _context.Cars.Update(car);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var car = _context.Cars.OfType<GasolineCar>().FirstOrDefault(c => c.Id == id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
        }
    }
}
