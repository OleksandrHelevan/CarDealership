using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity;

[Table("cars")]
public class Car
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("car_type")]
    public CarType CarType { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("brand")]
    public string Brand { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("model_name")]
    public string ModelName { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Engine))]
    [Column("engine_id")]
    public int EngineId { get; set; }

    public Engine Engine { get; set; } = null!;

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

    [NotMapped]
    public string BodyTypeString => BodyType.ToFriendlyString();

    [NotMapped]
    public string TransmissionString => Transmission.ToFriendlyString();

    [NotMapped]
    public string DriveTypeString => DriveType.ToFriendlyString();

    [NotMapped]
    public string ColorString => Color.ToFriendlyString();

    [NotMapped]
    public string CarTypeString => CarType.ToFriendlyString();
}