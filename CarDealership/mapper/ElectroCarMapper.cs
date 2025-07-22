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
            e.Weight, e.DriveType, e.Transmission);
    }
}