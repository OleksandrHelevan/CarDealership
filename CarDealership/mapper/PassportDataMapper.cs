using CarDealership.entity;
using CarDealership.model;

namespace CarDealership.mapper;

public class PassportDataMapper
{
    public static PassportDataDto ToDto(PassportData e)
    {
        return new PassportDataDto(e.FirstName, e.LastName, e.PassportNumber);
    }
}