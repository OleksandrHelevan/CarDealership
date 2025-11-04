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
            e.User.Password,
            e.User.AccessRight,
            PassportDataMapper.ToDto(e.PassportData)
        );
    }
}