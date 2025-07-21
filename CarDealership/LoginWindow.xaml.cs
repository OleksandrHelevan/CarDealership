using System.Windows;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.service;
using CarDealership.service.impl;

namespace CarDealership
{
    public partial class LoginWindow : Window
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
                MessageBox.Show("⚠️ Enter login and password.");
                return;
            }

            AccessRight selectedRight = (AccessRight)AccessComboBox.SelectedIndex;

            User? user = _userService.Login(login, password, selectedRight);

            if (user != null)
            {
                MessageBox.Show($"✅ Welcome, {user.Login} ({user.AccessRight})!", "Login Successful");

                // Відкрити MainWindow
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                
            }
            else
            {
                MessageBox.Show("❌ Invalid login, password or access right.", "Login Failed");
            }

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("⚠️ Enter login and password.");
                return;
            }

            bool success = _userService.Register(login, password, AccessRight.Authorized);

            if (success)
            {
                MessageBox.Show("✅ Registered successfully!", "Registration");
            }
            else
            {
                MessageBox.Show("❌ Login already exists.", "Registration Failed");
            }
        }
    }
}
