using System.Collections.Generic;
using System.Linq;
using CarDealership.entity;
using CarDealership.repo;

namespace CarDealership.service.impl
{
    public class EngineServiceImpl : IEngineService
    {
        private readonly IEngineRepository _repository;

        public EngineServiceImpl(IEngineRepository repository)
        {
            _repository = repository;
        }

        public List<Engine> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public Engine? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Engine engine)
        {
            _repository.Add(engine);
        }

        public void Update(Engine engine)
        {
            _repository.Update(engine);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}

