using System.Windows;
using CarDealership.config;
using CarDealership.exception;
using CarDealership.page.guest;
using CarDealership.repo.impl;
using CarDealership.service;
using CarDealership.service.impl;

namespace CarDealership.window
{
    public partial class GuestWindow
    {
        private readonly string _currentUserLogin;
        private readonly IAuthorizationRequestService _requestService;
        public GuestWindow(string login)
        {
            InitializeComponent();
            _requestService = new AuthorizationRequestService(new AuthorizationRequestRepository(new DealershipContext()));
            _currentUserLogin = login; 
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
        
        private void BtnAuthRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _requestService.CreateRequest(_currentUserLogin);
                MessageBox.Show($"Запит на авторизацію створено!", "Успіх");
            }
            catch (RequestAlreadyExistException ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }
    }
}