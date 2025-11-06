using System;
using System.Linq;
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
        private readonly UserServiceImpl _userService;
        private List<User> _allUsers = new();

        public AddOperatorPage()
        {
            _userService = new UserServiceImpl();
            InitializeComponent();
            RefreshUsersList();
        }

        private void RefreshUsersList()
        {
            _allUsers = new List<User>(_userService.GetAllByAccessRight(AccessRight.Authorized));
            ApplyFilter(SearchBox?.Text);
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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter(SearchBox.Text);
        }

        private void ApplyFilter(string? query)
        {
            if (_allUsers == null) return;

            IEnumerable<User> filtered = _allUsers;
            if (!string.IsNullOrWhiteSpace(query))
            {
                filtered = filtered.Where(u =>
                    u.Login != null &&
                    u.Login.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            UsersList.ItemsSource = filtered.ToList();
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
