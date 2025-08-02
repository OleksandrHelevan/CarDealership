using System.Collections.Immutable;
using CarDealership.entity;
using CarDealership.repo;
using CarDealership.dto;
using CarDealership.mapper;
using NHibernate.Util;

namespace CarDealership.service.impl
{
    public class GasolineCarServiceImpl : IGasolineCarService
    {
        private readonly IGasolineCarRepository _repository;

        public GasolineCarServiceImpl(IGasolineCarRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CarDto> GetAll()
        {
            return _repository.GetAll().Select(GasolineCarMapper.ToDto).ToList();
        }

        public GasolineCar? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(CarDto car)
        {
            _repository.Add(GasolineCarMapper.ToEntity(car));
        }

        public void Update(CarDto car)
        {
            _repository.Update(GasolineCarMapper.ToEntity(car));
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}