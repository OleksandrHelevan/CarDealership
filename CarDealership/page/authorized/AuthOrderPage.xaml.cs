using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarDealership.config;
using CarDealership.enums;
using CarDealership.repo.impl;
using CarDealership.service.impl;

namespace CarDealership.page.authorized;

public partial class AuthOrderPage : Page
{
    private readonly DealershipContext _context;
    private readonly string _userLogin;
    private readonly PaymentHistoryServiceImpl _paymentService;
    private readonly PaymentHistoryRepositoryImpl _paymentRepo;

    public class Row
    {
        public int Id { get; set; }
        public string CarName { get; set; }
        public string ProductNumber { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool Delivery { get; set; }
        public int LastReceiptId { get; set; }
        public bool HasReceipt { get; set; }
        public string HasReceiptText => HasReceipt ? "Так" : "Ні";
        public CarDealership.enums.RequestStatus? ReviewStatus { get; set; }
        public string? ReviewMessage { get; set; }
    }

    public AuthOrderPage(string userLogin)
    {
        InitializeComponent();
        _userLogin = userLogin;
        _context = new DealershipContext();
        _paymentRepo = new PaymentHistoryRepositoryImpl(_context);
        _paymentService = new PaymentHistoryServiceImpl(_context, _paymentRepo);
        LoadOrders();
    }

    private void LoadOrders()
    {
        var user = _context.Users.FirstOrDefault(u => u.Login == _userLogin);
        if (user == null) return;
        var client = _context.Clients.FirstOrDefault(c => c.UserId == user.Id);
        if (client == null) return;

        // Base the list on the latest OrderReview per order (for this client)
        var list = (from r in _context.OrderReviews
                    join o in _context.Orders on r.OrderId equals o.Id
                    join p in _context.Products on o.ProductId equals p.Id
                    join c in _context.Cars on p.CarId equals c.Id
                    where o.ClientId == client.Id
                    // keep only latest review per order
                    where !_context.OrderReviews.Any(r2 => r2.OrderId == r.OrderId && r2.Id > r.Id)
                    select new Row
                    {
                        Id = o.Id,
                        CarName = c.Brand + " " + c.ModelName,
                        ProductNumber = p.Number,
                        Amount = c.Price,
                        PaymentType = o.PaymentType,
                        Delivery = o.Delivery,
                        LastReceiptId = _context.PaymentHistory
                            .Where(ph => ph.OrderId == o.Id)
                            .OrderByDescending(ph => ph.Id)
                            .Select(ph => ph.Id)
                            .FirstOrDefault(),
                        HasReceipt = _context.PaymentHistory.Any(ph => ph.OrderId == o.Id),
                        ReviewStatus = r.Status,
                        ReviewMessage = r.Message
                    })
            .OrderByDescending(r => r.Id)
            .ToList();

        OrdersList.ItemsSource = new ObservableCollection<Row>(list);
    }

    private void Receipt_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is int orderId)
        {
            var dlg = new EnterCardDialog { Owner = Window.GetWindow(this) };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var id = _paymentService.CreateReceipt(orderId, dlg.CardNumber!);
                    // Refresh list so visibility/status updates immediately
                    LoadOrders();
                    MessageBox.Show("Квитанцію збережено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    private void Reason_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string msg && !string.IsNullOrWhiteSpace(msg))
        {
            MessageBox.Show(msg, "Причина відхилення", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void View_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is int receiptId && receiptId > 0)
        {
            var page = new ReceiptViewerPage(receiptId);
            NavigationService?.Navigate(page);
        }
        else
        {
            MessageBox.Show("Квитанцію не знайдено для цього замовлення.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
