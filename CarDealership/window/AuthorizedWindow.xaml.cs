using System.Windows;
using CarDealership.page;
using CarDealership.page.authorized;

namespace CarDealership.window;

public partial class AuthorizedWindow 
{
    private readonly string _currentUserLogin;
    
    public AuthorizedWindow(string login)
    {
        InitializeComponent();
        _currentUserLogin = login;
    }
    
    private void BtnGasolineCar_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ProductsPage(_currentUserLogin));
    }
    
    private void BtnMyOrders_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new MyOrdersPage(_currentUserLogin));
    }

    private void BtnMyAccount_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new UserDetailsPage(_currentUserLogin));
    }

}
