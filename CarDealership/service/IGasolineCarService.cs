using CarDealership.dto;
using CarDealership.entity;

namespace CarDealership.service
{
    public interface IGasolineCarService
    {
        IEnumerable<GasolineCarDto> GetAll();
        GasolineCar? GetById(int id);
        IEnumerable<GasolineCarDto> GetFiltered(GasolineCarFilterDto filter);
        void Add(GasolineCarDto electroCar);
        void Update(GasolineCarDto electroCar);
        void Delete(int id);
    }
}
