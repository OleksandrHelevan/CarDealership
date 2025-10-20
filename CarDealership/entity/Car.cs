using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity
{
    [Table("cars")]
    public abstract class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("brand")]
        public string Brand { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Column("model_name")]
        public string ModelName { get; set; } = null!;

        [Required]
        [Column("color")]
        public Color Color { get; set; }

        [Required]
        [Column("mileage")]
        public int Mileage { get; set; }

        [Required]
        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

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

        [Required]
        [Column("car_type")]
        public CarType CarType { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        // Computed properties
        [NotMapped]
        public string BodyTypeString => BodyType.ToFriendlyString();

        [NotMapped]
        public string TransmissionString => Transmission.ToFriendlyString();

        [NotMapped]
        public string DriveTypeString => DriveType.ToFriendlyString();

        [NotMapped]
        public string ColorString => Color.ToFriendlyString();

        [NotMapped]
        public string FullName => $"{Brand} {ModelName}";

        [NotMapped]
        public string CarTypeString => CarType.ToFriendlyString();

        [NotMapped]
        public string EngineString => this switch
        {
            ElectroCar e => e.Engine?.EngineString ?? string.Empty,
            GasolineCar g => g.Engine?.EngineString ?? string.Empty,
            _ => string.Empty
        };
    }
}
