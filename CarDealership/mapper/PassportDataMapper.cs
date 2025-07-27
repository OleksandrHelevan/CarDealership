using CarDealership.entity;
using CarDealership.model;

namespace CarDealership.mapper;

public class PassportDataMapper
{
    public static PassportDataDto ToDto(PassportData e)
    {
        return new PassportDataDto(e.FirstName, e.LastName, e.PassportNumber);
    }

    public static PassportData ToEntity(PassportDataDto dto)
    {
        return new PassportData
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PassportNumber = dto.PassportNumber
        };
    }
}