using System.Windows;
using CarDealership.page.admin;

namespace CarDealership.window;

public partial class AdminWindow : Window
{
    private readonly string _currentLogin;
    public AdminWindow(String login)
    {
        InitializeComponent();
        _currentLogin = login;
    }
}