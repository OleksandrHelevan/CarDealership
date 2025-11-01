using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarDealership.config;
using CarDealership.enums;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.page.query;

public partial class UsersAndCarsPaymentTypePage : Page
{
    private readonly DealershipContext _context;

    public class ClientRow
    {
        public int ClientId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AccessRight { get; set; } = string.Empty;
        public int OrdersCount { get; set; }
    }

    public class CarRow
    {
        public int CarId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int OrdersCount { get; set; }
        public string CarName => $"{Brand} {ModelName}";
    }

    private record PaymentTypeItem(string Label, PaymentType Value);

    public UsersAndCarsPaymentTypePage()
    {
        InitializeComponent();
        _context = new DealershipContext();
        LoadPickers();
        RunQuery();
    }

    private void LoadPickers()
    {
        try
        {
            // Localized payment types
            PaymentTypePicker.ItemsSource = new[]
            {
                new PaymentTypeItem("Готівка", PaymentType.Cash),
                new PaymentTypeItem("Кредит", PaymentType.Credit),
                new PaymentTypeItem("Картка", PaymentType.Card)
            };
            PaymentTypePicker.DisplayMemberPath = "Label";
            PaymentTypePicker.SelectedValuePath = "Value";
            PaymentTypePicker.SelectedIndex = 0;

            ShowPicker.ItemsSource = new[] { "Клієнти", "Авто" };
            ShowPicker.SelectedIndex = 0;

            ShowPicker.SelectionChanged += (_, __) => UpdateVisibility();
            UpdateVisibility();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка ініціалізації: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void UpdateVisibility()
    {
        var show = ShowPicker.SelectedItem as string;
        var showClients = string.Equals(show, "Клієнти", StringComparison.OrdinalIgnoreCase);
        ClientsBlock.Visibility = showClients ? Visibility.Visible : Visibility.Collapsed;
        CarsBlock.Visibility = showClients ? Visibility.Collapsed : Visibility.Visible;
    }

    private void RunBtn_Click(object sender, RoutedEventArgs e)
    {
        RunQuery();
    }

    private void RunQuery()
    {
        try
        {
            var selected = PaymentTypePicker.SelectedValue;
            if (selected is not PaymentType paymentType)
            {
                ClientsList.ItemsSource = null;
                CarsList.ItemsSource = null;
                return;
            }

            // Clients having orders with specified payment type
            var clientQuery = _context.Orders
                .Where(o => o.PaymentType == paymentType)
                .Include(o => o.Client)
                    .ThenInclude(c => c.PassportData)
                .Include(o => o.Client)
                    .ThenInclude(c => c.User)
                .AsEnumerable()
                .Where(o => o.Client != null && o.Client.User != null && o.Client.PassportData != null)
                .GroupBy(o => o.Client!.Id)
                .Select(g => new ClientRow
                {
                    ClientId = g.Key,
                    FullName = $"{g.First().Client!.PassportData!.FirstName} {g.First().Client!.PassportData!.LastName}",
                    Login = g.First().Client!.User!.Login,
                    Email = g.First().Client!.User!.Email,
                    AccessRight = g.First().Client!.User!.AccessRightString,
                    OrdersCount = g.Count()
                })
                .OrderBy(r => r.FullName)
                .ToList();

            ClientsList.ItemsSource = new ObservableCollection<ClientRow>(clientQuery);

            // Cars having orders with specified payment type
            var carsQuery = _context.Orders
                .Where(o => o.PaymentType == paymentType)
                .Include(o => o.Product)
                    .ThenInclude(p => p.Car)
                .AsEnumerable()
                .Where(o => o.Product != null && o.Product.Car != null)
                .GroupBy(o => o.Product!.Car!.Id)
                .Select(g => new CarRow
                {
                    CarId = g.Key,
                    Brand = g.First().Product!.Car!.Brand,
                    ModelName = g.First().Product!.Car!.ModelName,
                    Price = g.First().Product!.Car!.Price,
                    OrdersCount = g.Count()
                })
                .OrderBy(r => r.Brand)
                .ThenBy(r => r.ModelName)
                .ToList();

            CarsList.ItemsSource = new ObservableCollection<CarRow>(carsQuery);

            UpdateVisibility();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка виконання запиту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
