using CarDealership.enums;

namespace CarDealership.dto;

public class ElectroEngineDto : Engine
{
    public double BatteryCapacity { get; set; }
    public int Range { get; set; }
    public ElectroMotorType MotorType { get; set; }
    public ElectroEngineDto(double power, double batteryCapacity, int range, ElectroMotorType motorType) : base(power)
    {
        BatteryCapacity = batteryCapacity;
        Range = range;
        MotorType = motorType;
    }
    
}