using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity
{
    [Table("ElectroEngines")]
    public class ElectroEngine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("power")]
        public double Power { get; set; }

        [Required]
        [Column("battery_capacity")]
        public double BatteryCapacity { get; set; }

        [Required]
        [Column("range")]
        
        public int Range { get; set; }

        [Required]
        [Column("motor_type")]
        public ElectroMotorType MotorType { get; set; }
    }
}
