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
        _userService = new UserServiceImpl();

        ApproveCommand = new RelayCommand<AuthorizationRequest>(ApproveRequest);
        RejectCommand = new RelayCommand<AuthorizationRequest>(RejectRequest);

        LoadRequests();

        DataContext = this;
    }

    private void LoadRequests()
    {
        requests = new ObservableCollection<AuthorizationRequest>(_requestService.GetAllRequests());
        RequestsList.ItemsSource = requests;
    }

    private void ApproveRequest(AuthorizationRequest request)
    {
        if (request == null) return;

        request.Status = RequestStatus.Approved;

        try
        {
            if (_requestService.UpdateRequest(request))
            {
                var user = _userService.LoadByUsername(request.User.Login);
                if (user != null)
                {
                    user.AccessRight = AccessRight.Authorized;
                    _userService.Update(user);
                }
            }
        }
        finally
        {
            requests.Remove(request);
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
            }
            else
            {
            }
        }
        catch (System.Exception ex)
        {
        }
        finally
        {
            requests.Remove(request);
        }
    }
}