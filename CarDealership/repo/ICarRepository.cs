using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.repo
{
    public interface ICarRepository
    {
        IEnumerable<Car> GetAll();
        Car? GetById(int id);
        IEnumerable<Car> GetByType(CarType type);
        void Add(Car car);
        void Update(Car car);
        void Delete(int id);
    }
}

