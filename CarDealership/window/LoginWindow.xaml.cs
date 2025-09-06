using System.Windows;
using CarDealership.enums;
using CarDealership.dto;
using CarDealership.exception;
using CarDealership.service;
using CarDealership.service.impl;

namespace CarDealership.window
{
    public partial class LoginWindow
    {
        private readonly IUserService _userService;

        public LoginWindow()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введіть логін та пароль.", "Помилка");
                return;
            }

            try
            {
                UserDto user = _userService.Login(login, password)!;

                MessageBox.Show($"Ласкаво просимо, {user.Login} (Права доступу - {user.AccessRight.ToFriendlyString()})!", "Вхід успішний");
                
                if (user.AccessRight == AccessRight.Guest)
                {
                    GuestWindow guestWindow = new GuestWindow(user.Login);
                    guestWindow.Show();

                    DialogResult = true;
                    Close();
                }
                else if(user.AccessRight == AccessRight.Authorized)
                {
                    AuthorizedWindow authorizedWindow = new AuthorizedWindow(user.Login);
                    authorizedWindow.Show();
                    
                    DialogResult = true;
                    Close();
                }
            }
            catch (UserNotFoundException)
            {
                MessageBox.Show("Користувача з таким логіном не знайдено.", "Вхід не вдалось");
            }
            catch (InvalidPasswordException)
            {
                MessageBox.Show("Невірний пароль.", "Вхід не вдалось");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Вхід не вдалось");
            }
        }



        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введіть логін та пароль.", "Помилка");
                return;
            }

            bool success = _userService.Register(login, password, AccessRight.Guest);

            if (success)
            {
                MessageBox.Show("Реєстрація пройшла успішно! Рівень доступу - Гість", "Реєстрація");
            }
            else
            {
                MessageBox.Show("Логін вже існує.", "Не вдалося зареєструватися");
            }
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Введіть логін.", "Помилка");
                return;
            }

            ResetPasswordWindow resetWindow = new ResetPasswordWindow(_userService, login);
            resetWindow.ShowDialog();
        }
    }
}