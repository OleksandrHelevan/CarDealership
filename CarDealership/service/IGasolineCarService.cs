using CarDealership.entity;

namespace CarDealership.service
{
    public interface IGasolineCarService
    {
        IEnumerable<GasolineCar> GetAll();
        GasolineCar? GetById(int id);
        void Add(GasolineCar car);
        void Update(GasolineCar car);
        void Delete(int id);
    }
}
