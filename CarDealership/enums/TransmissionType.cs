namespace CarDealership.enums;

public enum TransmissionType
{
    Manual,
    Automatic,
    CVT,
    SemiAutomatic
}

public static class TransmissionTypeExtensions
{
    public static string ToFriendlyString(this TransmissionType transmission)
    {
        return transmission switch
        {
            TransmissionType.Manual => "Механіка",
            TransmissionType.Automatic => "Автомат",
            TransmissionType.CVT => "CVT",
            TransmissionType.SemiAutomatic => "Напівавтомат",
            _ => transmission.ToString()
        };
    }
}