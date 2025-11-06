using System.Windows;
using CarDealership.config;
using CarDealership.enums;
using CarDealership.exception;
using CarDealership.page;
using CarDealership.page.guest;
using CarDealership.repo.impl;
using CarDealership.service;
using CarDealership.service.impl;

namespace CarDealership.window
{
    public partial class GuestWindow
    {
        private readonly string _currentUserLogin;
        private readonly IUserService _userService;
        private readonly IAuthorizationRequestService _authorizationRequestService;

        public GuestWindow(string login)
        {
            InitializeComponent();
            _currentUserLogin = login;
            _userService = new UserServiceImpl();
            _authorizationRequestService =
                new AuthorizationRequestService(new AuthorizationRequestRepository(new DealershipContext()));

            UpdateRequestButtonState();

            MainFrame.Navigate(new GuestCarsPage());
        }

        private void BtnCars_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new GuestCarsPage());
        }

        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserDetailsPage(_currentUserLogin));
        }

        private void BtnRequestAccess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = _userService.LoadByUsername(_currentUserLogin);
                if (user == null)
                {
                    MessageBox.Show("Користувача не знайдено.");
                    BtnRequestAccess.IsEnabled = false;
                    return;
                }

                if (user.AccessRight != AccessRight.Guest)
                {
                    MessageBox.Show("Вам уже надано розширений доступ.");
                    BtnRequestAccess.IsEnabled = false;
                    return;
                }

                var existingRequest = _authorizationRequestService.GetRequestByLogin(_currentUserLogin);
                if (existingRequest != null)
                {
                    MessageBox.Show(existingRequest.Status == RequestStatus.Pending
                        ? "Запит уже подано. Очікуйте рішення."
                        : "Запит уже розглянуто.");
                    UpdateRequestButtonState();
                    return;
                }

                _authorizationRequestService.CreateRequest(_currentUserLogin);
                MessageBox.Show("Запит успішно подано. Очікуйте підтвердження.");
                UpdateRequestButtonState();
            }
            catch (RequestAlreadyExistException ex)
            {
                MessageBox.Show(ex.Message);
                UpdateRequestButtonState();
            }
            catch (UserNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
                UpdateRequestButtonState();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Не вдалося подати запит: {ex.Message}");
            }
        }

        private void UpdateRequestButtonState()
        {
            try
            {
                var user = _userService.LoadByUsername(_currentUserLogin);
                if (user == null)
                {
                    BtnRequestAccess.IsEnabled = false;
                    return;
                }

                if (user.AccessRight != AccessRight.Guest)
                {
                    BtnRequestAccess.IsEnabled = false;
                    return;
                }

                var existingRequest = _authorizationRequestService.GetRequestByLogin(_currentUserLogin);
                BtnRequestAccess.IsEnabled = existingRequest == null;
            }
            catch (System.Exception)
            {
                BtnRequestAccess.IsEnabled = false;
            }
        }
    }
}
