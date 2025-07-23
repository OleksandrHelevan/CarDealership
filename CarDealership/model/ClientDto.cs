using CarDealership.enums;

namespace CarDealership.model;

public class ClientDto : UserDto
{
    public PassportDataDto PassportData { get; set; }

    public ClientDto(string login, string password, AccessRight accessRight, PassportDataDto passportData)
        : base(login, password, accessRight)
    {
        PassportData = passportData;
    }
}