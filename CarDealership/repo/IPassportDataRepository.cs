using CarDealership.entity;

namespace CarDealership.repo
{
    public interface IPassportDataRepository
    {
        IEnumerable<PassportData> GetAll();
        PassportData? GetById(int id);
        PassportData? GetByPassportNumber(string passportNumber);
        void Add(PassportData passportData);
        void Update(PassportData passportData);
        void Delete(int id);
    }
}