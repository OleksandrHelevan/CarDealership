using CarDealership.entity;
using CarDealership.model;

namespace CarDealership.mapper;

public class GasolineEngineMapper
{
    public static GasolineEngineDto ToDto(GasolineEngineEntity entity)
    {
        return new GasolineEngineDto(entity.Power, entity.FuelType, entity.FuelConsumption);
    }
}