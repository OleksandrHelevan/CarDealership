using CarDealership.enums;

namespace CarDealership.dto
{
    public static class FilterHelper
    {
        public static TransmissionType? GetTransmissionType(string uiValue)
        {
            return uiValue switch
            {
                "Механіка" => TransmissionType.Manual,
                "Автомат" => TransmissionType.Automatic,
                "CVT" => TransmissionType.CVT,
                "Напівавтомат" => TransmissionType.SemiAutomatic,
                _ => null
            };
        }

        public static CarBodyType? GetBodyType(string uiValue)
        {
            return uiValue switch
            {
                "Мікро" => CarBodyType.Micro,
                "Хетчбек" => CarBodyType.Hatchback,
                "Седан" => CarBodyType.Sedan,
                "Кросовер" => CarBodyType.Crossover,
                "Купе" => CarBodyType.Coupe,
                "Купе SUV" => CarBodyType.CoupeSuv,
                "Гіперкар" => CarBodyType.Hyper,
                "Позашляховик" => CarBodyType.Suv,
                "Підвищеної прохідності" => CarBodyType.OffRoader,
                "Пікап" => CarBodyType.PickUp,
                "Кабріолет" => CarBodyType.Cabriolet,
                "Спортивний" => CarBodyType.Sport,
                "Універсальний MUV" => CarBodyType.Muv,
                "Універсал" => CarBodyType.Wagon,
                "Родстер" => CarBodyType.Roadster,
                "Лімузин" => CarBodyType.Limousine,
                "Формула-1" => CarBodyType.Formula1,
                "Маскл-кар" => CarBodyType.Muscle,
                "Фургон" => CarBodyType.Van,
                _ => null
            };
        }

        public static Color? GetColor(string uiValue)
        {
            return uiValue switch
            {
                "Червоний" => Color.Red,
                "Синій" => Color.Blue,
                "Зелений" => Color.Green,
                "Жовтий" => Color.Yellow,
                "Білий" => Color.White,
                "Чорний" => Color.Black,
                "Оранжевий" => Color.Orange,
                "Фіолетовий" => Color.Purple,
                "Коричневий" => Color.Brown,
                "Рожевий" => Color.Pink,
                "Сірий" => Color.Gray,
                "Блакитний" => Color.LightBlue,
                _ => null
            };
        }

        public static DriveType? GetDriveType(string uiValue)
        {
            return uiValue switch
            {
                "Передній привід" => DriveType.FWD,
                "Задній привід" => DriveType.RWD,
                "Повний привід (AWD)" => DriveType.AWD,
                "Повний привід (4WD)" => DriveType.FourWD,
                _ => null
            };
        }

        public static FuelType? GetFuelType(string uiValue)
        {
            return uiValue switch
            {
                "Бензин" => FuelType.Petrol,
                "Дизель" => FuelType.Diesel,
                "Газ" => FuelType.Gas,
                _ => null
            };
        }
    }
}
