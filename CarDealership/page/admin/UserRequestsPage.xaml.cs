using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.service;
using CarDealership.service.impl;
using CarDealership.repo.impl;

namespace CarDealership.page.admin;

public partial class UserRequestsPage : Page
{
    private readonly IAuthorizationRequestService _requestService;
    private readonly IUserService _userService;
    private ObservableCollection<AuthorizationRequest> requests;

    public ICommand ApproveCommand { get; }
    public ICommand RejectCommand { get; }

    public UserRequestsPage()
    {
        InitializeComponent();

        _requestService = new AuthorizationRequestService(new AuthorizationRequestRepository(new DealershipContext()));
        _userService = new UserService();

        ApproveCommand = new RelayCommand<AuthorizationRequest>(ApproveRequest);
        RejectCommand = new RelayCommand<AuthorizationRequest>(RejectRequest);

        LoadRequests();

        DataContext = this;
    }

    private void LoadRequests()
    {
        requests = new ObservableCollection<AuthorizationRequest>(_requestService.GetAllRequests());
        RequestsDataGrid.ItemsSource = requests;
    }

    private void ApproveRequest(AuthorizationRequest request)
    {
        if (request == null) return;

        request.Status = RequestStatus.Approved;

        try
        {
            if (_requestService.UpdateRequest(request))
            {
                var user = _userService.LoadByUsername(request.Login);
                if (user != null)
                {
                    user.AccessRight = AccessRight.Authorized;
                    _userService.Update(user);
                }

                MessageBox.Show($"Запит {request.Login} підтверджено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Помилка при підтвердженні запиту {request.Login}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (System.Exception ex)
        {
            MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            requests.Remove(request); // автоматично видаляємо з ObservableCollection
        }
    }

    private void RejectRequest(AuthorizationRequest request)
    {
        if (request == null) return;

        request.Status = RequestStatus.Rejected;

        try
        {
            if (_requestService.UpdateRequest(request))
            {
                MessageBox.Show($"Запит {request.Login} відхилено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Помилка при відмові запиту {request.Login}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (System.Exception ex)
        {
            MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            requests.Remove(request); // автоматично видаляємо з ObservableCollection
        }
    }

    private void RefreshRequests_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var requestsToDelete = _requestService.GetAllRequests()
                                    .Where(r => r.Status == RequestStatus.Approved || r.Status == RequestStatus.Rejected)
                                    .ToList();

            foreach (var req in requestsToDelete)
            {
                _requestService.DeleteRequest(req.Id); 
            }

            LoadRequests();

            MessageBox.Show("Старі запити видалено та список оновлено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (System.Exception ex)
        {
            MessageBox.Show($"Помилка при оновленні: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
