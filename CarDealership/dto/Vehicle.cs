using CarDealership.enums;

namespace CarDealership.dto;

public class Vehicle
{
    public int Id { get; set; }
    public CarType CarType { get; set; }
    public string Brand { get; set; }
    public string ModelName { get; set; }

    public EngineDto Engine { get; set; }

    public Color Color { get; set; }
    public string ColorString { get; set; }

    public int Mileage { get; set; }
    public double Price { get; set; }
    public int Weight { get; set; }
    public int Year { get; set; }
    public int NumberOfDoors { get; set; }
    public CarBodyType BodyType { get; set; }
    public string BodyTypeString { get; set; }

    public DriveType DriveType { get; set; }
    public string DriveTypeString { get; set; }

    public TransmissionType Transmission { get; set; }
    public string TransmissionString { get; set; }

    public string EngineString
    {
        get
        {
            if (Engine == null)
                return string.Empty;

            return Engine.EngineType == EngineType.Electro
                ? $"Тип двигуна: Електричний\n" +
                  $"Батарея: {Engine.BatteryCapacity:F1} кВт·г\n" +
                  $"Потужність: {Engine.Power:F1} кВт\n" +
                  $"Мотор: {Engine.MotorType?.ToFriendlyString()}\n" +
                  $"Запас ходу: {Engine.Range} км"
                : $"Тип двигуна: Бензиновий\n" +
                  $"Паливо: {Engine.FuelType?.ToFriendlyString()}\n" +
                  $"Витрата: {Engine.FuelConsumption:F1} л/100км\n" +
                  $"Потужність: {Engine.Power:F1} кВт";
        }
    }
}
