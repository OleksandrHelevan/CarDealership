using System.Collections.Generic;
using System.Linq;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.repo;

namespace CarDealership.service.impl
{
    public class CarServiceImpl : ICarService
    {
        private readonly ICarRepository _repository;

        public CarServiceImpl(ICarRepository repository)
        {
            _repository = repository;
        }

        public List<Car> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public Car? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public List<Car> GetByType(CarType type)
        {
            return _repository.GetByType(type).ToList();
        }

        public void Add(Car car)
        {
            _repository.Add(car);
        }

        public void Update(Car car)
        {
            _repository.Update(car);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}

