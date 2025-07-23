using CarDealership.entity;
using CarDealership.mapper;
using CarDealership.model;
using CarDealership.repo;


namespace CarDealership.service.impl
{
    public class PassportDataServiceImpl : IPassportDataService
    {
        private readonly IPassportDataRepository _repository;

        public PassportDataServiceImpl(IPassportDataRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<PassportDataDto> GetAll()
        {
            return _repository.GetAll().Select(PassportDataMapper.ToDto).ToList();
        }

        public PassportDataDto? GetById(int id)
        {
            return PassportDataMapper.ToDto(_repository.GetById(id));
        }

        public PassportDataDto? GetByPassportNumber(string passportNumber)
        {
            return PassportDataMapper.ToDto(_repository.GetByPassportNumber(passportNumber));
        }

        public void Create(PassportData passportData)
        {
            _repository.Add(passportData);
        }

        public void Update(PassportData passportData)
        {
            _repository.Update(passportData);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}