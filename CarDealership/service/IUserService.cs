using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.service
{
    public interface IUserService
    {
        bool Register(string login, string password, AccessRight accessRight);
        User? Login(string login, string password, AccessRight accessRight);
    }
}