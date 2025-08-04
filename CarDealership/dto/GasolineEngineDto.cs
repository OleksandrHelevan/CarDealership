using System.Text;
using CarDealership.enums;

namespace CarDealership.dto;

public class GasolineEngineDto : Engine
{
    public FuelType FuelType { get; set; }
    public float FuelConsumption { get; set; }
    
    public string FuelTypeString { get; set; }

    public GasolineEngineDto() : base(0)
    {
    }
    public GasolineEngineDto(double power, FuelType fuelType, float fuelConsumption) : base(power)
    {
        FuelType = fuelType;
        FuelConsumption = fuelConsumption;
        FuelTypeString = fuelType.ToFriendlyString();
    }
    
}