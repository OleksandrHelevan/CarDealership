namespace CarDealership.dto;

public abstract class Engine
{
    public double Power { get; set; }
    private const double WattsPerHorsepower = 745.7;

    public short HorsePower { get; set; }

    protected Engine(double power)
    {
        Power = power;
        HorsePower = CalculateToHorsePower();
        Console.WriteLine(HorsePower);
    }

    protected short CalculateToHorsePower()
    {
        return (short)Math.Round(Power * 1000 / WattsPerHorsepower);
    }
}