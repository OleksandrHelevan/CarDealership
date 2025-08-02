
using CarDealership.dto.interfaces;
using CarDealership.enums;

namespace CarDealership.dto;

public abstract class Vehicle : IPrintable
{
    public string Brand { get; set; }
    
    public int Weight { get; set; }
    public string ModelName { get; set; }
    public Engine Engine { get; set; }
    public Color Color { get; set; }
    public int Mileage { get; set; }
    public double Price { get; set; }
    
    public int Year { get; set; }
    
    public int NumberOfDoors { get; set; }
    
    public CarBodyType BodyType { get; set; }


    public Vehicle()
    {
    }
    public Vehicle(string brand, string modelName, Engine engine, Color color, int mileage, double price, int weight, int year, int numberOfDoors, CarBodyType bodyType)
    {
        Brand = brand;
        ModelName = modelName;
        Engine = engine;
        Color = color;
        Mileage = mileage;
        Price = price;
        Weight = weight;
        Year = year;
        NumberOfDoors = numberOfDoors;
        BodyType = bodyType;
    }
    
    public abstract string Print();
}