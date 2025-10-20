namespace CarDealership.enums;

public enum RequestStatus
{
    Pending,
    Approved,
    Rejected
}

public static class RequestStatusExtensions
{
    public static string ToFriendlyString(this RequestStatus status)
    {
        return status switch
        {
            RequestStatus.Pending => "В обробці",
            RequestStatus.Approved => "Схвалено",
            RequestStatus.Rejected => "Відхилено",
        };
    }
}