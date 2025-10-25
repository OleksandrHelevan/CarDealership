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

    private void LoadOrders()
    {
        var orders = _context.Orders
            // show only orders that have no review created yet
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
                // ensure review exists
                var review = _reviewRepo.GetByOrderId(orderId);
                if (review == null)
                {
                    review = new OrderReview { OrderId = orderId, Status = enums.RequestStatus.Pending, RequiresDeliveryAddress = order.Delivery, RequiresCardNumber = order.PaymentType == PaymentType.Card };
                    _reviewRepo.Add(review);
                }

                if (dlg.IsApproved)
                {
                    _reviewService.Approve(review.Id, null);
                    if (order.PaymentType == PaymentType.Card && !string.IsNullOrWhiteSpace(dlg.CardNumber))
                    {
                        _reviewService.SubmitDetails(review.Id, null, dlg.CardNumber);
                    }
                }
                else
                {
                    _reviewService.Reject(review.Id, dlg.Reason!);
                }

                LoadOrders();
                MessageBox.Show("Рішення збережено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
