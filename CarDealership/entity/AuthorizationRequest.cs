using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using CarDealership.enums;

namespace CarDealership.entity;

[Table("authorization_requests")]
public class AuthorizationRequest : INotifyPropertyChanged
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    private string _login = null!;
    [Required]
    [MaxLength(100)]
    [Column("login")]
    public string Login
    {
        get => _login;
        set { _login = value; OnPropertyChanged(); }
    }

    private RequestStatus _status;
    [Required]
    [Column("status")]
    public RequestStatus Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    [Column("requested_at")]
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    [Column("processed_at")]
    public DateTime? ProcessedAt { get; set; }

    [Column("processed_by")]
    public int? ProcessedBy { get; set; }

    [MaxLength(500)]
    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    [ForeignKey(nameof(ProcessedBy))]
    public virtual User? ProcessedByUser { get; set; }

    // Computed properties
    [NotMapped]
    public string StatusString => Status.ToFriendlyString();

    [NotMapped]
    public bool IsPending => Status == RequestStatus.Pending;

    [NotMapped]
    public bool IsProcessed => Status != RequestStatus.Pending;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}