using CarDealership.entity;
using System.Collections.Generic;

namespace CarDealership.repo
{
    public interface IElectroCarRepository
    {
        IEnumerable<ElectroCar> GetAll();
        ElectroCar? GetById(int id);
        void Add(ElectroCar car);
        void Update(ElectroCar car);
        void Delete(int id);
    }
}