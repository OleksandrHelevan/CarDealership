using CarDealership.entity;
using CarDealership.model;

namespace CarDealership.mapper;

public class UserMapper
{
    public static User ToEntity(UserDto userDto)
    {
        return new User
        {
            Login = userDto.Login, Password = userDto.Password, AccessRight = userDto.AccessRight
        };
    }

    public static UserDto ToDto(User user)
    {
        return new UserDto(user.Login, user.Password, user.AccessRight);
    }
}