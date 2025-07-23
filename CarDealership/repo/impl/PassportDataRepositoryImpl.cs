using CarDealership.config;
using CarDealership.entity;


namespace CarDealership.repo.impl
{
    public class PassportDataRepositoryImpl : IPassportDataRepository
    {
        private readonly DealershipContext _context;

        public PassportDataRepositoryImpl()
        {
            _context = new DealershipContext();
        }

        public IEnumerable<PassportData> GetAll()
        {
            return _context.Set<PassportData>().ToList();
        }

        public PassportData? GetById(int id)
        {
            return _context.Set<PassportData>().FirstOrDefault(p => p.Id == id);
        }

        public PassportData? GetByPassportNumber(string passportNumber)
        {
            return _context.Set<PassportData>().FirstOrDefault(p => p.PassportNumber == passportNumber);
        }

        public void Add(PassportData passportData)
        {
            _context.Set<PassportData>().Add(passportData);
            _context.SaveChanges();
        }

        public void Update(PassportData passportData)
        {
            _context.Set<PassportData>().Update(passportData);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Set<PassportData>().Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}