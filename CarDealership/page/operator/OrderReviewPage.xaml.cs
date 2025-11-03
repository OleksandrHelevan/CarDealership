using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarDealership.config;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.repo.impl;
using CarDealership.service.impl;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.page.@operator;

public partial class OrderReviewPage : Page
{
    private readonly DealershipContext _context;

    private readonly OrderReviewServiceImpl _reviewService;
    private readonly OrderRepositoryImpl _orderRepo;
    private readonly OrderReviewRepositoryImpl _reviewRepo;
    private readonly string? _operatorLogin;

    public class OrderRow
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ProductNumber { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool Delivery { get; set; }
        public string? Address { get; set; }
    }

    public OrderReviewPage()
    {
        InitializeComponent();
        _context = new DealershipContext();
        _orderRepo = new OrderRepositoryImpl(_context);
        _reviewRepo = new OrderReviewRepositoryImpl(_context);
        _reviewService = new OrderReviewServiceImpl(_orderRepo, _reviewRepo);
        LoadOrders();
    }

    public OrderReviewPage(string operatorLogin) : this()
    {
        _operatorLogin = operatorLogin;
    }

    private void LoadOrders()
    {
        var orders = _context.Orders
            .Where(o => !_context.OrderReviews.Any(r => r.OrderId == o.Id))
            .Include(o => o.Client)
                .ThenInclude(c => c.PassportData)
            .Include(o => o.Product)
            .Select(o => new OrderRow
            {
                Id = o.Id,
                ClientName = (o.Client != null && o.Client.PassportData != null)
                    ? (o.Client.PassportData.FirstName + " " + o.Client.PassportData.LastName)
                    : string.Empty,
                ProductNumber = o.Product != null ? o.Product.Number : string.Empty,
                PaymentType = o.PaymentType,
                Delivery = o.Delivery,
                Address = o.Address
            })
            .OrderByDescending(r => r.Id)
            .ToList();

        OrdersList.ItemsSource = new ObservableCollection<OrderRow>(orders);
    }

    private void Review_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return;

            var dlg = new OrderDecisionDialog(order.PaymentType) { Owner = Window.GetWindow(this) };
            if (dlg.ShowDialog() == true)
            {
                var review = _reviewRepo.GetByOrderId(orderId);
                if (review == null)
                {
                    review = new OrderReview { OrderId = orderId, Status = RequestStatus.Pending, RequiresDeliveryAddress = order.Delivery, RequiresCardNumber = order.PaymentType == PaymentType.Card };
                    _reviewRepo.Add(review);
                }

                if (dlg.IsApproved)
                {
                    _reviewService.Approve(review.Id, null);

                    if (!string.IsNullOrEmpty(_operatorLogin))
                    {
                        var user = _context.Users.FirstOrDefault(u => u.Login == _operatorLogin);
                        if (user != null)
                        {
                            review.ApprovedByUserId = user.Id;
                            _reviewRepo.Update(review);
                        }
                    }

                    var deliveryAddress = order.Delivery ? order.Address : null;
                    var cardNumber = order.PaymentType == PaymentType.Card ? dlg.CardNumber : null;
                    if (order.Delivery || order.PaymentType == PaymentType.Card)
                    {
                        try
                        {
                            _reviewService.SubmitDetails(review.Id, deliveryAddress, cardNumber);
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show($"Помилка збереження деталей: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
                else
                {
                    _reviewService.Reject(review.Id, dlg.Reason!);
                }

                LoadOrders();
                MessageBox.Show("Рішення збережено.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
