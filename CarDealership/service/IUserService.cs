using CarDealership.dto;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.service
{
    public interface IUserService
    {
        bool Register(string login, string password, AccessRight accessRight);
        UserDto? Login(string login, string password);
        
        bool UpdatePassword(string login, string newPassword);
        User LoadByUsername(string login);
        User? LoadByEmail(string email);
        bool Update(User user);
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetAllByAccessRight(AccessRight accessRight);

    }
    
}
