using CarDealership.entity;
using CarDealership.dto;
using System.Collections.Generic;

namespace CarDealership.repo
{
    public interface IElectroCarRepository
    {
        IEnumerable<ElectroCar> GetAll();
        ElectroCar? GetById(int id);
        IEnumerable<ElectroCar> GetFiltered(ElectroCarFilterDto filter);
        void Add(ElectroCar car);
        void Update(ElectroCar car);
        void Delete(int id);
    }
}