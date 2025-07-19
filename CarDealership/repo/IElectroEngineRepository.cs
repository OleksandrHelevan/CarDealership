using CarDealership.entity;

namespace CarDealership.repo;

public interface IElectroEngineRepository
{
    IEnumerable<ElectroEngine> GetAll();
    ElectroEngine GetById(int id);
    void Add(ElectroEngine engine);
    void Update(ElectroEngine engine);
    void Delete(int id);
    void Save(); 
}