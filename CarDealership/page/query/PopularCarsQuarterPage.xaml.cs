using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CarDealership.config;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.page.query;

public partial class PopularCarsQuarterPage
{
    private readonly DealershipContext _context;

    private record PeriodItem(string Label, DateTime Start, DateTime End);

    public class PopularItem
    {
        public int Rank { get; set; }
        public string Title { get; set; } = string.Empty; // Brand Model
        public int Count { get; set; }
    }

    public PopularCarsQuarterPage()
    {
        InitializeComponent();
        _context = new DealershipContext();
        LoadPeriods();
        RunQuery();
    }

    private void LoadPeriods()
    {
        // Динамічно генеруємо квартальні періоди від 01.01.2024 до сьогодні
        var items = new List<PeriodItem>();
        var start = new DateTime(2024, 1, 1);
        var today = DateTime.Today;

        int q = 1;
        while (start < today)
        {
            var end = start.AddMonths(3);
            var labelEnd = end > today ? today : end;
            var label = $"Q{q} {start:yyyy} ({start:dd.MM.yyyy}–{labelEnd:dd.MM.yyyy})";
            // Для запиту включаємо поточний день: ексклюзивна верхня межа = завтра
            var queryEnd = end > today ? today.AddDays(1) : end;
            items.Add(new PeriodItem(label, start, queryEnd));

            start = end;
            q = q == 4 ? 1 : q + 1;
        }

        QuarterPicker.ItemsSource = items;
        QuarterPicker.DisplayMemberPath = nameof(PeriodItem.Label);
        QuarterPicker.SelectedIndex = items.Count > 0 ? items.Count - 1 : 0;
    }

    private void BtnRun_Click(object sender, RoutedEventArgs e)
    {
        RunQuery();
    }

    private void RunQuery()
    {
        try
        {
            var period = QuarterPicker.SelectedItem as PeriodItem;
            if (period == null) return;

            var top = _context.PaymentHistory
                .Include(ph => ph.Order)
                    .ThenInclude(o => o.Product)
                        .ThenInclude(p => p.Car)
                .Where(ph => ph.CreatedAt >= period.Start && ph.CreatedAt < period.End)
                .Where(ph => ph.Order != null && ph.Order.Product != null && ph.Order.Product.Car != null)
                .AsEnumerable()
                .GroupBy(ph => new { ph.Order!.Product!.Car!.Brand, ph.Order!.Product!.Car!.ModelName })
                .Select(g => new PopularItem
                {
                    Title = g.Key.Brand + " " + g.Key.ModelName,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Title)
                .Take(10)
                .ToList();

            for (int i = 0; i < top.Count; i++) top[i].Rank = i + 1;

            ResultsItems.ItemsSource = new ObservableCollection<PopularItem>(top);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка запиту: {ex.Message}");
        }
    }
}
