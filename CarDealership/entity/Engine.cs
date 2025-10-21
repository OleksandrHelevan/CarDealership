using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity;

[Table("engines")]
public class Engine
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("engine_type")]
    public EngineType EngineType { get; set; }

    [Required]
    [Column("power")]
    public double Power { get; set; }

    [Column("fuel_type")]
    public FuelType? FuelType { get; set; }

    [Column("fuel_consumption")]
    public float? FuelConsumption { get; set; }

    [Column("battery_capacity")]
    public double? BatteryCapacity { get; set; }

    [Column("range")]
    public int? Range { get; set; }

    [Column("motor_type")]
    public ElectroMotorType? MotorType { get; set; }

    [NotMapped]
    public string EngineString =>
        EngineType == EngineType.Electro
            ? $"Тип двигуна: Електричний\nБатарея: {BatteryCapacity:F1} кВт·г\nПотужність: {Power:F1} кВт\nМотор: {MotorType?.ToFriendlyString()}\nЗапас ходу: {Range} км"
            : $"Тип двигуна: Бензиновий\nПотужність: {Power:F1} к.с.\nПальне: {FuelType?.ToFriendlyString()}\nВитрата: {FuelConsumption:F1} л/100км";
}