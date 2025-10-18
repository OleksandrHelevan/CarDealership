using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.dto;

public abstract class Vehicle
{
    public int Id { get; set; }
    public string Brand { get; set; }

    public int Weight { get; set; }
    public string ModelName { get; set; }
    public Engine Engine { get; set; }
    public Color Color { get; set; }
    
    public string ColorString { get; set; }
    public int Mileage { get; set; }
    public double Price { get; set; }

    public int Year { get; set; }

    public int NumberOfDoors { get; set; }

    public CarBodyType BodyType { get; set; }

    public string BodyTypeString { get; set; }

    public string VehicleTypeName { get; set; } = string.Empty;
    
    public string EngineString
    {
        get
        {
            if (Engine == null)
                return string.Empty;

            return Engine switch
            {
                GasolineEngineDto g => 
                    $"Тип двигуна: Бензиновий\n" +
                    $"Паливо: {g.FuelTypeString} ({g.FuelType.ToFriendlyString()})\n" +
                    $"Витрата: {g.FuelConsumption:F1} л/100км\n" +
                    $"Потужність: {g.Power:F1} кВт",

                ElectroEngineDto e => 
                    $"Тип двигуна: Електричний\n" +
                    $"Батарея: {e.BatteryCapacity:F1} кВт·г\n" +
                    $"Потужність: {e.Power:F1} кВт\n" +
                    $"Мотор: {e.ElectroMotorTypeString}\n" +
                    $"Запас ходу: {e.Range} км",

                _ => string.Empty
            };
        }
    }

    public Vehicle()
    {
    }

    public Vehicle(string brand, string modelName, Engine engine, Color color, int mileage, double price, int weight,
        int year, int numberOfDoors, CarBodyType bodyType)
    {
        Brand = brand;
        ModelName = modelName;
        Engine = engine;
        Color = color;
        ColorString = color.ToFriendlyString();
        Mileage = mileage;
        Price = price;
        Weight = weight;
        Year = year;
        NumberOfDoors = numberOfDoors;
        BodyType = bodyType;
        BodyTypeString = bodyType.ToFriendlyString();
    }
}