using CarDealership.entity;
using System.Collections.Generic;

namespace CarDealership.repo
{
    public interface IGasolineCarRepository
    {
        IEnumerable<GasolineCar> GetAll();
        GasolineCar? GetById(int id);
        void Add(GasolineCar car);
        void Update(GasolineCar car);
        void Delete(int id);
    }
}