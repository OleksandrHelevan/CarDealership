using CarDealership.config;
using CarDealership.mapper;
using CarDealership.dto;
using CarDealership.repo.impl;

namespace CarDealership.service.impl;

public class GasolineEngineServiceImpl : IGasolineEngineService
{
    private readonly GasolineEngineRepository _repository = new GasolineEngineRepository(new DealershipContext());

    public List<GasolineEngineDto> GetAllGasolineEngines()
    {
        var entities = _repository.GetAll();
        var dtos = entities.Select(GasolineEngineMapper.ToDto).ToList();
      
        return dtos;
    }
}
