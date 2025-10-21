using System.Collections.Generic;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.service
{
    public interface ICarService
    {
        List<Car> GetAll();
        Car? GetById(int id);
        List<Car> GetByType(CarType type);
        void Add(Car car);
        void Update(Car car);
        void Delete(int id);
    }
}

