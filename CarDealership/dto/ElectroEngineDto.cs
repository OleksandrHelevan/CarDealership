using CarDealership.enums;

namespace CarDealership.dto;

public class ElectroEngineDto : Engine
{
    public int Id { get; set; }
    public double BatteryCapacity { get; set; }
    public int Range { get; set; }
    public ElectroMotorType MotorType { get; set; }

    public String ElectroMotorTypeString { get; set; }

    public ElectroEngineDto(
        int id,
        double power, double batteryCapacity, int range, ElectroMotorType motorType) : base(power)
    {
        BatteryCapacity = batteryCapacity;
        Range = range;
        MotorType = motorType;
        ElectroMotorTypeString = MotorType.ToFriendlyString();
    }
}