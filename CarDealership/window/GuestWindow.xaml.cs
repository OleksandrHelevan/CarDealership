using System.Windows;
using CarDealership.page;
using CarDealership.page.guest;

namespace CarDealership.window
{
    public partial class GuestWindow
    {
        private readonly string _currentUserLogin;

        public GuestWindow(string login)
        {
            InitializeComponent();
            _currentUserLogin = login;
            MainFrame.Navigate(new GuestCarsPage());
        }

        private void BtnCars_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new GuestCarsPage());
        }

        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserDetailsPage(_currentUserLogin));
        }
    }
}
