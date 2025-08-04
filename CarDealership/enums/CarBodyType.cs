namespace CarDealership.enums;

public enum CarBodyType
{
    Micro,
    Hatchback,
    Sedan,
    Crossover,
    Coupe,
    CoupeSuv,
    Hyper,
    Suv,
    OffRoader,
    PickUp,
    Cabriolet,
    Sport,
    Muv,
    Wagon,
    Roadster,
    Limousine,
    Formula1,
    Muscle,
    Van,
}

public static class CarBodyTypeExtensions
{
    public static string ToFriendlyString(this CarBodyType bodyType)
    {
        return bodyType switch
        {
            CarBodyType.Micro => "Мікро",
            CarBodyType.Hatchback => "Хетчбек",
            CarBodyType.Sedan => "Седан",
            CarBodyType.Crossover => "Кросовер",
            CarBodyType.Coupe => "Купе",
            CarBodyType.CoupeSuv => "Купе SUV",
            CarBodyType.Hyper => "Гіперкар",
            CarBodyType.Suv => "Позашляховик",
            CarBodyType.OffRoader => "Підвищеної прохідності",
            CarBodyType.PickUp => "Пікап",
            CarBodyType.Cabriolet => "Кабріолет",
            CarBodyType.Sport => "Спортивний",
            CarBodyType.Muv => "Універсальний MUV",
            CarBodyType.Wagon => "Універсал",
            CarBodyType.Roadster => "Родстер",
            CarBodyType.Limousine => "Лімузин",
            CarBodyType.Formula1 => "Формула-1",
            CarBodyType.Muscle => "Маскл-кар",
            CarBodyType.Van => "Фургон",
            _ => bodyType.ToString()
        };
    }
}
