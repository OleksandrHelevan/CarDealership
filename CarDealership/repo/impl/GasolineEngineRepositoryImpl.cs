using CarDealership.entity;
using CarDealership.config;

namespace CarDealership.repo.impl
{
    public class GasolineEngineRepository : IGasolineEngineRepository
    {
        private readonly DealershipContext _context;

        public GasolineEngineRepository(DealershipContext context)
        {
            _context = context;
        }

        public IEnumerable<GasolineEngine> GetAll()
        {
            return _context.GasolineEngines.ToList();
        }

        public GasolineEngine GetById(int id)
        {
            return _context.GasolineEngines.FirstOrDefault(e => e.Id == id) ?? new GasolineEngine();
        }

        public void Add(GasolineEngine engine)
        {
            _context.GasolineEngines.Add(engine);
            _context.SaveChanges();
        }

        public void Update(GasolineEngine engine)
        {
            _context.GasolineEngines.Update(engine);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var engine = GetById(id);
            if (engine != null)
            {
                _context.GasolineEngines.Remove(engine);
                _context.SaveChanges();
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}