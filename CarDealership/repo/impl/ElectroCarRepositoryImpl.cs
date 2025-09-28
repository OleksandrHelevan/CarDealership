using CarDealership.entity;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarDealership.repo.impl
{
    public class ElectroCarRepositoryImpl : IElectroCarRepository
    {
        private readonly DealershipContext _context;

        public ElectroCarRepositoryImpl(DealershipContext context)
        {
            _context = context;
        }

        public IEnumerable<ElectroCar> GetAll()
        {
            return _context.ElectroCars.Include(c => c.Engine).ToList();
        }

        public IEnumerable<ElectroCar> GetFiltered(ElectroCarFilterDto filter)
        {
            var query = _context.ElectroCars.Include(c => c.Engine).AsQueryable();

            if (filter.Id.HasValue)
            {
                query = query.Where(c => c.Id == filter.Id.Value);
            }

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                query = query.Where(c => 
                    c.Brand.Contains(filter.SearchText) || 
                    c.ModelName.Contains(filter.SearchText));
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

            if (filter.MotorType.HasValue)
            {
                query = query.Where(c => c.Engine.MotorType == filter.MotorType.Value);
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

        public ElectroCar? GetById(int id)
        {
            return _context.ElectroCars.Include(c => c.Engine).FirstOrDefault(c => c.Id == id);
        }

        public void Add(ElectroCar car)
        {
            _context.ElectroCars.Add(car);
            _context.SaveChanges();
        }

        public void Update(ElectroCar car)
        {
            _context.ElectroCars.Update(car);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var car = _context.ElectroCars.FirstOrDefault(c => c.Id == id);
            if (car != null)
            {
                _context.ElectroCars.Remove(car);
                _context.SaveChanges();
            }
        }
    }
}