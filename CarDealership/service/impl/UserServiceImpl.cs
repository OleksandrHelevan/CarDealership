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

        public UserDto? Login(string login, string password)
        {
            var userFromDb = _userRepository.GetByLogin(login);

            if (userFromDb == null)
            {
                throw new UserNotFoundException($"Користувач з логіном '{login}' не знайдений.");
            }
            if (userFromDb.Password != DealershipPasswordEncoder.Encode(password))
            {
                throw new InvalidPasswordException("Невірний пароль.");
            }

            return UserMapper.ToDto(userFromDb);
        }


        public bool UpdatePassword(string login, string password)
        {
            var user = _userRepository.GetByLogin(login);
            if (user == null)
                throw new UserNotFoundException($"User with '{login}' not found.");
            user.Password = DealershipPasswordEncoder.Encode(password);
            _userRepository.Update(user);
            return true;
        }
    }
}