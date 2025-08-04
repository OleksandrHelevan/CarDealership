using CarDealership.entity;
using CarDealership.dto;

namespace CarDealership.mapper
{
    public class GasolineEngineMapper
    {
        public static GasolineEngineDto ToDto(GasolineEngine entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "GasolineEngine is null.");
            }

            return new GasolineEngineDto(
                entity.Power,
                entity.FuelType,
                entity.FuelConsumption
            );
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