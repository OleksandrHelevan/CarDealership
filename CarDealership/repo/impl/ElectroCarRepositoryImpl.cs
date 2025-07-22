using CarDealership.entity;
using CarDealership.config;


namespace CarDealership.repo.impl
{
    public class ElectroCarRepository : IElectroCarRepository
    {
        private readonly DealershipContext _context;

        public ElectroCarRepository(DealershipContext context)
        {
            _context = context;
        }

        public IEnumerable<ElectroCar> GetAll()
        {
            return _context.ElectroCars.ToList();
        }

        public ElectroCar? GetById(int id)
        {
            return _context.ElectroCars.FirstOrDefault(c => c.Id == id);
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