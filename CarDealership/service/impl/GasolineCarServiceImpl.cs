using CarDealership.entity;
using CarDealership.repo;

namespace CarDealership.service.impl
{
    public class GasolineCarServiceImpl : IGasolineCarService
    {
        private readonly IGasolineCarRepository _repository;

        public GasolineCarServiceImpl(IGasolineCarRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<GasolineCar> GetAll()
        {
            return _repository.GetAll();
        }

        public GasolineCar? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(GasolineCar car)
        {
            _repository.Add(car);
        }

        public void Update(GasolineCar car)
        {
            _repository.Update(car);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}