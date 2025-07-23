using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealership.entity;

[Table("passport_data")]
public class PassportData
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("first_name")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Column("last_name")]
    public string LastName { get; set; } = null!;

    [Required]
    [Column("passport_number")]
    public string PassportNumber { get; set; } = null!;
}