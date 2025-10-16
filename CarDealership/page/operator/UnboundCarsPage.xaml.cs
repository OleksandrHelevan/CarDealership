using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore; // важливо для Include
using CarDealership.config;
using CarDealership.dto;
using CarDealership.enums;
using CarDealership.mapper;
using CarDealership.repo.impl;
using CarDealership.service.impl;

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
                // 🔹 Бензинові авто
                var gasolineUnbound = context.GasolineCars
                    .Include(gc => gc.Engine) // підвантажуємо двигун
                    .Where(gc => !context.Products.Any(p => p.GasolineCarId == gc.Id))
                    .ToList();

                foreach (var gc in gasolineUnbound)
                {
                    if (gc.Engine == null)
                        Debug.WriteLine($"⚠ GasolineCar Id={gc.Id}, Brand={gc.Brand} має NULL Engine!");
                    else
                        Debug.WriteLine($"✅ GasolineCar Id={gc.Id}, EngineId={gc.Engine.Id}");
                }

                var gasolineDto = gasolineUnbound
                    .Select(gc => GasolineCarMapper.ToDto(gc)) // безпечний мапінг
                    .ToList<Vehicle>();

                // 🔹 Електроавто
                var electroUnbound = context.ElectroCars
                    .Include(ec => ec.Engine)
                    .Where(ec => !context.Products.Any(p => p.ElectroCarId == ec.Id))
                    .ToList();

                foreach (var ec in electroUnbound)
                {
                    if (ec.Engine == null)
                        Debug.WriteLine($"⚠ ElectroCar Id={ec.Id}, Brand={ec.Brand} має NULL Engine!");
                    else
                        Debug.WriteLine($"✅ ElectroCar Id={ec.Id}, EngineId={ec.Engine.Id}");
                }

                var electroDto = electroUnbound
                    .Select(ec => ElectroCarMapper.ToDto(ec)) // безпечний мапінг
                    .ToList<Vehicle>();

                // 🔹 Об’єднуємо усі авто
                var all = gasolineDto.Concat(electroDto).ToList();
                UnboundCarsList.ItemsSource = all;

                Debug.WriteLine($"Ітого Unbound Cars: {all.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Помилка у LoadUnboundCars: {ex}");
                MessageBox.Show($"Помилка завантаження авто: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PutOnSale_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.DataContext is not Vehicle vehicle) return;

            var dialog = new PutOnSaleDialog(vehicle);
            dialog.Owner = Window.GetWindow(this);
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
                    MessageBox.Show("Авто виставлено на продаж");
                    LoadUnboundCars();
                }
                else
                {
                    MessageBox.Show("Продукт з таким номером вже існує");
                }
            }
        }
    }
}
