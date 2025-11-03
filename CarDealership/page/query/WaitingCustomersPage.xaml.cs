using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarDealership.config;
using CarDealership.enums;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.page.query;

public partial class WaitingCustomersPage : Page
{
    private readonly DealershipContext _context;

    public class WaitingRow
    {
        public int OrderId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string ProductNumber { get; set; } = string.Empty;
        public string CarTitle { get; set; } = string.Empty;
        public string OrderDate { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public int OrdersCount { get; set; }
    }

    public WaitingCustomersPage()
    {
        InitializeComponent();
        _context = new DealershipContext();
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            var baseQuery = _context.Orders
                .Include(o => o.Client).ThenInclude(c => c.PassportData)
                .Include(o => o.Product).ThenInclude(p => p.Car)
                .Where(o => !o.Product.InStock || o.Product.Amount == 0)
                // exclude explicitly rejected orders
                .Where(o => !_context.OrderReviews.Any(r => r.OrderId == o.Id && r.Status == RequestStatus.Rejected));

            var list = baseQuery
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            // Show one row per client (latest order), also compute their waiting orders count
            var grouped = list
                .GroupBy(o => o.ClientId)
                .Select(g => new { Latest = g.OrderByDescending(x => x.OrderDate).First(), Count = g.Count() })
                .ToList();

            var rows = grouped.Select(x => new WaitingRow
            {
                OrderId = x.Latest.Id,
                ClientId = x.Latest.ClientId,
                ClientName = (x.Latest.Client?.PassportData != null)
                    ? ($"{x.Latest.Client.PassportData.FirstName} {x.Latest.Client.PassportData.LastName}")
                    : (x.Latest.Client?.User?.Login ?? "Unknown"),
                Phone = x.Latest.PhoneNumber,
                ProductNumber = x.Latest.Product?.Number ?? string.Empty,
                CarTitle = (x.Latest.Product?.Car != null) ? ($"{x.Latest.Product.Car.Brand} {x.Latest.Product.Car.ModelName}") : string.Empty,
                OrderDate = x.Latest.OrderDate.ToString("yyyy-MM-dd HH:mm"),
                OrdersCount = x.Count
            }).ToList();

            Results.ItemsSource = new ObservableCollection<WaitingRow>(rows);
            var totalClients = rows.Count; // already unique by client
            TotalCountText.Text = totalClients.ToString();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка завантаження: {ex.Message}");
        }
    }
}
