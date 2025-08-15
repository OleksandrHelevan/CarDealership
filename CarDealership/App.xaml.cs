using System.Windows;
using CarDealership.window;

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
        
        if (loginResult != true)
        {
            MessageBox.Show("Вхід не виконано. Додаток буде закрито.", "Інформація");
            Shutdown();
        }
    }



}