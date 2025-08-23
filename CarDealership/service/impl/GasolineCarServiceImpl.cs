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

        public IEnumerable<GasolineCarDto> GetAll()
        {
            return _repository.GetAll().Select(GasolineCarMapper.ToDto).ToList();
        }

        public IEnumerable<GasolineCarDto> GetFiltered(GasolineCarFilterDto filter)
        {
            return _repository.GetFiltered(filter).Select(GasolineCarMapper.ToDto).ToList();
        }

        public GasolineCar? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(GasolineCarDto electroCar)
        {
            _repository.Add(GasolineCarMapper.ToEntity(electroCar));
        }

        public void Update(GasolineCarDto electroCar)
        {
            _repository.Update(GasolineCarMapper.ToEntity(electroCar));
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}