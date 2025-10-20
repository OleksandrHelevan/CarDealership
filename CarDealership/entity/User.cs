using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity;

[Table("users")]
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
    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Required]
    [Column("access_right")]
    public AccessRight AccessRight { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("last_login_at")]
    public DateTime? LastLoginAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual Client? Client { get; set; }

    // Computed properties
    [NotMapped]
    public string AccessRightString => AccessRight.ToFriendlyString();

    [NotMapped]
    public bool IsAdmin => AccessRight == AccessRight.Admin;

    [NotMapped]
    public bool IsOperator => AccessRight == AccessRight.Operator;
}