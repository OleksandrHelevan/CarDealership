using CarDealership.dto;
using CarDealership.entity;
using CarDealership.mapper;
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

        public IEnumerable<ElectroCarDto> GetAllCars()
        {
            return _repository.GetAll().Select(ElectroCarMapper.ToDto).ToList();
        }

        public IEnumerable<ElectroCarDto> GetFilteredCars(ElectroCarFilterDto filter)
        {
            return _repository.GetFiltered(filter).Select(ElectroCarMapper.ToDto).ToList();
        }

        public ElectroCar? GetCarById(int id)
        {
            return _repository.GetById(id);
        }

        public void AddCar(ElectroCarDto electroCar)
        {
            _repository.Add(ElectroCarMapper.ToEntity(electroCar));
        }

        public void UpdateCar(ElectroCarDto electroCar)
        {
            _repository.Update(ElectroCarMapper.ToEntity(electroCar));
        }

        public void DeleteCar(int id)
        {
            _repository.Delete(id);
        }
    }
}