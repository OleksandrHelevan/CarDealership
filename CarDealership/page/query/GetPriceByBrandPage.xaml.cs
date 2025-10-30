using System.Collections.ObjectModel;
using System.Windows;
using CarDealership.config;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.page.query;

public partial class GetPriceByBrandPage
{
    private readonly DealershipContext _context;

    public class SoldModelCard
    {
        public string Title { get; set; } = string.Empty;
        public string ProductNumber { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class BrandSumRow
    {
        public string Brand { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Sum { get; set; }
    }

    public GetPriceByBrandPage()
    {
        InitializeComponent();
        _context = new DealershipContext();
        LoadBrands();
        LoadBrandSums();
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
            BrandPicker.ItemsSource = brands;
            if (brands.Count > 0)
                BrandPicker.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка завантаження марок: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void LoadBrandSums()
    {
        try
        {
            var grouped = _context.PaymentHistory
                .Include(ph => ph.Order)
                    .ThenInclude(o => o.Product)
                        .ThenInclude(p => p.Car)
                .Where(ph => ph.Order != null && ph.Order.Product != null && ph.Order.Product.Car != null)
                .AsEnumerable()
                .GroupBy(ph => ph.Order!.Product!.Car!.Brand)
                .Select(g => new BrandSumRow
                {
                    Brand = g.Key,
                    Count = g.Count(),
                    Sum = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.Sum)
                .ToList();

            BrandSumsList.ItemsSource = new ObservableCollection<BrandSumRow>(grouped);
            var total = grouped.Sum(x => x.Sum);
            TotalSumText.Text = $"Загальна сума: ${total}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка завантаження сум: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BtnQuery1_Click(object sender, RoutedEventArgs e)
    {
        var brand = BrandPicker.SelectedItem as string;
        if (string.IsNullOrWhiteSpace(brand))
        {
            MessageBox.Show("Оберіть марку.", "Увага", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        try
        {
            var list = _context.PaymentHistory
                .Include(ph => ph.Order)
                    .ThenInclude(o => o.Product)
                        .ThenInclude(p => p.Car)
                .Where(ph => ph.Order != null && ph.Order.Product != null && ph.Order.Product.Car != null)
                .Where(ph => ph.Order!.Product!.Car!.Brand == brand)
                .Select(ph => new SoldModelCard
                {
                    Title = ph.Order!.Product!.Car!.Brand + " " + ph.Order!.Product!.Car!.ModelName,
                    ProductNumber = ph.Order!.Product!.Number,
                    Year = ph.Order!.Product!.Car!.Year,
                    Price = ph.Order!.Product!.Car!.Price,
                    OrderId = ph.OrderId,
                    CreatedAt = ph.CreatedAt
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            ResultsItems.ItemsSource = new ObservableCollection<SoldModelCard>(list);
            var selectedSum = list.Sum(x => x.Price);
            SelectedSumText.Text = $"Сума обраної марки: ${selectedSum}";
            
            LoadBrandSums();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка виконання запиту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
