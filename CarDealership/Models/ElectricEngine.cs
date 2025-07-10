namespace CarDealership.Models;

public class ElectricEngine : Engine
{
    public ElectricEngine(double power) : base(power)
    {
    }

    public override string Print()
    {
        throw new NotImplementedException();
    }
}