using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity
{
    [Table("electro_engines")]
    public class ElectroEngine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("power", TypeName = "decimal(8,2)")]
        public decimal Power { get; set; }

        [Required]
        [Column("battery_capacity", TypeName = "decimal(8,2)")]
        public decimal BatteryCapacity { get; set; }

        [Required]
        [Column("range")]
        public int Range { get; set; }

        [Required]
        [Column("motor_type")]
        public ElectroMotorType MotorType { get; set; }

        [Column("charging_time", TypeName = "decimal(5,2)")]
        public decimal? ChargingTime { get; set; }

        [Column("max_charging_power", TypeName = "decimal(8,2)")]
        public decimal? MaxChargingPower { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<ElectroCar> ElectroCars { get; set; } = new List<ElectroCar>();

        // Computed properties
        [NotMapped]
        public string EngineString 
        {
            get
            {
                return $"Тип двигуна: Електричний\n" +
                    $"Батарея: {BatteryCapacity:F1} кВт·г\n" +
                    $"Потужність: {Power:F1} кВт\n" +
                    $"Мотор: {MotorType.ToFriendlyString()}\n" +
                    $"Запас ходу: {Range} км" +
                    (ChargingTime.HasValue ? $"\nЧас зарядки: {ChargingTime:F1} год" : "") +
                    (MaxChargingPower.HasValue ? $"\nМакс. потужність зарядки: {MaxChargingPower:F1} кВт" : "");
            }
        }
    }
}
