using CarDealership.entity;
using CarDealership.dto;

namespace CarDealership.mapper;

public class UserMapper
{
    public static User ToEntity(UserDto userDto)
    {
        return new User
        {
            Id = userDto.Id,
            Login = userDto.Login,
            PasswordHash = config.decoder.DealershipPasswordEncoder.Encode(userDto.Password),
            AccessRight = userDto.AccessRight
        };
    }

    public static UserDto ToDto(User user)
    {
        return new UserDto(user.Id, user.Login, "", user.AccessRight);
    }
}