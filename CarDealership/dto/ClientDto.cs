using CarDealership.enums;

namespace CarDealership.dto;

public class ClientDto : UserDto
{
    public int Id { get; set; }
    public PassportDataDto PassportData { get; set; }

    public ClientDto(int id, string login, string password, AccessRight accessRight, PassportDataDto passportData)
        : base(id, login, password, accessRight)
    {
        PassportData = passportData;
    }
}