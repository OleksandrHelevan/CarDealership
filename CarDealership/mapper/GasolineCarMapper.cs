using CarDealership.entity;
using CarDealership.dto;
using System;

namespace CarDealership.mapper
{
    public class GasolineCarMapper
    {
        public static GasolineCarDto ToDto(GasolineCar e)
        {
            var dto = new GasolineCarDto(
                e.Brand,
                e.ModelName,
                GasolineEngineMapper.ToDto(e.Engine),
                e.Color,
                e.Mileage,
                e.Price,
                e.Weight,
                e.DriveType,
                e.Transmission,
                e.Year,
                e.NumberOfDoors,
                e.BodyType
            );
            dto.Id = e.Id;
            return dto;
        }


        public static GasolineCar ToEntity(GasolineCarDto dto)
        {
            if (dto.Engine is not GasolineEngineDto gasolineEngineDto)
                throw new ArgumentException("Expected GasolineEngineDto for GasolineCar");

            return new GasolineCar
            {
                Id = dto.Id,
                Brand = dto.Brand,
                ModelName = dto.ModelName,
                Engine = GasolineEngineMapper.ToEntity(gasolineEngineDto),
                Color = dto.Color,
                Mileage = dto.Mileage,
                Price = dto.Price,
                Weight = dto.Weight,
                DriveType = dto.DriveType,
                Transmission = dto.Transmission,
                Year = dto.Year,
                NumberOfDoors = dto.NumberOfDoors,
                BodyType = dto.BodyType
            };
        }
    }
}