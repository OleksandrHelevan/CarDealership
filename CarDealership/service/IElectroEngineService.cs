using CarDealership.entity;
using CarDealership.model;

namespace CarDealership.service;

public interface IElectroEngineService
{
    List<ElectroEngineDto> GetAllElectroEngines();
}