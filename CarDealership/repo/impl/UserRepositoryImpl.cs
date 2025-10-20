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
            var existing = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existing == null)
            {
                // If not found, attach and mark modified as a fallback
                _context.Users.Attach(user);
                _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                existing.Login = user.Login;
                existing.PasswordHash = user.PasswordHash;
                existing.AccessRight = user.AccessRight;
                existing.IsActive = user.IsActive;
                existing.LastLoginAt = user.LastLoginAt;
            }
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
                u.PasswordHash == user.PasswordHash &&
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
