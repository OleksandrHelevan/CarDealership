using CarDealership.dto;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.mapper;

public static class ProductMapper
{
    public static ProductDto ToDto(Product entity)
    {
        Vehicle vehicle = null!;

        if (entity.CarType == CarType.Electro && entity.ElectroCar != null)
        {
            vehicle = ElectroCarMapper.ToDto(entity.ElectroCar);
        }
        else if (entity.CarType == CarType.Gasoline && entity.GasolineCar != null)
        {
            vehicle = GasolineCarMapper.ToDto(entity.GasolineCar);
        }
        else
        {
            throw new InvalidOperationException("Unknown car type or vehicle is null");
        }

        return new ProductDto(
            entity.Number,
            entity.CountryOfOrigin,
            entity.InStock,
            entity.AvailableFrom,
            entity.CarType,
            vehicle
        );
    }

    public static Product ToEntity(ProductDto dto)
    {
        var entity = new Product
        {
            Number = dto.Number,
            CountryOfOrigin = dto.CountryOfOrigin,
            InStock = dto.InStock,
            AvailableFrom = dto.AvailableFrom,
            CarType = dto.CarType
        };

        if (dto.CarType == CarType.Electro && dto.Vehicle is ElectroCarDto electroDto)
        {
            entity.ElectroCar = ElectroCarMapper.ToEntity(electroDto);
        }
        else if (dto.CarType == CarType.Gasoline && dto.Vehicle is GasolineCarDto gasolineDto)
        {
            entity.GasolineCar = GasolineCarMapper.ToEntity(gasolineDto);
        }
        else
        {
            throw new InvalidOperationException("Unknown car type or vehicle DTO type mismatch");
        }

        return entity;
    }
}