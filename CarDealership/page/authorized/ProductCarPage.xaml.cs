using System.Windows;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.repo.impl;
using CarDealership.service.impl;

namespace CarDealership.page.authorized
{
    public partial class ProductCarPage
    {
        private readonly ProductServiceImpl _productService;

        public ProductCarPage()
        {
            InitializeComponent();
            _productService = new ProductServiceImpl(new ProductRepositoryImpl(new DealershipContext()));
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var products = _productService.GetAll();
                ProductsList.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
