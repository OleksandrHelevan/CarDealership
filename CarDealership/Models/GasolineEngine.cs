using System.Text;

namespace CarDealership.Models;

public class GasolineEngine : Engine
{
    private FuelType FuelType { get; set; }
    private float FuelConsumption { get; set; }

    public GasolineEngine(double power, FuelType fuelType, float fuelConsumption) : base(power)
    {
        FuelType = fuelType;
        FuelConsumption = fuelConsumption;
    }

    public override string Print()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Gasoline Engine");
        builder.AppendLine($"Fuel Type: {FuelType}");
        builder.AppendLine($"Fuel Consumption: {FuelConsumption}");
        builder.AppendLine($"- Power: {CalculateToHorsePower()} HP");
        return builder.ToString();
    }
}