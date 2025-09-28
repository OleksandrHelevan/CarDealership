using CarDealership.enums;

namespace CarDealership.dto;
public class ClientDto : UserDto
{
    public int Id { get; set; }
    public int UserId { get; set; } // додати
    public PassportDataDto PassportData { get; set; }

    public ClientDto(int id, int userId, string login, string password, AccessRight accessRight, PassportDataDto passportData)
        : base(id, login, password, accessRight)
    {
        Id = id;
        UserId = userId; // правильно
        PassportData = passportData;
    }
}
