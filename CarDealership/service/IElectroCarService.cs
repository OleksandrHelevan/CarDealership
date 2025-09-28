using CarDealership.entity;
using System.Collections.Generic;
using CarDealership.dto;

namespace CarDealership.service
{
    public interface IElectroCarService
    {
        IEnumerable<ElectroCarDto> GetAllCars();
        IEnumerable<ElectroCarDto> GetFilteredCars(ElectroCarFilterDto filter);
        ElectroCar? GetCarById(int id);
        void AddCar(ElectroCarDto electroCar);
        void UpdateCar(ElectroCarDto electroCar);
        void DeleteCar(int id);
    }
}