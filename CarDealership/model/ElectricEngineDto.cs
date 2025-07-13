namespace CarDealership.model;

public class ElectricEngineDto : Engine
{
    public ElectricEngineDto(double power) : base(power)
    {
    }

    public override string Print()
    {
        throw new NotImplementedException();
    }
}