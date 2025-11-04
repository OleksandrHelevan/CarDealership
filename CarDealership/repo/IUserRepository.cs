using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.repo;

public interface IUserRepository
{
    void Save(User user);
    bool ExistsByLogin(string login);
    User? GetByLogin(string login);
    User? GetByEmail(string email);
    public void Update(User user);
    IEnumerable<User> GetAll();
    IEnumerable<User> GetAllByAccessRight(AccessRight accessRight);
}