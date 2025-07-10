using CarDealership.Models.Enums;

namespace CarDealership.Models;

public class User
{
    private string Login { get; set; }
    private string Password { get; set; }
    private AccessRight AccessRight { get; set; }

    public User(string login, string password, AccessRight accessRight)
    {
        Login = login;
        Password = password;
        AccessRight = accessRight;
    }
    
}