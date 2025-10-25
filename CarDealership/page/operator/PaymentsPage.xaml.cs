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

namespace CarDealership.page.@operator;

public partial class PaymentsPage : Page
{
    private readonly DealershipContext _context;
    private readonly PaymentHistoryRepositoryImpl _repo;

    public class Row
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ClientName { get; set; }
        public string ProductNumber { get; set; }
        public string CarName { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public PaymentsPage()
    {
        InitializeComponent();
        _context = new DealershipContext();
        _repo = new PaymentHistoryRepositoryImpl(_context);
        LoadData();
    }

    private void LoadData()
    {
        var list = _repo.GetAll()
            .Select(ph => new Row
            {
                Id = ph.Id,
                OrderId = ph.OrderId,
                ClientName = ph.Order?.Client?.PassportData != null
                    ? $"{ph.Order.Client.PassportData.FirstName} {ph.Order.Client.PassportData.LastName}"
                    : string.Empty,
                ProductNumber = ph.Order?.Product?.Number ?? string.Empty,
                CarName = ph.Order?.Product?.Car != null
                    ? $"{ph.Order.Product.Car.Brand} {ph.Order.Product.Car.ModelName}"
                    : string.Empty,
                Amount = ph.Amount,
                CreatedAt = ph.CreatedAt
            })
            .OrderByDescending(r => r.Id)
            .ToList();

        PaymentsList.ItemsSource = new ObservableCollection<Row>(list);
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is int id)
        {
            var ph = _repo.GetById(id);
            if (ph?.ReceiptPdf == null || ph.ReceiptPdf.Length == 0)
            {
                MessageBox.Show("Квитанцію не знайдено.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var tmp = Path.Combine(Path.GetTempPath(), $"receipt-{id}-{Guid.NewGuid():N}.pdf");
            File.WriteAllBytes(tmp, ph.ReceiptPdf);
            Process.Start(new ProcessStartInfo(tmp) { UseShellExecute = true });
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is int id)
        {
            var ph = _repo.GetById(id);
            if (ph?.ReceiptPdf == null || ph.ReceiptPdf.Length == 0)
            {
                MessageBox.Show("Квитанцію не знайдено.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var dlg = new SaveFileDialog { FileName = $"receipt-{id}.pdf", Filter = "PDF (*.pdf)|*.pdf" };
            if (dlg.ShowDialog() == true)
            {
                File.WriteAllBytes(dlg.FileName, ph.ReceiptPdf);
                MessageBox.Show("Збережено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

