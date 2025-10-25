using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarDealership.config;
using CarDealership.repo.impl;
using Microsoft.Win32;

namespace CarDealership.page.authorized;

public partial class ReceiptsPage : Page
{
    private readonly DealershipContext _context;
    private readonly string _userLogin;
    private readonly PaymentHistoryRepositoryImpl _repo;

    public class Row
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string CarName { get; set; }
        public string ProductNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public ReceiptsPage(string userLogin)
    {
        InitializeComponent();
        _userLogin = userLogin;
        _context = new DealershipContext();
        _repo = new PaymentHistoryRepositoryImpl(_context);
        LoadData();
    }

    private void LoadData()
    {
        var user = _context.Users.FirstOrDefault(u => u.Login == _userLogin);
        if (user == null) return;
        var client = _context.Clients.FirstOrDefault(c => c.UserId == user.Id);
        if (client == null) return;

        var list = _repo.GetByClientId(client.Id)
            .Join(_context.Products, ph => ph.Order.ProductId, p => p.Id, (ph, p) => new { ph, p })
            .Join(_context.Cars, php => php.p.CarId, c => c.Id, (php, c) => new Row
            {
                Id = php.ph.Id,
                OrderId = php.ph.OrderId,
                CarName = c.Brand + " " + c.ModelName,
                ProductNumber = php.p.Number,
                Amount = php.ph.Amount,
                CreatedAt = php.ph.CreatedAt
            })
            .OrderByDescending(r => r.Id)
            .ToList();

        ReceiptsList.ItemsSource = new ObservableCollection<Row>(list);
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is int id)
        {
            var ph = _repo.GetById(id);
            if (ph == null || ph.ReceiptPdf == null || ph.ReceiptPdf.Length == 0)
            {
                MessageBox.Show("Квитанцію не знайдено.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var tmpPath = Path.Combine(Path.GetTempPath(), $"receipt-{ph.Id}.pdf");
            File.WriteAllBytes(tmpPath, ph.ReceiptPdf);

            var psi = new ProcessStartInfo(tmpPath)
            {
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is int id)
        {
            var ph = _repo.GetById(id);
            if (ph == null || ph.ReceiptPdf == null || ph.ReceiptPdf.Length == 0)
            {
                MessageBox.Show("Квитанцію не знайдено.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var dlg = new SaveFileDialog
            {
                FileName = $"receipt-{ph.Id}.pdf",
                Filter = "PDF (*.pdf)|*.pdf"
            };
            if (dlg.ShowDialog() == true)
            {
                File.WriteAllBytes(dlg.FileName, ph.ReceiptPdf);
                MessageBox.Show("Збережено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

