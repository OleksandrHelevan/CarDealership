using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using CarDealership.config;
using CarDealership.entity;
using CarDealership.dto;
using CarDealership.enums;
using CarDealership.repo.impl;
using CarDealership.service.impl;

namespace CarDealership.page.@operator
{
    public partial class UnboundCarsPage : Page
    {
        private readonly DealershipContext _context;
        private readonly ProductServiceImpl _productService;

        public UnboundCarsPage()
        {
            InitializeComponent();
            _context = new DealershipContext();
            _productService = new ProductServiceImpl(new ProductRepositoryImpl(new DealershipContext()));
            LoadUnboundCars();
        }

        private void LoadUnboundCars()
        {
            try
            {
                var cars = _context.Cars
                    .Include(c => c.Engine)
                    .Where(c => !c.OnSale)
                    .ToList();

                CarsList.ItemsSource = cars;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Помилка завантаження: {ex.Message}");
            }
        }

        private void PutOnSale_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.DataContext is not Car car)
                return;

            var vehicle = ToVehicle(car);
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
                    // mark car as on sale
                    var carToUpdate = _context.Cars.FirstOrDefault(x => x.Id == car.Id);
                    if (carToUpdate != null)
                    {
                        carToUpdate.OnSale = true;
                        _context.SaveChanges();
                    }
                    MessageBox.Show("Авто виставлено на продаж");
                    LoadUnboundCars();
                }
                else
                {
                    MessageBox.Show("Продукт з таким номером вже існує");
                }
            }
        }

        private static Vehicle ToVehicle(Car c)
        {
            var engineDto = new EngineDto
            {
                Id = c.Engine.Id,
                EngineType = c.Engine.EngineType,
                Power = c.Engine.Power,
                FuelType = c.Engine.FuelType,
                FuelConsumption = c.Engine.FuelConsumption,
                BatteryCapacity = c.Engine.BatteryCapacity,
                Range = c.Engine.Range,
                MotorType = c.Engine.MotorType
            };

            return new Vehicle
            {
                Id = c.Id,
                CarType = c.CarType,
                Brand = c.Brand,
                ModelName = c.ModelName,
                Engine = engineDto,
                Color = c.Color,
                ColorString = c.Color.ToFriendlyString(),
                Mileage = c.Mileage,
                Price = (double)c.Price,
                Weight = c.Weight,
                Year = c.Year,
                NumberOfDoors = c.NumberOfDoors,
                BodyType = c.BodyType,
                BodyTypeString = c.BodyType.ToFriendlyString(),
                DriveType = c.DriveType,
                DriveTypeString = c.DriveType.ToFriendlyString(),
                Transmission = c.Transmission,
                TransmissionString = c.Transmission.ToFriendlyString()
            };
        }
    }
}
