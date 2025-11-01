using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CarDealership.config;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.page.query;

public partial class MinStockCarsPage
{
    private readonly DealershipContext _context;

    public class MinStockItem
    {
        public string Title { get; set; } = string.Empty;
        public string ProductNumber { get; set; } = string.Empty;
        public int Amount { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
    }

    public MinStockCarsPage()
    {
        InitializeComponent();
        _context = new DealershipContext();
        LoadModels();
        RunQuery();
    }

    private void LoadModels()
    {
        try
        {
            var models = _context.Products
                .Include(p => p.Car)
                .Where(p => p.Car != null && p.Amount == 1)
                .Select(p => new { p.Car!.Brand, p.Car!.ModelName })
                .AsEnumerable()
                .Select(c => ($"{c.Brand} {c.ModelName}").Trim())
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            models.Insert(0, "Всі");
            ModelPicker.ItemsSource = models;
            ModelPicker.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка завантаження моделей: {ex.Message}");
        }
    }

    private void BtnRun_Click(object sender, RoutedEventArgs e)
    {
        RunQuery();
    }

    private void RunQuery()
    {
        try
        {
            var selected = ModelPicker.SelectedItem as string;

            var q = _context.Products
                .Include(p => p.Car)
                .Where(p => p.Car != null)
                .Where(p => p.Amount == 1);

            if (!string.IsNullOrWhiteSpace(selected) && !string.Equals(selected, "Всі", StringComparison.OrdinalIgnoreCase))
            {
                q = q.Where(p => (p.Car!.Brand + " " + p.Car!.ModelName) == selected);
            }

            var list = q
                .Select(p => new MinStockItem
                {
                    Title = p.Car!.Brand + " " + p.Car!.ModelName,
                    ProductNumber = p.Number,
                    Amount = p.Amount,
                    AvailableFrom = p.AvailableFrom,
                    Year = p.Car!.Year,
                    Price = p.Car!.Price
                })
                .OrderBy(x => x.Title)
                .ToList();

            ResultsItems.ItemsSource = new ObservableCollection<MinStockItem>(list);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка запиту: {ex.Message}");
        }
    }
}
