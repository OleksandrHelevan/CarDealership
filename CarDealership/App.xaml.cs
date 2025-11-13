using System.Windows;
using CarDealership.window;
using CarDealership.util;

namespace CarDealership;

public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        KeyboardShortcuts.Register();

        var loginWindow = new LoginWindow();
        bool? loginResult = loginWindow.ShowDialog();

        if (loginResult != true)
        {
            MessageBox.Show("Вхід не виконано. Програму буде завершено.", "Помилка");
            Shutdown();
        }
    }
}
