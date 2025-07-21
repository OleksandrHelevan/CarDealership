
using CarDealership.model.interfaces;
using CarDealership.enums;

namespace CarDealership.model;

public abstract class Vehicle : IPrintable
{
    public string Brand { get; set; }
    
    public int Weight { get; set; }
    public string ModelName { get; set; }
    public Engine Engine { get; set; }
    public Color Color { get; set; }
    public int Mileage { get; set; }
    public double Price { get; set; }


    public Vehicle(string brand, string modelName, Engine engine, Color color, int mileage, double price, int weight)
    {
        Brand = brand;
        ModelName = modelName;
        Engine = engine;
        Color = color;
        Mileage = mileage;
        Price = price;
        Weight = weight;
    }
    
    public abstract string Print();
}