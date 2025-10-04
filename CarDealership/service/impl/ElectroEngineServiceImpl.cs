using CarDealership.config;
using CarDealership.mapper;
using CarDealership.dto;
using CarDealership.entity;
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

    public void AddElectroEngine(ElectroEngine engine)
    {
        _repository.Add(engine);
    }


    public void UpdateElectroEngine(ElectroEngineDto engine)
    {
        _repository.Update(ElectroEngineMapper.ToEntity(engine));
    }
    public void DeleteElectroEngine(int id)
    {
        _repository.Delete(id);
    }
}