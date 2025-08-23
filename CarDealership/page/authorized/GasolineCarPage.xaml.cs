using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.enums;
using CarDealership.repo.impl;
using CarDealership.service.impl;
using CarDealership.page.authorized;

namespace CarDealership.page.authorized
{
    public partial class GasolineCarPage 
    {
        private readonly ProductServiceImpl _productService;
        private readonly BuyServiceImpl _buyService;
        private readonly MigrationServiceImpl _migrationService;

        public ICommand BuyCommand { get; }

        public GasolineCarPage()
        {
            InitializeComponent();
            
            var productRepo = new ProductRepositoryImpl(new DealershipContext());
            var gasolineCarRepo = new GasolineCarRepository(new DealershipContext());
            var electroCarRepo = new ElectroCarRepositoryImpl(new DealershipContext());
            
            _productService = new ProductServiceImpl(productRepo);
            _migrationService = new MigrationServiceImpl(productRepo, gasolineCarRepo, electroCarRepo);
            var orderService = new OrderService(new OrderRepositoryImpl(new DealershipContext()));
            var clientRepo = new ClientRepository(new DealershipContext());
            _buyService = new BuyServiceImpl(productRepo, orderService, clientRepo);

            BuyCommand = new RelayCommand<ProductDto>(BuyCar);

            RunMigration();
            
            LoadData();
        }

        private void RunMigration()
        {
            try
            {
                _migrationService.MigrateGasolineCarsToProducts();
                _migrationService.MigrateElectroCarsToProducts();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Migration error: {ex.Message}");
            }
        }

        private void LoadData()
        {
            try
            {
                // Get only gasoline car products
                var allProducts = _productService.GetAll();
                var gasolineProducts = allProducts.Where(p => p.CarType == CarType.Gasoline).ToList();
                GasolineCarsList.ItemsSource = gasolineProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get all gasoline products
                var allProducts = _productService.GetAll();
                var gasolineProducts = allProducts.Where(p => p.CarType == CarType.Gasoline).ToList();

                // Apply filters to products
                var filteredProducts = gasolineProducts.Where(p =>
                {
                    var vehicle = p.Vehicle as GasolineCarDto;
                    if (vehicle == null) return false;

                    // Search text filter
                    if (!string.IsNullOrEmpty(SearchBox.Text.Trim()))
                    {
                        var searchText = SearchBox.Text.Trim().ToLower();
                        if (!vehicle.Brand.ToLower().Contains(searchText) && 
                            !vehicle.ModelName.ToLower().Contains(searchText))
                            return false;
                    }

                    // Transmission filter
                    var selectedTransmission = (FilterTransmission.SelectedItem as ComboBoxItem)?.Content.ToString();
                    if (!string.IsNullOrEmpty(selectedTransmission) && selectedTransmission != "КПП")
                    {
                        var transmissionType = FilterHelper.GetTransmissionType(selectedTransmission);
                        if (transmissionType.HasValue && vehicle.Transmission != transmissionType.Value)
                            return false;
                    }

                    // Body type filter
                    var selectedBodyType = (FilterBodyType.SelectedItem as ComboBoxItem)?.Content.ToString();
                    if (!string.IsNullOrEmpty(selectedBodyType) && selectedBodyType != "Тип кузова")
                    {
                        var bodyType = FilterHelper.GetBodyType(selectedBodyType);
                        if (bodyType.HasValue && vehicle.BodyType != bodyType.Value)
                            return false;
                    }

                    // Color filter
                    var selectedColor = (FilterColor.SelectedItem as ComboBoxItem)?.Content.ToString();
                    if (!string.IsNullOrEmpty(selectedColor) && selectedColor != "Колір")
                    {
                        var color = FilterHelper.GetColor(selectedColor);
                        if (color.HasValue && vehicle.Color != color.Value)
                            return false;
                    }

                    // Drive type filter
                    var selectedDriveType = (FilterDriveType.SelectedItem as ComboBoxItem)?.Content.ToString();
                    if (!string.IsNullOrEmpty(selectedDriveType) && selectedDriveType != "Привід")
                    {
                        var driveType = FilterHelper.GetDriveType(selectedDriveType);
                        if (driveType.HasValue && vehicle.DriveType != driveType.Value)
                            return false;
                    }

                    // Fuel type filter
                    var selectedFuelType = (FilterFuelType.SelectedItem as ComboBoxItem)?.Content.ToString();
                    if (!string.IsNullOrEmpty(selectedFuelType) && selectedFuelType != "Тип пального")
                    {
                        var fuelType = FilterHelper.GetFuelType(selectedFuelType);
                        if (fuelType.HasValue && vehicle.Engine is GasolineEngineDto engine && engine.FuelType != fuelType.Value)
                            return false;
                    }

                    // Range filters
                    if (int.TryParse(FilterYearFrom.Text, out int yearFrom) && yearFrom > 0 && vehicle.Year < yearFrom)
                        return false;
                    if (int.TryParse(FilterYearTo.Text, out int yearTo) && yearTo > 0 && vehicle.Year > yearTo)
                        return false;
                    if (double.TryParse(FilterPriceFrom.Text, out double priceFrom) && priceFrom > 0 && vehicle.Price < priceFrom)
                        return false;
                    if (double.TryParse(FilterPriceTo.Text, out double priceTo) && priceTo > 0 && vehicle.Price > priceTo)
                        return false;
                    if (float.TryParse(FilterWeightFrom.Text, out float weightFrom) && weightFrom > 0 && vehicle.Weight < weightFrom)
                        return false;
                    if (float.TryParse(FilterWeightTo.Text, out float weightTo) && weightTo > 0 && vehicle.Weight > weightTo)
                        return false;
                    if (int.TryParse(FilterMileageFrom.Text, out int mileageFrom) && mileageFrom > 0 && vehicle.Mileage < mileageFrom)
                        return false;
                    if (int.TryParse(FilterMileageTo.Text, out int mileageTo) && mileageTo > 0 && vehicle.Mileage > mileageTo)
                        return false;
                    if (int.TryParse(FilterDoorsFrom.Text, out int doorsFrom) && doorsFrom > 0 && vehicle.NumberOfDoors < doorsFrom)
                        return false;
                    if (int.TryParse(FilterDoorsTo.Text, out int doorsTo) && doorsTo > 0 && vehicle.NumberOfDoors > doorsTo)
                        return false;

                    return true;
                }).ToList();

                GasolineCarsList.ItemsSource = filteredProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при застосуванні фільтра: {ex.Message}", "Помилка");
            }
        }





