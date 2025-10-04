using System.Windows;
using CarDealership.page.admin;
using CarDealership.page.@operator;

namespace CarDealership.window;

public partial class OperatorWindow : Window
{
    private readonly string _currentLogin;
    public OperatorWindow(String login)
    {
        InitializeComponent();
        _currentLogin = login;
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
}