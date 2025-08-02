using CarDealership.dto;

namespace CarDealership.service;

public interface IGasolineEngineService
{
    List<GasolineEngineDto> GetAllGasolineEngines();
}