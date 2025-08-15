using System.Windows;
using CarDealership.enums;
using CarDealership.dto;
using CarDealership.service;
using CarDealership.service.impl;

namespace CarDealership
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

            AccessRight selectedRight = (AccessRight)AccessComboBox.SelectedIndex;

            try
            {
                UserDto? user = _userService.Login(login, password, selectedRight);

                if (user != null)
                {
                    MessageBox.Show($"Ласкаво просимо, {user.Login} ({user.AccessRight})!", "Вхід успішний");

                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Невірний логін, пароль або рівень доступу.", "Вхід не вдалось");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Вхід не вдалось");
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
