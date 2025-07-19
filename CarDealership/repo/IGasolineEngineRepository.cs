using CarDealership.entity;

namespace CarDealership.repo;

public interface IGasolineEngineRepository
{
    IEnumerable<GasolineEngine> GetAll();
    GasolineEngine GetById(int id);
    void Add(GasolineEngine engine);
    void Update(GasolineEngine engine);
    void Delete(int id);
    void Save();
}