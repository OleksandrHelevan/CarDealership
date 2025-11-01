using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.entity;

namespace CarDealership.page.@operator
{
    public partial class EditProductPage : Page
    {
        private readonly DealershipContext _context;
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public EditProductPage()
        {
            InitializeComponent();

            _context = new DealershipContext();
            EditCommand = new RelayCommand<Product>(OpenEditDialog);
            DeleteCommand = new RelayCommand<Product>(DeleteProduct);

            DataContext = this;

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var allProducts = _context.Products
                    .Include(p => p.Car)
                    .ThenInclude(c => c.Engine)
                    .ToList();

                ProductsList.ItemsSource = allProducts;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Помилка завантаження: {ex.Message}");
            }
        }

        private void OpenEditDialog(Product product)
        {
            if (product == null) return;

            var dialog = new EditProductDialog(product)
            {
                Owner = Window.GetWindow(this),
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _context.SaveChanges();
                    MessageBox.Show("Продукт успішно оновлено!");
                    LoadData();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Помилка оновлення: {ex.Message}\n\nДеталі: {ex.InnerException?.Message}");
                }
            }
        }

        private void DeleteProduct(Product product)
        {
            if (product == null) return;

            var result = MessageBox.Show(
                "Видалити цей продукт?", "Підтвердження",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                MessageBox.Show("Продукт видалено успішно.");
                LoadData();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Помилка видалення: {ex.Message}\n\nДеталі: {ex.InnerException?.Message}");
            }
        }

        private void StockStatusApplyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selected = StockStatusFilter.SelectedItem as ComboBoxItem;
                var text = selected?.Content?.ToString() ?? string.Empty;

                var query = _context.Products
                    .Include(p => p.Car)
                    .ThenInclude(c => c.Engine)
                    .AsQueryable();

                if (string.Equals(text, "Нема в наявності", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(p => !p.InStock);
                }
                else if (string.Equals(text, "В наявності", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(p => p.InStock);
                }

                ProductsList.ItemsSource = query.ToList();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Помилка фільтру: {ex.Message}");
            }
        }
    }
}


