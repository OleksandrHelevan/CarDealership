namespace CarDealership.enums;

public enum PaymentType
{
    Cash, Credit, Card
}

public static class PaymentTypeExtensions
{
    public static string ToFriendlyString(this PaymentType type)
    {
        return type switch
        {
            PaymentType.Cash => "Готівка",
            PaymentType.Credit => "Кредит",
            PaymentType.Card => "Картка",
        };
    }
}