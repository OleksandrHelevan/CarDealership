using System.Windows;
using System.Windows.Controls;
using CarDealership.config;
using CarDealership.dto;
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
            // 1. Знайти ключ (user) за логіном
            var user = _context.Users.FirstOrDefault(u => u.Login == _userLogin);
            if (user == null) return;

            // 2. Знайти клієнта за user_id
            var client = _context.Clients.FirstOrDefault(c => c.UserId == user.Id);
            if (client == null) return;

            // 3. Знайти всі замовлення для цього client_id
            var ordersList = _context.Orders
                .Where(o => o.ClientId == client.Id)
                .Include(o => o.Product)
                .ThenInclude(p => p.ElectroCar)
                .Include(o => o.Product)
                .ThenInclude(p => p.GasolineCar)
                .Select(o => new OrderDto()
                {
                    Id = o.Id,
                    OrderId = o.Id,
                    CarName = o.Product.ElectroCar != null 
                        ? $"{o.Product.ElectroCar.Brand} {o.Product.ElectroCar.ModelName}" 
                        : o.Product.GasolineCar != null 
                            ? $"{o.Product.GasolineCar.Brand} {o.Product.GasolineCar.ModelName}" 
                            : "Невідомо",
                    PaymentType = o.PaymentType,
                    Delivery = o.Delivery,
                    OrderDate = o.OrderDate,
                    CreatedAt = o.OrderDate
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
                        MessageBox.Show("Замовлення відмінено", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadOrders(); // Оновлюємо список після видалення
                    }
                }
            }
        }

    }
}
