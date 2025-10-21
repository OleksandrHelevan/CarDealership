using System.Collections.Generic;
using CarDealership.entity;

namespace CarDealership.service
{
    public interface IEngineService
    {
        List<Engine> GetAll();
        Engine? GetById(int id);
        void Add(Engine engine);
        void Update(Engine engine);
        void Delete(int id);
    }
}

