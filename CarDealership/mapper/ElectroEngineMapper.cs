using CarDealership.entity;
using CarDealership.dto;

namespace CarDealership.mapper;

public class ElectroEngineMapper
{
    public static ElectroEngineDto ToDto(ElectroEngine entity)
    {
        return new ElectroEngineDto(
            entity.Id,
            (double)entity.Power,
            (double)entity.BatteryCapacity,
            entity.Range,
            entity.MotorType
        );
    }

    public static ElectroEngine ToEntity(ElectroEngineDto dto)
    {
        return new ElectroEngine
        {
            Id = dto.Id,
            Power = (decimal)dto.Power,  
            BatteryCapacity = (decimal)dto.BatteryCapacity,
            Range = dto.Range,
            MotorType = dto.MotorType
        };
    }
}