        private void BuyCar(ProductDto product)
        {
            try
            {
                var vehicle = product.Vehicle as GasolineCarDto;
                if (vehicle == null)
                {
                    MessageBox.Show("Помилка: Невірний тип автомобіля", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Debug: Check if car has valid ID
                if (vehicle.Id <= 0)
                {
                    MessageBox.Show($"Помилка: Невірний ID автомобіля: {vehicle.Id}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Show buy dialog
                var buyDialog = new BuyCarDialog();
                buyDialog.Owner = Window.GetWindow(this);
                
                if (buyDialog.ShowDialog() == true)
                {
                    // Complete the buy car DTO with car information
                    var buyCarDto = buyDialog.BuyCarDto;
                    buyCarDto.CarId = vehicle.Id;
                    buyCarDto.CarType = CarType.Gasoline;
                    buyCarDto.AvailableFrom = DateTime.Now;

                    // Debug: Show the buy request details
                    System.Diagnostics.Debug.WriteLine($"Buying car: ID={vehicle.Id}, Brand={vehicle.Brand}, Model={vehicle.ModelName}");

                    // Attempt to buy the car
                    bool success = _buyService.BuyCar(buyCarDto);

                    if (success)
                    {
                        MessageBox.Show($"Ви успішно купили {vehicle.Brand} {vehicle.ModelName} за ${vehicle.Price}!\nЗамовлення створено в базі даних.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Помилка при покупці автомобіля. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при покупці: {ex.Message}\n\nДеталі: {ex.StackTrace}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is T t) _execute(t);
        }

        public event EventHandler CanExecuteChanged;
    }
}
