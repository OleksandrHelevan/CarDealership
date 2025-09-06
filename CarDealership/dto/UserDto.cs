using CarDealership.enums;

namespace CarDealership.dto;

public class UserDto
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public AccessRight AccessRight { get; set; }

    public UserDto(int id, string login, string password, AccessRight accessRight)
    {
        Login = login;
        Password = password;
        AccessRight = accessRight;
    }
    
}