namespace CarDealership.enums
{
    public enum Color
    {
        Red,
        Blue,
        Green,
        Yellow,
        White,
        Black,
        Orange,
        Purple,
        Brown,
        Pink,
        Grey,
        LightBlue
    }

    public static class ColorExtensions
    {
        public static string ToFriendlyString(this Color color)
        {
            return color switch
            {
                Color.Red => "Червоний",
                Color.Blue => "Синій",
                Color.Green => "Зелений",
                Color.Yellow => "Жовтий",
                Color.White => "Білий",
                Color.Black => "Чорний",
                Color.Orange => "Оранжевий",
                Color.Purple => "Фіолетовий",
                Color.Brown => "Коричневий",
                Color.Pink => "Рожевий",
                Color.Grey => "Сірий",
                Color.LightBlue => "Блакитний",
                _ => color.ToString()
            };
        }
    }
}