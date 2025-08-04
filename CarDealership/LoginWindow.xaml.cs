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
                MessageBox.Show("Enter login and password.");
                return;
            }

            AccessRight selectedRight = (AccessRight)AccessComboBox.SelectedIndex;

            try
            {
                UserDto? user = _userService.Login(login, password, selectedRight);


                if (user != null)
                {
                    MessageBox.Show($"Welcome, {user.Login} ({user.AccessRight})!", "Login Successful");

                    GuestWindow mainWindow = new GuestWindow();
                    mainWindow.Show();
                }
                else
                {
                    MessageBox.Show("Invalid login, password or access right.", "Login Failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Login Failed");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Enter login and password.");
                return;
            }

            bool success = _userService.Register(login, password, AccessRight.Guest);

            if (success)
            {
                MessageBox.Show("Registered successfully! With access rights - Guest", "Registration");
            }
            else
            {
                MessageBox.Show("Login already exists.", "Registration Failed");
            }
        }
    }
}