using CarDealership.entity;
using CarDealership.model;

namespace CarDealership.mapper
{
    public class GasolineEngineMapper
    {
        public static GasolineEngineDto ToDto(GasolineEngine entity)
        {
            return new GasolineEngineDto(entity.Power, entity.FuelType, entity.FuelConsumption);
        }
        
        public static GasolineEngine ToEntity(GasolineEngineDto dto)
        {
            return new GasolineEngine
            {
                Power = dto.Power,
                FuelType = dto.FuelType,
                FuelConsumption = dto.FuelConsumption
            };
        }
    }
}