using CarDealership.enums;
using CarDealership.model.interfaces;

namespace CarDealership.model;

public class ElectroEngineDto : Engine
{
    public double BatteryCapacity { get; set; }
    public int Range { get; set; }
    public ElectroMotorType MotorType { get; set; }
    
    public ElectroEngineDto(double power, double batteryCapacity, int range, ElectroMotorType motorType) : base(power)
    {
        BatteryCapacity = batteryCapacity;
        Range = range;
        MotorType = motorType;
    }

    public override string Print()
    {
        return $"Електродвигун:\n" +
               $"- Потужність: {Power} кВт\n" +
               $"- Ємність батареї: {BatteryCapacity} кВт·год\n" +
               $"- Запас ходу: {Range} км\n" +
               $"- Тип мотора: {MotorType}";
    }

}