using CarDealership.entity;
using CarDealership.config;
using Microsoft.EntityFrameworkCore;

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