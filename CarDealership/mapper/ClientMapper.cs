using CarDealership.entity;
using CarDealership.dto;

namespace CarDealership.mapper;

public class ClientMapper
{
    public static ClientDto ToDto(Client e)
    {
        return new ClientDto(
            e.Id,
            e.UserId,
            e.User.Login,
            "",
            e.User.AccessRight,
            PassportDataMapper.ToDto(e.PassportData)
        );
    }

    public static Client ToEntity(ClientDto dto)
    {
        return new Client
        {
            Id = dto.Id,
            UserId = dto.UserId,
            PassportDataId = dto.PassportData.Id
        };
    }
}