using System.Configuration;
using System.Data;
using System.Windows;
using CarDealership.page;

namespace CarDealership;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var loginWindow = new LoginWindow();
        bool? loginResult = loginWindow.ShowDialog();

        if (loginResult == true)
        {
            var mainWindow = new GuestWindow(loginWindow.UsernameTextBox.Text);
            Current.MainWindow = mainWindow;
            mainWindow.Show();
        }
        else
        {
        
            MessageBox.Show("Вхід не виконано. Додаток буде закрито.", "Інформація");
            Shutdown();
        }
    }


}