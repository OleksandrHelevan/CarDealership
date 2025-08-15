namespace CarDealership.enums
{
    public enum ElectroMotorType
    {
        Asynchronous,
        DirectCurrent,
        Synchronous,
        PermanentMagnet,
        Induction
    }

    public static class ElectroMotorTypeExtensions
    {
        public static string ToFriendlyString(this ElectroMotorType motorType)
        {
            return motorType switch
            {
                ElectroMotorType.Asynchronous => "Асинхронний",
                ElectroMotorType.DirectCurrent => "Постійного струму",
                ElectroMotorType.Synchronous => "Синхронний",
                ElectroMotorType.PermanentMagnet => "Постійний магніт",
                ElectroMotorType.Induction => "Індукційний",
                _ => motorType.ToString()
            };
        }
    }
}