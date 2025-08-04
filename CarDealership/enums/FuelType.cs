namespace CarDealership.enums;

public enum FuelType
{
    Petrol,
    Diesel,
    Gas
}
public static class FuelTypeExtensions
{
    public static string ToFriendlyString(this FuelType fuelType)
    {
        return fuelType switch
        {
            FuelType.Petrol => "Бензин",
            FuelType.Diesel => "Дизель",
            FuelType.Gas => "Газ",
            _ => fuelType.ToString()
        };
    }
}
