using System.Windows;
using CarDealership.page;

namespace CarDealership
{
    public partial class GuestWindow : Window
    {
        public GuestWindow()
        {
            InitializeComponent();
            
            MainFrame.Navigate(new GasolineEnginePage());
        }

        private void BtnGasoline_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new GasolineEnginePage());
        }

        private void BtnElectro_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ElectroEnginePage());
        }
    }
}