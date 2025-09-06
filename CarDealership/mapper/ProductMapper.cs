using CarDealership.dto;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.mapper;

public static class ProductMapper
{
    public static ProductDto ToDto(Product entity)
    {
        Vehicle vehicle = null!;

        if (entity.ElectroCarId.HasValue && entity.ElectroCar != null)
        {
            vehicle = ElectroCarMapper.ToDto(entity.ElectroCar);
        }
        else if (entity.GasolineCarId.HasValue && entity.GasolineCar != null)
        {
            vehicle = GasolineCarMapper.ToDto(entity.GasolineCar);
        }
        else
        {
            throw new InvalidOperationException("No valid car reference found");
        }

        return new ProductDto(
            entity.Id,
            entity.Number,
            entity.CountryOfOrigin,
            entity.InStock,
            entity.AvailableFrom,
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
            AvailableFrom = dto.AvailableFrom
        };

        // Set foreign key IDs based on vehicle type
        if (dto.Vehicle is ElectroCarDto electroDto)
        {
            entity.ElectroCarId = electroDto.Id;
        }
        else if (dto.Vehicle is GasolineCarDto gasolineDto)
        {
            entity.GasolineCarId = gasolineDto.Id;
        }
        else
        {
            throw new InvalidOperationException("Unknown vehicle DTO type");
        }

        return entity;
    }
}