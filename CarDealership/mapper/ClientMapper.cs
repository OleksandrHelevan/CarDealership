using CarDealership.entity;
using CarDealership.model;

namespace CarDealership.mapper;

public class ClientMapper
{
    public static ClientDto ToDto(Client e)
    {
        return new ClientDto(e.User.Login, e.User.Password, e.User.AccessRight,
            PassportDataMapper.ToDto(e.PassportData));
    }
}