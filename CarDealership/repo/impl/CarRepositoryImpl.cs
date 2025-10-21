using System.Collections.Generic;
using System.Linq;
using CarDealership.config;
using CarDealership.entity;
using CarDealership.enums;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.repo.impl
{
    public class CarRepositoryImpl : ICarRepository
    {
        private readonly DealershipContext _context;

        public CarRepositoryImpl(DealershipContext context)
        {
            _context = context;
        }

        public IEnumerable<Car> GetAll()
        {
            return _context.Set<Car>()
                .Include(c => c.Engine)
                .ToList();
        }

        public Car? GetById(int id)
        {
            return _context.Set<Car>()
                .Include(c => c.Engine)
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Car> GetByType(CarType type)
        {
            return _context.Set<Car>()
                .Include(c => c.Engine)
                .Where(c => c.CarType == type)
                .ToList();
        }

        public void Add(Car car)
        {
            _context.Set<Car>().Add(car);
            _context.SaveChanges();
        }

        public void Update(Car car)
        {
            _context.Set<Car>().Update(car);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var car = _context.Set<Car>().FirstOrDefault(c => c.Id == id);
            if (car != null)
            {
                _context.Set<Car>().Remove(car);
                _context.SaveChanges();
            }
        }
    }
}

