using CarDealership.dto;
using CarDealership.entity;

namespace CarDealership.service
{
    public interface IGasolineCarService
    {
        IEnumerable<CarDto> GetAll();
        GasolineCar? GetById(int id);
        void Add(CarDto car);
        void Update(CarDto car);
        void Delete(int id);
    }
}
