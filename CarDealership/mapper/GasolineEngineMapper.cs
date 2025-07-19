using CarDealership.entity;
using CarDealership.model;

namespace CarDealership.mapper;

public class GasolineEngineMapper
{
    public static GasolineEngineDto ToDto(GasolineEngine entity)
    {
        return new GasolineEngineDto(entity.Power, entity.FuelType, entity.FuelConsumption);
    }
    
}