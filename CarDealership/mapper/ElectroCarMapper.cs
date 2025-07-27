using CarDealership.entity;
using CarDealership.model;

namespace CarDealership.mapper;

public class ElectroCarMapper
{
    public static CarDto ToDto(ElectroCar e)
    {
        return new CarDto(e.Brand, e.ModelName,
            ElectroEngineMapper.ToDto(e.Engine),
            e.Color, e.Mileage, e.Price,
            e.Weight, e.DriveType, e.Transmission,
            e.Year, e.NumberOfDoors, e.BodyType);
    }

    public static ElectroCar ToEntity(CarDto dto)
    {
        if (dto.Engine is not ElectroEngineDto electroEngineDto)
            throw new ArgumentException("Expected ElectroEngineDto for ElectroCar");

        return new ElectroCar
        {
            Brand = dto.Brand,
            ModelName = dto.ModelName,
            Engine = ElectroEngineMapper.ToEntity(electroEngineDto),
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