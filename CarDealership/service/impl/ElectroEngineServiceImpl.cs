using CarDealership.config;
using CarDealership.entity;
using CarDealership.mapper;
using CarDealership.model;
using CarDealership.repo;
using CarDealership.repo.impl;

namespace CarDealership.service.impl;

public class ElectroEngineServiceImpl : IElectroEngineService
{
    private readonly IElectroEngineRepository _repository = new ElectroEngineRepository(new DealershipContext());
    public List<ElectroEngineDto> GetAllElectroEngines()
    {
        var entities = _repository.GetAll();
        var result = entities.Select(ElectroEngineMapper.ToDto).ToList();
        
        return result;
    }
}