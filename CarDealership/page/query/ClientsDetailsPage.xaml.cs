using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CarDealership.config;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.page.query;

public partial class ClientsDetailsPage
{
    private readonly DealershipContext _context;

    public class ClientRow
    {
        public int ClientId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PassportNumber { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AccessRight { get; set; } = string.Empty;
        public int OrdersCount { get; set; }
        public DateTime? LastPaymentAt { get; set; }
    }

    public ClientsDetailsPage()
    {
        InitializeComponent();
        _context = new DealershipContext();
        LoadBrands();
        LoadBrandSummary();
    }

    private void LoadBrands()
    {
        try
        {
            var brands = _context.Cars
                .Select(c => c.Brand)
                .Distinct()
                .OrderBy(b => b)
                .ToList();
            // Add "All" option to fetch every client with any payment
            brands.Insert(0, "Всі");
            BrandPicker.ItemsSource = brands;
            if (brands.Count > 0) BrandPicker.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка завантаження марок: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public class BrandClientsRow
    {
        public string Brand { get; set; } = string.Empty;
        public int ClientsCount { get; set; }
        public int OrdersCount { get; set; }
    }

    private void LoadBrandSummary()
    {
        try
        {
            var rows = _context.PaymentHistory
                .Include(ph => ph.Order)
                    .ThenInclude(o => o.Product)
                        .ThenInclude(p => p.Car)
                .Include(ph => ph.Order)
                    .ThenInclude(o => o.Client)
                .Where(ph => ph.Order != null && ph.Order.Product != null && ph.Order.Product.Car != null)
                .AsEnumerable()
                .GroupBy(ph => ph.Order!.Product!.Car!.Brand)
                .Select(g => new BrandClientsRow
                {
                    Brand = g.Key,
                    ClientsCount = g.Select(x => x.Order!.ClientId).Distinct().Count(),
                    OrdersCount = g.Select(x => x.OrderId).Distinct().Count()
                })
                .OrderByDescending(r => r.OrdersCount)
                .ToList();

            BrandClientsList.ItemsSource = new ObservableCollection<BrandClientsRow>(rows);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка завантаження зведення: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RunBtn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var brand = BrandPicker.SelectedItem as string;
        if (string.IsNullOrWhiteSpace(brand))
        {
            MessageBox.Show("Оберіть марку.", "Увага", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        try
        {
            var query = _context.PaymentHistory
                .Include(ph => ph.Order)
                    .ThenInclude(o => o.Product)
                        .ThenInclude(p => p.Car)
                .Include(ph => ph.Order)
                    .ThenInclude(o => o.Client)
                        .ThenInclude(c => c.PassportData)
                .Include(ph => ph.Order)
                    .ThenInclude(o => o.Client)
                        .ThenInclude(c => c.User)
                .Where(ph => ph.Order != null && ph.Order.Product != null && ph.Order.Product.Car != null);

            if (brand != "Всі")
            {
                query = query.Where(ph => ph.Order!.Product!.Car!.Brand == brand);
            }

            var items = query
                .AsEnumerable()
                .Where(ph => ph.Order?.Client != null && ph.Order.Client.PassportData != null && ph.Order.Client.User != null)
                .GroupBy(ph => ph.Order!.Client!.Id)
                .Select(g => new ClientRow
                {
                    ClientId = g.Key,
                    FullName = $"{g.First().Order!.Client!.PassportData!.FirstName} {g.First().Order!.Client!.PassportData!.LastName}",
                    PassportNumber = g.First().Order!.Client!.PassportData!.PassportNumber,
                    Login = g.First().Order!.Client!.User!.Login,
                    Email = g.First().Order!.Client!.User!.Email,
                    AccessRight = g.First().Order!.Client!.User!.AccessRightString,
                    OrdersCount = g.Select(x => x.OrderId).Distinct().Count(),
                    LastPaymentAt = g.Max(x => (DateTime?)x.CreatedAt)
                })
                .OrderBy(r => r.FullName)
                .ToList();

            ClientsList.ItemsSource = new ObservableCollection<ClientRow>(items);

            // refresh summary too
            LoadBrandSummary();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка пошуку клієнтів: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
