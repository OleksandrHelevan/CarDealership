using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.repo.impl;
using CarDealership.service;
using CarDealership.service.impl;

namespace CarDealership.page.admin
{
    public partial class UserRequestsPage : Page
    {
        private readonly IAuthorizationRequestService _requestService;
        private readonly IUserService _userService;
        private readonly ObservableCollection<AuthorizationRequest> _allRequests = new();
        private readonly ObservableCollection<AuthorizationRequest> _filteredRequests = new();

        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        public UserRequestsPage()
        {
            InitializeComponent();

            _requestService = new AuthorizationRequestService(new AuthorizationRequestRepository(new DealershipContext()));
            _userService = new UserServiceImpl();

            ApproveCommand = new RelayCommand<AuthorizationRequest>(ApproveRequest);
            RejectCommand = new RelayCommand<AuthorizationRequest>(RejectRequest);

            RequestsList.ItemsSource = _filteredRequests;
            LoadRequests();

            DataContext = this;
        }

        private void LoadRequests()
        {
            _allRequests.Clear();
            foreach (var request in _requestService.GetAllRequests().Where(r => r.Status == RequestStatus.Pending))
            {
                _allRequests.Add(request);
            }

            ApplySearch(LoginSearchBox?.Text);
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
                _allRequests.Remove(request);
                _filteredRequests.Remove(request);
            }
        }

        private void RejectRequest(AuthorizationRequest request)
        {
            if (request == null) return;

            request.Status = RequestStatus.Rejected;

            try
            {
                if (!_requestService.UpdateRequest(request))
                {
                    MessageBox.Show("Не вдалося.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відхилення запиту: {ex.Message}");
            }
            finally
            {
                _allRequests.Remove(request);
                _filteredRequests.Remove(request);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ApplySearch(LoginSearchBox?.Text);
        }

        private void ApplySearch(string? query)
        {
            _filteredRequests.Clear();

            IEnumerable<AuthorizationRequest> filtered = _allRequests;
            if (!string.IsNullOrWhiteSpace(query))
            {
                filtered = filtered.Where(r =>
                    r.User?.Login != null &&
                    r.User.Login.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            foreach (var request in filtered)
            {
                _filteredRequests.Add(request);
            }
        }
    }
}
