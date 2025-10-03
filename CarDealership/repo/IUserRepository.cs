using CarDealership.config;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.repo;

public interface IUserRepository
{
    void Save(User user);

    void Delete(User user);

    bool ExistsByLogin(string login);

    bool Exists(User user);

    User? GetByLogin(string login);
    
    public void Update(User user);
    
    IEnumerable<User> GetAll();
    
    IEnumerable<User> GetAllByAccessRight(AccessRight accessRight);
    
}