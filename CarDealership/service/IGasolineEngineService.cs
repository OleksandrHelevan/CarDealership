using CarDealership.dto;
using CarDealership.entity;

namespace CarDealership.service;

public interface IGasolineEngineService
{
    List<GasolineEngineDto> GetAllGasolineEngines();
    
    void AddGasolineEngine(GasolineEngine dto);
}