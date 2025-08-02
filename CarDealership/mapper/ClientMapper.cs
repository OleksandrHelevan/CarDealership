using CarDealership.entity;
using CarDealership.dto;

namespace CarDealership.mapper;

public class ClientMapper
{
    public static ClientDto ToDto(Client e)
    {
        return new ClientDto(
            e.User.Login,
            e.User.Password,
            e.User.AccessRight,
            PassportDataMapper.ToDto(e.PassportData)
        );
    }

    public static Client ToEntity(ClientDto dto)
    {
        return new Client
        {
            User = new User
            {
                Login = dto.Login,
                Password = dto.Password,
                AccessRight = dto.AccessRight
            },
            PassportData = PassportDataMapper.ToEntity(dto.PassportData)
        };
    }
}