using System.Collections.Generic;
using System.Linq;
using CarDealership.config;
using CarDealership.entity;

namespace CarDealership.repo.impl
{
    public class EngineRepositoryImpl : IEngineRepository
    {
        private readonly DealershipContext _context;

        public EngineRepositoryImpl(DealershipContext context)
        {
            _context = context;
        }

        public IEnumerable<Engine> GetAll()
        {
            return _context.Set<Engine>().ToList();
        }

        public Engine? GetById(int id)
        {
            return _context.Set<Engine>().FirstOrDefault(e => e.Id == id);
        }

        public void Add(Engine engine)
        {
            _context.Set<Engine>().Add(engine);
            _context.SaveChanges();
        }

        public void Update(Engine engine)
        {
            _context.Set<Engine>().Update(engine);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var engine = _context.Set<Engine>().FirstOrDefault(e => e.Id == id);
            if (engine != null)
            {
                _context.Set<Engine>().Remove(engine);
                _context.SaveChanges();
            }
        }
    }
}

