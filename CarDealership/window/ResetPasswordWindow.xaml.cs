using System.Windows;
using CarDealership.service;

namespace CarDealership
{
    public partial class ResetPasswordWindow : Window
    {
        private readonly IUserService _userService;
        private readonly string _login;

        public ResetPasswordWindow(IUserService userService, string login)
        {
            InitializeComponent();
            _userService = userService;
            _login = login;
        }

        private void UpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Введіть новий пароль та підтвердження.", "Помилка");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Паролі не збігаються.", "Помилка");
                return;
            }

            bool success = _userService.UpdatePassword(_login, newPassword);

            if (success)
            {
                MessageBox.Show("Пароль успішно оновлено!", "Оновлення пароля");
                this.Close();
            }
            else
            {
                MessageBox.Show("Логін не існує.", "Помилка");
            }
        }
    }
}