using CarDealership.dto;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.mapper;

public static class ProductMapper
{
    public static ProductDto ToDto(Product entity)
    {
        if (entity.Car == null)
            throw new InvalidOperationException("Product has no Car loaded");

        if (entity.Car.Engine == null)
            throw new InvalidOperationException("Car has no Engine loaded");

        var engineDto = new EngineDto
        {
            Id = entity.Car.Engine.Id,
            EngineType = entity.Car.Engine.EngineType,
            Power = entity.Car.Engine.Power,
            FuelType = entity.Car.Engine.FuelType,
            FuelConsumption = entity.Car.Engine.FuelConsumption,
            BatteryCapacity = entity.Car.Engine.BatteryCapacity,
            Range = entity.Car.Engine.Range,
            MotorType = entity.Car.Engine.MotorType
        };

        var vehicle = new Vehicle
        {
            Id = entity.Car.Id,
            CarType = entity.Car.CarType,
            Brand = entity.Car.Brand,
            ModelName = entity.Car.ModelName,
            Engine = engineDto,
            Color = entity.Car.Color,
            ColorString = entity.Car.Color.ToFriendlyString(),
            Mileage = entity.Car.Mileage,
            Price = (double)entity.Car.Price,
            Weight = entity.Car.Weight,
            DriveType = entity.Car.DriveType,
            DriveTypeString = entity.Car.DriveType.ToFriendlyString(),
            Transmission = entity.Car.Transmission,
            TransmissionString = entity.Car.Transmission.ToFriendlyString(),
            Year = entity.Car.Year,
            NumberOfDoors = entity.Car.NumberOfDoors,
            BodyType = entity.Car.BodyType,
            BodyTypeString = entity.Car.BodyType.ToFriendlyString()
        };

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
            Id = dto.Id,
            Number = dto.Number,
            CountryOfOrigin = dto.CountryOfOrigin,
            InStock = dto.InStock,
            AvailableFrom = dto.AvailableFrom,
            CarId = dto.Vehicle.Id
        };
        return entity;
    }
}
