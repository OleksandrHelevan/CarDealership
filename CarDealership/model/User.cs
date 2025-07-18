using CarDealership.enums;

namespace CarDealership.model;

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