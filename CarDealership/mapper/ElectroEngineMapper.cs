using CarDealership.entity;
using CarDealership.model;

namespace CarDealership.mapper;

public class ElectroEngineMapper
{
    public static ElectroEngineDto ToDto(ElectroEngine entity)
    {
        return new ElectroEngineDto(entity.Power, entity.BatteryCapacity, entity.Range, entity.MotorType);
    }
}