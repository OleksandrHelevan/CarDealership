using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity;

[Table("requests")]
public class AuthorizationRequest
{
    [Column("id")]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("login")]
    public string Login { get; set; }

    [Column("message")] public string Message { get; set; }

    [Required] [Column("status")] public RequestStatus Status { get; set; } = RequestStatus.Pending;
}