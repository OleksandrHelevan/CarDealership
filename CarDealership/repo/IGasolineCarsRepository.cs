using CarDealership.entity;
using CarDealership.dto;
using System.Collections.Generic;

namespace CarDealership.repo
{
    public interface IGasolineCarRepository
    {
        IEnumerable<GasolineCar> GetAll();
        GasolineCar? GetById(int id);
        IEnumerable<GasolineCar> GetFiltered(GasolineCarFilterDto filter);
        void Add(GasolineCar car);
        void Update(GasolineCar car);
        void Delete(int id);
    }
}