using CarDealership.config;
using CarDealership.entity;
using CarDealership.model;
using CarDealership.repo;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.repo.impl
{
    public class ElectroEngineRepository : IElectroEngineRepository
    {
        private readonly DealershipContext _context;

        public ElectroEngineRepository(DealershipContext context)
        {
            _context = context;
        }

        public IEnumerable<ElectroEngine> GetAll()
        {
            return _context.ElectroEngines.ToList();
        }

        public ElectroEngine GetById(int id)
        {
            return _context.ElectroEngines.Find(id);
        }

        public void Add(ElectroEngine engine)
        {
            _context.ElectroEngines.Add(engine);
        }

        public void Update(ElectroEngine engine)
        {
            _context.ElectroEngines.Update(engine);
        }

        public void Delete(int id)
        {
            var engine = _context.ElectroEngines.Find(id);
            if (engine != null)
            {
                _context.ElectroEngines.Remove(engine);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}