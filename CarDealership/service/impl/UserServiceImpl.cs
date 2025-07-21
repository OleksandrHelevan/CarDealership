using CarDealership.entity;
using CarDealership.enums;
using CarDealership.repo;
using CarDealership.repo.impl;

namespace CarDealership.service.impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepositoryImpl();
        }

        public bool Register(string login, string password, AccessRight accessRight)
        {
            if (_userRepository.ExistsByLogin(login))
                return false;

            var user = new User
            {
                Login = login,
                Password = password,
                AccessRight = accessRight
            };

            _userRepository.Save(user);
            return true;
        }

        public User? Login(string login, string password, AccessRight accessRight)
        {
            var user = new User
            {
                Login = login,
                Password = password,
                AccessRight = accessRight
            };

            return _userRepository.Exists(user)
                ? _userRepository.GetByLogin(login)
                : null;
        }
    }
}