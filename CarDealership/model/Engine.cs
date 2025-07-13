using CarDealership.model.interfaces;

namespace CarDealership.model;

public abstract class Engine : IPrintable
{
    private double Power { get; set; }
    private const double WattsPerHorsepower = 745.7;

    protected Engine(double power)
    {
        Power = power;
    }

    protected short CalculateToHorsePower()
    {
        return (short)Math.Round(Power / WattsPerHorsepower);
    }

    public abstract string Print();
}