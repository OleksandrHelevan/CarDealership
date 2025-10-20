using CarDealership.entity;
using CarDealership.dto;

namespace CarDealership.mapper;

public class ElectroCarMapper
{
    public static ElectroCarDto ToDto(ElectroCar e)
    {
        var engineDto = e.Engine != null ? ElectroEngineMapper.ToDto(e.Engine) : null;
    
        var dto = new ElectroCarDto(
            e.Id,
            e.Brand, e.ModelName,
            engineDto,
            e.Color, e.Mileage, (double)e.Price,
            e.Weight, e.DriveType, e.Transmission,
            e.Year, e.NumberOfDoors, e.BodyType);
        dto.Id = e.Id;
        return dto;
    }


    public static ElectroCar ToEntity(ElectroCarDto dto)
    {
        if (dto.Engine is not ElectroEngineDto electroEngineDto)
            throw new ArgumentException("Expected ElectroEngineDto for ElectroCar");

        return new ElectroCar
        {
            Id = dto.Id,
            Brand = dto.Brand,
            ModelName = dto.ModelName,
            Engine = ElectroEngineMapper.ToEntity(electroEngineDto),
            Color = dto.Color,
            Mileage = dto.Mileage,
            Price = (decimal)dto.Price,
            Weight = dto.Weight,
            DriveType = dto.DriveType,
            Transmission = dto.Transmission,
            Year = dto.Year,
            NumberOfDoors = dto.NumberOfDoors,
            BodyType = dto.BodyType,
            CarType = CarDealership.enums.CarType.Electro
        };
    }

}