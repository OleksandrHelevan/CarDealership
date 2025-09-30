using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity;

[Table("requests")]
public class AuthorizationRequest : INotifyPropertyChanged
{
    [Column("id")]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    private string _login;
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

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}