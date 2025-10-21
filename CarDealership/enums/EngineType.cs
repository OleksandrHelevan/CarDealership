namespace CarDealership.enums;

public enum EngineType
{
    Electro, 
    Gasoline
}

public static class EngineTypeExtensions
{
    public static string ToFriendlyString(this EngineType engineType)
    {
        return engineType switch
        {
            EngineType.Electro => "Електричний",
            EngineType.Gasoline => "Бензиновий",
            _ => engineType.ToString()
        };
    }
}
