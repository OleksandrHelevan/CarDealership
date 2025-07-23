using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Forms.VisualStyles;
using CarDealership.enums;

namespace CarDealership.entity
{
    [Table("electro_cars")]
    public class ElectroCar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("brand")]
        public string Brand { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("model_name")]
        public string ModelName { get; set; }

        [Required]
        public ElectroEngine Engine { get; set; }

        [Required]
        [Column("color")]
        public Color Color { get; set; }

        [Required]
        [Column("mileage")]
        public int Mileage { get; set; }

        [Required]
        [Column("price")]
        public double Price { get; set; }

        [Required]
        [Column("weight")]
        public int Weight { get; set; }

        [Required]
        [Column("drive_type")]
        public DriveType DriveType { get; set; }

        [Required]
        [Column("transmission")]
        public TransmissionType Transmission { get; set; }
        
        [Required]
        [Column("year")]
        public int Year { get; set; }
        
        [Required]
        [Column("number_of_doors")]
        public int NumberOfDoors { get; set; }
        
        [Required]
        [Column("body_type")]
        public CarBodyType BodyType { get; set; }
        
    }
}