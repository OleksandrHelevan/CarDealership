using CarDealership.entity;
using CarDealership.dto;

namespace CarDealership.service;

public interface IElectroEngineService
{
    List<ElectroEngineDto> GetAllElectroEngines();
}