using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity;

[Table("requests")]
public class AuthorizationRequest : INotifyPropertyChanged
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public int UserId { get; set; }

    public User User { get; set; }

    private RequestStatus _status;
    [Required]
    [Column("status")]
    public RequestStatus Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}