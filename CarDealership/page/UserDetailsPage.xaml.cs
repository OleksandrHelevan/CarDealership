using System.Windows;
using System.Windows.Controls;
using CarDealership.config;
using CarDealership.enums;
using CarDealership.service;
using CarDealership.service.impl;

namespace CarDealership.page
{
    public partial class UserDetailsPage : Page
    {
        private readonly DealershipContext _context;
        private readonly string _userLogin;
        private readonly IUserService _userService;

        public UserDetailsPage(string userLogin)
        {
            InitializeComponent();
            _context = new DealershipContext();
            _userLogin = userLogin;
            _userService = new UserServiceImpl();

            LoadUserDetails();
        }

        private void LoadUserDetails()
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == _userLogin);
            if (user == null)
            {
                LoginValue.Text = _userLogin;
                RoleValue.Text = "-";
                FirstNameValue.Text = "-";
                LastNameValue.Text = "-";
                PassportValue.Text = "-";
                return;
            }

            LoginValue.Text = user.Login;
            RoleValue.Text = user.AccessRight.ToFriendlyString();

            var client = _context.Clients.FirstOrDefault(c => c.UserId == user.Id);
            if (client != null)
            {
                var passport = _context.PassportData.FirstOrDefault(p => p.Id == client.PassportDataId);
                FirstNameValue.Text = passport?.FirstName ?? "-";
                LastNameValue.Text = passport?.LastName ?? "-";
                PassportValue.Text = passport?.PassportNumber ?? "-";
            }
            else
            {
                FirstNameValue.Text = "-";
                LastNameValue.Text = "-";
                PassportValue.Text = "-";
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new ResetPasswordWindow(_userService, _userLogin);
            wnd.ShowDialog();
        }
    }
}

