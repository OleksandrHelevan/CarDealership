using System.Windows;
using System.Windows.Controls;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.window;
using Microsoft.EntityFrameworkCore;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;


namespace CarDealership.page.authorized
{
    public partial class MyOrdersPage : Page
    {
        private readonly DealershipContext _context;
        private readonly string _userLogin;

        public MyOrdersPage(string userLogin)
        {
            InitializeComponent();
            _context = new DealershipContext();
            _userLogin = userLogin;

            LoadOrders();
        }

        private void LoadOrders()
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == _userLogin);
            if (user == null) return;

            var client = _context.Clients.FirstOrDefault(c => c.UserId == user.Id);
            if (client == null) return;

            var ordersList = _context.Orders
                .Where(o => o.ClientId == client.Id)
                .Include(o => o.Product)
                .ThenInclude(p => p.Car)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderId = o.Id,
                    CarName = o.Product.Car != null
                        ? $"{o.Product.Car.Brand} {o.Product.Car.ModelName}"
                        : "Невідомо",
                    PaymentType = o.PaymentType,
                    DeliveryRequired = o.DeliveryRequired,
                    OrderDate = o.OrderDate,
                    CreatedAt = o.OrderDate,
                    
                })
                .ToList();

            OrdersList.ItemsSource = ordersList;
        }

        private void CancelOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int orderId)
            {
                var result = MessageBox.Show(
                    "Ви дійсно хочете відмінити це замовлення?",
                    "Підтвердження",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
                    if (order != null)
                    {
                        _context.Orders.Remove(order);
                        _context.SaveChanges();
                        MessageBox.Show("Замовлення відмінено", "Успіх", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        LoadOrders();
                    }
                }
            }
        }
        private void UpdateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int orderId)
            {
                var order = _context.Orders
                    .Include(o => o.Product)
                    .FirstOrDefault(o => o.Id == orderId);

                if (order?.Product != null)
                {
                    var updateWindow = new UpdateOrderDialog(order.Product.Id);
                    updateWindow.ShowDialog();

                    LoadOrders();
                }
            }
        }
    }
   
}
