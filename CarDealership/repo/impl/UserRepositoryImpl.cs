using CarDealership.config;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.repo.impl
{
    public class UserRepositoryImpl :  IUserRepository
    {
        private readonly DealershipContext _context;

        public UserRepositoryImpl()
        {
            _context = new DealershipContext();
        }

        public void Save(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public bool ExistsByLogin(string login)
        {
            return _context.Users.Any(u => u.Login == login);
        }

        public bool Exists(User user)
        {
            return _context.Users.Any(u =>
                u.Login == user.Login &&
                u.Password == user.Password &&
                u.AccessRight == user.AccessRight);
        }

        public User? GetByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }
        
        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }
        
        public IEnumerable<User> GetAllByAccessRight(AccessRight accessRight)
        {
            return _context.Users.Where(u => u.AccessRight == accessRight).ToList();
        }
    }
}