using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity;

[Table("gasoline_cars")]
public class GasolineCar
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
    [Column("engine_id")]
    public GasolineEngine Engine { get; set; }

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
}