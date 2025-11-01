using System.Collections.ObjectModel;
using System.Windows;
using CarDealership.config;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.page.query;

public partial class BrandsDelayPage
{
    private readonly DealershipContext _context;

    public class BrandDelayRow
    {
        public string Brand { get; set; } = string.Empty;
        public int Count { get; set; }
        public DateTime? OldestFrom { get; set; }
    }

    public class DelayedCard
    {
        public string Title { get; set; } = string.Empty;
        public string ProductNumber { get; set; } = string.Empty;
        public string CountryOfOrigin { get; set; } = string.Empty;
        public DateTime? AvailableFrom { get; set; }
        public string InStockText { get; set; } = string.Empty;
    }

    public BrandsDelayPage()
    {
        InitializeComponent();
        _context = new DealershipContext();
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            var now = DateTime.UtcNow;

            var delayed = _context.Products
                .Include(p => p.Car)
                .Where(p => p.InStock == false && p.AvailableFrom != null && p.AvailableFrom < now)
                .ToList();

            var brandGroups = delayed
                .GroupBy(p => p.Car.Brand)
                .Select(g => new BrandDelayRow
                {
                    Brand = g.Key,
                    Count = g.Count(),
                    OldestFrom = g.Min(x => x.AvailableFrom)
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            BrandDelaysList.ItemsSource = new ObservableCollection<BrandDelayRow>(brandGroups);

            var cards = delayed
                .Select(p => new DelayedCard
                {
                    Title = p.Car.Brand + " " + p.Car.ModelName,
                    ProductNumber = p.Number,
                    CountryOfOrigin = p.CountryOfOrigin,
                    AvailableFrom = p.AvailableFrom,
                    InStockText = "В наявності: ні"
                })
                .OrderBy(c => c.AvailableFrom)
                .ToList();

            DelayedItems.ItemsSource = new ObservableCollection<DelayedCard>(cards);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка завантаження даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
