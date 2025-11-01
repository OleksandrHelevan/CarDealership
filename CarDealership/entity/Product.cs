using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealership.entity;

[Table("products")]
public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("number")]
    public string Number { get; set; } = null!;

    [Required]
    [Column("country_of_origin")]
    public string CountryOfOrigin { get; set; } = null!;

    [Required]
    [Column("in_stock")]
    public bool InStock { get; set; }

    [Required]
    [Column("amount")]
    public int Amount { get; set; }

    [Column("available_from")]
    public DateTime? AvailableFrom { get; set; }

    [Required]
    [ForeignKey(nameof(Car))]
    [Column("car_id")]
    public int CarId { get; set; }

    public Car Car { get; set; } = null!;
}
