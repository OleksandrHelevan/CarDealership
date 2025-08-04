namespace CarDealership.enums;

public enum DriveType
{
    FWD,
    RWD,
    AWD,
    FourWD
}
public static class DriveTypeExtensions
{
    public static string ToFriendlyString(this DriveType driveType)
    {
        return driveType switch
        {
            DriveType.FWD => "Передній привід",
            DriveType.RWD => "Задній привід",
            DriveType.AWD => "Повний привід (AWD)",
            DriveType.FourWD => "Повний привід (4WD)",
            _ => driveType.ToString()
        };
    }
}
