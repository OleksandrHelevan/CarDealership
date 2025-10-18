namespace CarDealership.enums;

public enum CarType
{
    Electro,
    Gasoline

}

public static class CarTypeExtensions
{
    public static string ToFriendlyString(this CarType carType)
    {
        return carType switch
        {
            CarType.Electro => "Електричний",
            CarType.Gasoline => "Бензиновий",
        };
    }
}