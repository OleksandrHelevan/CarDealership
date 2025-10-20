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
    [MaxLength(100)]
    [Column("first_name")]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("last_name")]
    public string LastName { get; set; } = null!;

    [MaxLength(100)]
    [Column("middle_name")]
    public string? MiddleName { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("passport_number")]
    public string PassportNumber { get; set; } = null!;

    [MaxLength(100)]
    [Column("issued_by")]
    public string? IssuedBy { get; set; }

    [Column("issued_date")]
    public DateTime? IssuedDate { get; set; }

    [MaxLength(20)]
    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    [MaxLength(255)]
    [Column("email")]
    public string? Email { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    // Computed properties
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}".Trim();

    [NotMapped]
    public string FullNameWithMiddle => $"{FirstName} {MiddleName} {LastName}".Trim();
}