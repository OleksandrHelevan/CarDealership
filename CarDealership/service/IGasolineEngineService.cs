using CarDealership.model;

namespace CarDealership.service;

public interface IGasolineEngineService
{
    List<GasolineEngineDto> GetAllGasolineEngines();
}