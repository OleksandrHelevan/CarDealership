using System.Windows;
using CarDealership.page;
using CarDealership.page.admin;
using CarDealership.page.authorized;
using CarDealership.page.@operator;

namespace CarDealership.window;

public partial class AdminWindow : Window
{
    private readonly string _currentLogin;
    public AdminWindow(String login)
    {
        InitializeComponent();
        _currentLogin = login;
    }

    private void BtnAddOperator_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new AddOperatorPage());
    }
    
    private void BtnOperatorFunc_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new OperatorWindow(_currentLogin));
    }
    
    private void BtnUnboundCars_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new UnboundCarsPage());
    }

    private void BtnShowRequest_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new UserRequestsPage());
    }

    private void BtnAddGasolineCar_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new AddGasolineCarPage());
    }

    private void BtnAddElectroCar_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new AddElectroCarPage());
    }
    
    private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new EditProductPage());
    }
    
    private void BtnMyAccount_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new UserDetailsPage(_currentLogin));
    }
}