using CarDealership.enums;

namespace CarDealership.dto;

public class EngineDto
{
    public int Id { get; set; }
    public EngineType EngineType { get; set; }
    public double Power { get; set; }

    public FuelType? FuelType { get; set; }
    public float? FuelConsumption { get; set; }

    public double? BatteryCapacity { get; set; }
    public int? Range { get; set; }
    public ElectroMotorType? MotorType { get; set; }
}
