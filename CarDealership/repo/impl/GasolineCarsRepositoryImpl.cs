using CarDealership.entity;
using CarDealership.config;
using CarDealership.dto;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarDealership.repo.impl
{
    public class GasolineCarRepository : IGasolineCarRepository
    {
        private readonly DealershipContext _context;

        public GasolineCarRepository(DealershipContext context)
        {
            _context = context;
        }

        public IEnumerable<GasolineCar> GetAll()
        {
            return _context.GasolineCars
                .Include(c => c.Engine)
                .ToList();
        }

        public GasolineCar? GetById(int id)
        {
            return _context.GasolineCars
                .Include(c => c.Engine)
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<GasolineCar> GetFiltered(GasolineCarFilterDto filter)
        {
            var query = _context.GasolineCars
                .Include(c => c.Engine)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                var searchText = filter.SearchText.ToLower();
                query = query.Where(c => 
                    c.Brand.ToLower().Contains(searchText) || 
                    c.ModelName.ToLower().Contains(searchText));
            }

            if (filter.TransmissionType.HasValue)
            {
                query = query.Where(c => c.Transmission == filter.TransmissionType.Value);
            }

            if (filter.BodyType.HasValue)
            {
                query = query.Where(c => c.BodyType == filter.BodyType.Value);
            }

            if (filter.Color.HasValue)
            {
                query = query.Where(c => c.Color == filter.Color.Value);
            }

            if (filter.DriveType.HasValue)
            {
                query = query.Where(c => c.DriveType == filter.DriveType.Value);
            }

            if (filter.FuelType.HasValue)
            {
                query = query.Where(c => c.Engine.FuelType == filter.FuelType.Value);
            }

            if (filter.YearFrom.HasValue)
            {
                query = query.Where(c => c.Year >= filter.YearFrom.Value);
            }
            if (filter.YearTo.HasValue)
            {
                query = query.Where(c => c.Year <= filter.YearTo.Value);
            }

            if (filter.PriceFrom.HasValue)
            {
                query = query.Where(c => c.Price >= filter.PriceFrom.Value);
            }
            if (filter.PriceTo.HasValue)
            {
                query = query.Where(c => c.Price <= filter.PriceTo.Value);
            }

            if (filter.WeightFrom.HasValue)
            {
                query = query.Where(c => c.Weight >= filter.WeightFrom.Value);
            }
            if (filter.WeightTo.HasValue)
            {
                query = query.Where(c => c.Weight <= filter.WeightTo.Value);
            }

            if (filter.MileageFrom.HasValue)
            {
                query = query.Where(c => c.Mileage >= filter.MileageFrom.Value);
            }
            if (filter.MileageTo.HasValue)
            {
                query = query.Where(c => c.Mileage <= filter.MileageTo.Value);
            }

            if (filter.DoorsFrom.HasValue)
            {
                query = query.Where(c => c.NumberOfDoors >= filter.DoorsFrom.Value);
            }
            if (filter.DoorsTo.HasValue)
            {
                query = query.Where(c => c.NumberOfDoors <= filter.DoorsTo.Value);
            }

            return query.ToList();
        }

        public void Add(GasolineCar car)
        {
            _context.GasolineCars.Add(car);
            _context.SaveChanges();
        }

        public void Update(GasolineCar car)
        {
            _context.GasolineCars.Update(car);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var car = _context.GasolineCars.FirstOrDefault(c => c.Id == id);
            if (car != null)
            {
                _context.GasolineCars.Remove(car);
                _context.SaveChanges();
            }
        }
    }
}