using System.Collections.Generic;
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
    public string Login { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    [Column("email")]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("password")]
    public string Password { get; set; } = null!;

    [Required]
    [Column("access_right")]
    public AccessRight AccessRight { get; set; }

    public Client? Client { get; set; }

    public ICollection<AuthorizationRequest>? Requests { get; set; }

    [NotMapped]
    public string AccessRightString => AccessRight.ToFriendlyString();
}
