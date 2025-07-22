using CarDealership.entity;
using CarDealership.repo;

namespace CarDealership.service.impl
{
    public class ElectroCarServiceImpl : IElectroCarService
    {
        private readonly IElectroCarRepository _repository;

        public ElectroCarServiceImpl(IElectroCarRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ElectroCar> GetAllCars()
        {
            return _repository.GetAll();
        }

        public ElectroCar? GetCarById(int id)
        {
            return _repository.GetById(id);
        }

        public void AddCar(ElectroCar car)
        {
            _repository.Add(car);
        }

        public void UpdateCar(ElectroCar car)
        {
            _repository.Update(car);
        }

        public void DeleteCar(int id)
        {
            _repository.Delete(id);
        }
    }
}