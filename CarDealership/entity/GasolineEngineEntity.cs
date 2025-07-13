using CarDealership.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealership.entity
{
    [Table("gasoline_engines")]
    public class GasolineEngineEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id {get; set; }

        [Required]
        [Column("power")]
        public double Power { get; set; }

        [Required]
        [Column("fuel_type")]
        public FuelType FuelType { get; set; }

        [Required] [Column("fuel_consumption")]
        public float FuelConsumption {get; set; }
        
    }
    
}