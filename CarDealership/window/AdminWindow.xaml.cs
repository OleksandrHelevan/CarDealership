using System.Windows;
using CarDealership.page;
using CarDealership.page.admin;
using CarDealership.page.@operator;
using CarDealership.page.query;

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

    private void BtnUnboundCars_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new UnboundCarsPage());
    }

    private void BtnShowRequest_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new UserRequestsPage());
    }

    private void BtnAddCar_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new AddCarPage());
    }

    private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new EditProductPage());
    }

    private void BtnMyAccount_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new UserDetailsPage(_currentLogin));
    }

    private void BtnCommandPrompt_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new SqlConsolePage());
    }

    private void showOrders_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new OrderReviewPage(_currentLogin));
    }

    private void BtnClients_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ClientsDetailsPage());
    }

    private void BtnUsersCarsByPayment_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new UsersAndCarsPaymentTypePage());
    }

    private void BtnMinStock_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new PopularCarsQuarterPage());
    }

    private void BtnWaitingCustomers_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new WaitingCustomersPage());
    }

    private void BtnDealerContracts_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new DealerContractsCountPage());
    }
}
