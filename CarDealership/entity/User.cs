using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity;

[Table("keys")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("login")]
    public string Login { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("password")]
    public string Password { get; set; }

    [Required] [Column("access_right")] 
    public AccessRight AccessRight { get; set; }
}