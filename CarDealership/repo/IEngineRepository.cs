using System.Collections.Generic;
using CarDealership.entity;

namespace CarDealership.repo
{
    public interface IEngineRepository
    {
        IEnumerable<Engine> GetAll();
        Engine? GetById(int id);
        void Add(Engine engine);
        void Update(Engine engine);
        void Delete(int id);
    }
}

