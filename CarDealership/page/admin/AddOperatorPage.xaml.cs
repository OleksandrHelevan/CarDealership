using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.service.impl;
using CarDealership.window;

namespace CarDealership.page.admin
{
    public partial class AddOperatorPage : Page
    {
        private readonly UserServiceImpl _userService = new UserServiceImpl();

        public AddOperatorPage()
        {
            InitializeComponent();
            RefreshUsersList();
        }

        private void RefreshUsersList()
        {
            List<User> users = new List<User>(_userService.GetAllByAccessRight(AccessRight.Authorized));
            UsersDataGrid.ItemsSource = users;
        }

        private void AddOperatorButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(AccessRight.Operator);
            registerWindow.Title = "Додати оператора";
            if (registerWindow.ShowDialog() == true)
            {
                string login = registerWindow.LoginBox.Text.Trim();
                string password = registerWindow.PasswordBox.Password;

                bool success = _userService.Register(login, password, AccessRight.Operator);

                if (success)
                    MessageBox.Show($"Оператор '{login}' доданий успішно!");
                else
                    MessageBox.Show("Не вдалося додати оператора. Можливо, логін вже існує.");

                RefreshUsersList();
            }
        }

        private void AssignOperatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is User user)
            {
                user.AccessRight = AccessRight.Operator;
                bool success = _userService.Update(user);

                if (success)
                    MessageBox.Show($"Користувачу '{user.Login}' призначено права оператора!");
                else
                    MessageBox.Show("Не вдалося оновити користувача.");

                RefreshUsersList();
            }
        }
    }
}
