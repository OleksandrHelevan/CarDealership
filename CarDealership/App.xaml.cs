using System.Windows;
using CarDealership.window;
using CarDealership.service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CarDealership;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            // Create and configure the host
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Add logging
                    services.AddLogging(builder =>
                    {
                        builder.AddConsole();
                        builder.AddDebug();
                    });

                    // Add database services
                    services.AddDatabaseServices();

                    // Add hosted service for database initialization
                    services.AddHostedService<DatabaseStartupService>();
                })
                .Build();

            // Start the host
            await _host.StartAsync();

            // Wait for database initialization to complete
            await Task.Delay(2000); // Give time for database initialization

            // Show login window
            var loginWindow = new LoginWindow();
            bool? loginResult = loginWindow.ShowDialog();
            
            if (loginResult != true)
            {
                MessageBox.Show("Вхід не виконано. Додаток буде закрито.", "Інформація");
                Shutdown();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка ініціалізації бази даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host != null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }
        
        base.OnExit(e);
    }
}