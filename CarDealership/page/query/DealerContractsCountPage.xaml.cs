using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarDealership.config;
using Microsoft.EntityFrameworkCore;
using CarDealership.enums;

namespace CarDealership.page.query;

public partial class DealerContractsCountPage : Page
{
    private readonly DealershipContext _context;

    public class Row
    {
        public int DealerId { get; set; }
        public string DealerLogin { get; set; } = string.Empty;
        public int ContractsCount { get; set; }
    }

    private class DealerOption
    {
        public int? Id { get; set; }
        public string Display { get; set; } = string.Empty;
        public override string ToString() => Display;
    }

    public DealerContractsCountPage()
    {
        InitializeComponent();
        _context = new DealershipContext();
        LoadUsers();
        RunQuery();
    }

    private void LoadUsers()
    {
        try
        {
            var users = _context.Users
                .Where(u => u.AccessRight == AccessRight.Operator || u.AccessRight == AccessRight.Admin)
                .OrderBy(u => u.Login)
                .Select(u => new DealerOption { Id = u.Id, Display = u.Login })
                .ToList();

            var items = new System.Collections.Generic.List<DealerOption>
            {
                new DealerOption { Id = null, Display = "Усі дилери" }
            };
            items.AddRange(users);
            DealerPicker.ItemsSource = items;
            DealerPicker.SelectedIndex = 0;
        }
        catch (System.Exception ex)
        {
            MessageBox.Show($"Помилка завантаження користувачів: {ex.Message}");
        }
    }

    private void RunQuery()
    {
        try
        {
            int? selectedId = (DealerPicker.SelectedItem as DealerOption)?.Id;

            var baseUsers = _context.Users
                .Where(u => u.AccessRight == AccessRight.Operator || u.AccessRight == AccessRight.Admin);

            var q = from u in baseUsers
                    where (selectedId == null || u.Id == selectedId)
                    join ph in _context.PaymentHistory on u.Id equals ph.OperatorId into g
                    from phg in g.DefaultIfEmpty()
                    group phg by new { u.Id, u.Login } into grp
                    orderby grp.Key.Login
                    select new Row
                    {
                        DealerId = grp.Key.Id,
                        DealerLogin = grp.Key.Login,
                        ContractsCount = grp.Count(x => x != null)
                    };

            var list = q.ToList();
            Results.ItemsSource = new ObservableCollection<Row>(list);
        }
        catch (System.Exception ex)
        {
            MessageBox.Show($"Помилка завантаження: {ex.Message}");
        }
    }

    private void DealerPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RunQuery();
    }
}
