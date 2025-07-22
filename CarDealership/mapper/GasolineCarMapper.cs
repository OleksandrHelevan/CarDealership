using CarDealership.entity;
using CarDealership.model;

namespace CarDealership.mapper;

public class GasolineCarMapper
{
    public static CarDto ToDto(GasolineCar e)
    {
        return new CarDto(e.Brand, e.ModelName,
            GasolineEngineMapper.ToDto(e.Engine),
            e.Color, e.Mileage, e.Price,
            e.Weight, e.DriveType, e.Transmission);
    }
}