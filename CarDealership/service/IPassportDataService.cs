using CarDealership.entity;
using CarDealership.dto;

namespace CarDealership.service
{
    public interface IPassportDataService
    {
        IEnumerable<PassportDataDto> GetAll();
        PassportDataDto? GetById(int id);
        PassportDataDto? GetByPassportNumber(string passportNumber);
        void Create(PassportData passportData);
        void Update(PassportData passportData);
        void Delete(int id);
    }
}