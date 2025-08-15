using CarDealership.dto;
using CarDealership.enums;

namespace CarDealership.service
{
    public interface IUserService
    {
        bool Register(string login, string password, AccessRight accessRight);
        UserDto? Login(string login, string password, AccessRight accessRight);
        
        bool UpdatePassword(string login, string newPassword);
    }
}