using CarDealership.dto;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.mapper;

public static class ProductMapper
{
    public static ProductDto ToDto(Product entity)
    {
        if (entity.Car == null)
            throw new InvalidOperationException("Product.Car is null");

        Vehicle vehicle = entity.Car switch
        {
            ElectroCar electro => ElectroCarMapper.ToDto(electro),
            GasolineCar gasoline => GasolineCarMapper.ToDto(gasoline),
            _ => throw new InvalidOperationException("Unknown car type")
        };

        return new ProductDto(
            entity.Id,
            entity.ProductNumber,
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
            ProductNumber = dto.Number,
            CountryOfOrigin = dto.CountryOfOrigin,
            InStock = dto.InStock,
            AvailableFrom = dto.AvailableFrom
        };

        switch (dto.Vehicle)
        {
            case ElectroCarDto electroDto:
                entity.CarId = electroDto.Id;
                entity.CarType = CarType.Electro;
                break;
            case GasolineCarDto gasolineDto:
                entity.CarId = gasolineDto.Id;
                entity.CarType = CarType.Gasoline;
                break;
            default:
                throw new InvalidOperationException("Unknown vehicle DTO type");
        }

        return entity;
    }
}