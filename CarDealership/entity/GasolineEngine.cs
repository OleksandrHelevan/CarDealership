using CarDealership.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealership.entity
{
    [Table("gasoline_engines")]
    public class GasolineEngine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("power", TypeName = "decimal(8,2)")]
        public decimal Power { get; set; }

        [Required]
        [Column("fuel_type")]
        public FuelType FuelType { get; set; }

        [Required]
        [Column("fuel_consumption", TypeName = "decimal(5,2)")]
        public decimal FuelConsumption { get; set; }

        [Column("displacement", TypeName = "decimal(4,2)")]
        public decimal? Displacement { get; set; }

        [Column("cylinders")]
        public int? Cylinders { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<GasolineCar> GasolineCars { get; set; } = new List<GasolineCar>();

        // Computed properties
        [NotMapped]
        public string EngineString 
        {
            get
            {
                return $"Тип двигуна: Бензиновий\n" +
                    $"Паливо: {FuelType.ToFriendlyString()}\n" +
                    $"Витрата: {FuelConsumption:F1} л/100км\n" +
                    $"Потужність: {Power:F1} кВт" +
                    (Displacement.HasValue ? $"\nОб'єм: {Displacement:F1} л" : "") +
                    (Cylinders.HasValue ? $"\nЦиліндри: {Cylinders}" : "");
            }
        }
    }
}