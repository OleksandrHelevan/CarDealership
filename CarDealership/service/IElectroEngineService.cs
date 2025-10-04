using CarDealership.entity;
using CarDealership.dto;

namespace CarDealership.service;

public interface IElectroEngineService
{
    List<ElectroEngineDto> GetAllElectroEngines();
    public void AddElectroEngine(ElectroEngine dto);

    void UpdateElectroEngine(ElectroEngineDto engine);
    void DeleteElectroEngine(int id);
}