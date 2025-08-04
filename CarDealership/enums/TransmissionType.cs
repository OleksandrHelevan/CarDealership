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
            TransmissionType.Manual => "Механічна",
            TransmissionType.Automatic => "Автоматична",
            TransmissionType.CVT => "Варіатор (CVT)",
            TransmissionType.SemiAutomatic => "Напівавтоматична",
            _ => transmission.ToString()
        };
    }
}
