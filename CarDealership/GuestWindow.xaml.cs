using System.Windows;
using CarDealership.page;

namespace CarDealership
{
    public partial class GuestWindow
    {
        public GuestWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(new GasolineCarPage());
        }

        private void BtnGasolineCar_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new GasolineCarPage());
        }

        private void BtnElectroCar_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ElectroCarPage());
        }
    }
}