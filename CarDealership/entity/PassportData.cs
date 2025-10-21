using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealership.entity;

[Table("passport_data")]
public class PassportData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("first_name")]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [Column("last_name")]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [Required]
    [Column("passport_number")]
    [MaxLength(50)]
    public string PassportNumber { get; set; } = null!;
}