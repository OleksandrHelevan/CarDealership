namespace CarDealership.Models;

public abstract class Vehicle
{
    private string Brand { get; set; }
    private string ModelName { get; set; }
    private Engine Engine { get; set; }
    private Color Color { get; set; }
    private int Mileage { get; set; }
    private double Price { get; set; }


    public Vehicle(string brand, string modelName, Engine engine, Color color, int mileage, double price)
    {
        Brand = brand;
        ModelName = modelName;
        Engine = engine;
        Color = color;
        Mileage = mileage;
        Price = price;
    }
}