using CarDealership.config.decoder;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.mapper;
using CarDealership.dto;
using CarDealership.exception;
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
                Password = DealershipPasswordEncoder.Encode(password),
                AccessRight = accessRight
            };

            _userRepository.Save(user);
            return true;
        }

        public UserDto? Login(string login, string password, AccessRight accessRight)
        {
            Console.WriteLine(DealershipPasswordEncoder.Encode(password));

            var user = new User
            {
                Login = login,
                Password = DealershipPasswordEncoder.Encode(password),
                AccessRight = accessRight
            };

            return _userRepository.Exists(user)
                ? UserMapper.ToDto(_userRepository.GetByLogin(login))
                : throw new UserNotFoundException($"User with '{login}' not found.");
        }
    }
}