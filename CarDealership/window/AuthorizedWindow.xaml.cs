using System.Windows;
using CarDealership.page.authorized;

namespace CarDealership.window;

public partial class AuthorizedWindow 
{
    
    
    public AuthorizedWindow()
    {
        InitializeComponent();
    }
    
    private void BtnGasolineCar_Click(object sender, RoutedEventArgs e)
    {
        InitializeComponent();
        MainFrame.Navigate(new GasolineCarPage());
    }

    private void BtnElectroCar_Click(object sender, RoutedEventArgs e)
    {
       
    }
}