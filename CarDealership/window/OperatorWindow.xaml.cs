using System.Windows;

namespace CarDealership.window;

public partial class OperatorWindow : Window
{
    private readonly string _currentLogin;
    public OperatorWindow(String login)
    {
        InitializeComponent();
        _currentLogin = login;
    }
}