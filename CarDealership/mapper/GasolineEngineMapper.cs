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
                entity.Id,
                (double)entity.Power,
                entity.FuelType,
                (float)entity.FuelConsumption
            );
        }
        
        public static GasolineEngine ToEntity(GasolineEngineDto dto)
        {
            return new GasolineEngine
            {
                Id = dto.Id,
                Power = (decimal)dto.Power,
                FuelType = dto.FuelType,
                FuelConsumption = (decimal)dto.FuelConsumption
            };
        }
    }
}