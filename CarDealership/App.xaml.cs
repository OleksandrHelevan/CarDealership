using System.Configuration;
using System.Data;
using System.Windows;

namespace CarDealership;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = new MainWindow();
        Current.MainWindow = mainWindow;

        var loginWindow = new LoginWindow();
        bool? result = loginWindow.ShowDialog();
        Console.WriteLine($"Login dialog result: {result}");

        if (result == true)
        {
            mainWindow.Show();
        }
        else
        {
            Shutdown();
        }
    }
}