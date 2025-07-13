using CarDealership.entity;

namespace CarDealership.repo;

public interface IGasolineEngineRepository
{
    IEnumerable<GasolineEngineEntity> GetAll();
    GasolineEngineEntity GetById(int id);
    void Add(GasolineEngineEntity engine);
    void Update(GasolineEngineEntity engine);
    void Delete(int id);
    void Save();
}