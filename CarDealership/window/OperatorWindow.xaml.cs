using System.Windows;
using CarDealership.page.admin;

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
    
}