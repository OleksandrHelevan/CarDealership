using CarDealership.entity;
using System.Collections.Generic;

namespace CarDealership.service
{
    public interface IElectroCarService
    {
        IEnumerable<ElectroCar> GetAllCars();
        ElectroCar? GetCarById(int id);
        void AddCar(ElectroCar car);
        void UpdateCar(ElectroCar car);
        void DeleteCar(int id);
    }
}