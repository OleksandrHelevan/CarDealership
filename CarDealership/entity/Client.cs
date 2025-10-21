using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealership.entity;

[Table("clients")]
public class Client
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(PassportData))]
    [Column("passport_data_id")]
    public int PassportDataId { get; set; }

    public PassportData PassportData { get; set; } = null!;
}