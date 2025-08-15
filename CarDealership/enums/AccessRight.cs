namespace CarDealership.enums
{
    public enum AccessRight
    {
        Guest,
        Authorized,
        Operator,
        Admin
    }

    public static class AccessRightExtensions
    {
        public static string ToFriendlyString(this AccessRight accessRight)
        {
            return accessRight switch
            {
                AccessRight.Guest => "Гість",
                AccessRight.Authorized => "Авторизований",
                AccessRight.Operator => "Оператор",
                AccessRight.Admin => "Адмін",
                _ => accessRight.ToString()
            };
        }
    }
}