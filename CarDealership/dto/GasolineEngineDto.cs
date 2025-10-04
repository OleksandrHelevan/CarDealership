using System.Text;
using CarDealership.enums;

namespace CarDealership.dto;

public class GasolineEngineDto : Engine
{
    public int Id { get; set; }
    public FuelType FuelType { get; set; }
    public float FuelConsumption { get; set; }
    
    public string FuelTypeString { get; set; }

    public GasolineEngineDto() : base(0)
    {
    }
    public GasolineEngineDto(int id, double power, FuelType fuelType, float fuelConsumption) : base(power)
    {
        Id = id;
        FuelType = fuelType;
        FuelConsumption = fuelConsumption;
        FuelTypeString = fuelType.ToFriendlyString();
    }
    
}