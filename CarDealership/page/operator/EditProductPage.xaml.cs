
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

        public EditProductPage()
        {
            InitializeComponent();

            _context = new DealershipContext();
            EditCommand = new RelayCommand<Product>(EditProduct);

            DataContext = this;

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var allProducts = _context.Products
                    .Include(p => p.ElectroCar)
                    .ThenInclude(ec => ec.Engine)
                    .Include(p => p.GasolineCar)
                    .ThenInclude(gc => gc.Engine)
                    .ToList();

                ProductsList.ItemsSource = allProducts;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Помилка завантаження: {ex.Message}");
            }
        }

        private void EditProduct(Product product)
        {
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
    }
}