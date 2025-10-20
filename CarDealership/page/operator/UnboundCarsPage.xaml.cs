using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.mapper;
using CarDealership.repo.impl;
using CarDealership.service.impl;
using CarDealership.enums;
using CarDealership.entity;

namespace CarDealership.page.@operator
{
    public partial class UnboundCarsPage : Page
    {
        private readonly ProductServiceImpl _productService;

        public UnboundCarsPage()
        {
            InitializeComponent();
            _productService = new ProductServiceImpl(new ProductRepositoryImpl(new DealershipContext()));
            LoadUnboundCars();
        }

        private void LoadUnboundCars()
        {
            using var context = new DealershipContext();

            try
            {
                Debug.WriteLine("=== LoadUnboundCars START ===");

                // 🔹 Завантажуємо усі машини, які ще не мають Product
                var unboundCars = context.Cars
                    .Include(c => (c as GasolineCar).Engine)
                    .Include(c => (c as ElectroCar).Engine)
                    .AsSplitQuery()
                    .Where(c => !context.Products.Any(p => p.CarId == c.Id && p.CarType == c.CarType))
                    .ToList();

                Debug.WriteLine($"Found {unboundCars.Count} unbound cars total.");

                // 🔹 Розділяємо по типах і мапимо у Vehicle DTO
                var gasolineCars = unboundCars.OfType<GasolineCar>().ToList();
                var electroCars = unboundCars.OfType<ElectroCar>().ToList();

                Debug.WriteLine($"GasolineCars: {gasolineCars.Count}, ElectroCars: {electroCars.Count}");

                var gasolineDtos = gasolineCars
                    .Select(gc =>
                    {
                        var dto = GasolineCarMapper.ToDto(gc);
                        dto.VehicleTypeName = "Бензиновий автомобіль";
                        return dto;
                    })
                    .ToList<Vehicle>();

                var electroDtos = electroCars
                    .Select(ec =>
                    {
                        var dto = ElectroCarMapper.ToDto(ec);
                        dto.VehicleTypeName = "Електромобіль";
                        return dto;
                    })
                    .ToList<Vehicle>();

                var all = gasolineDtos.Concat(electroDtos).ToList();

                UnboundCarsList.ItemsSource = all;
                Debug.WriteLine($"Ітого Unbound Cars (after mapping): {all.Count}");

                Debug.WriteLine("=== LoadUnboundCars END ===");
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"❌ Помилка у LoadUnboundCars: {ex}");
                MessageBox.Show($"Помилка завантаження авто: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PutOnSale_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.DataContext is not Vehicle vehicle) return;

            var dialog = new PutOnSaleDialog(vehicle)
            {
                Owner = Window.GetWindow(this)
            };

            if (dialog.ShowDialog() == true)
            {
                var product = dialog.CreatedProduct;
                if (product == null)
                {
                    MessageBox.Show("Не вдалося створити продукт");
                    return;
                }

                if (_productService.Create(product))
                {
                    MessageBox.Show("✅ Авто виставлено на продаж");
                    LoadUnboundCars();
                }
                else
                {
                    MessageBox.Show("⚠️ Продукт з таким номером вже існує");
                }
            }
        }
    }
}
