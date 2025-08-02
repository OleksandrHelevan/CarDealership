using CarDealership.enums;

namespace CarDealership.dto;

public class UserDto
{
    public string Login { get; set; }
    public string Password { get; set; }
    public AccessRight AccessRight { get; set; }

    public UserDto(string login, string password, AccessRight accessRight)
    {
        Login = login;
        Password = password;
        AccessRight = accessRight;
    }
    
}