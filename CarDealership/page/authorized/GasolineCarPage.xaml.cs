using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.enums;
using CarDealership.repo.impl;
using CarDealership.service.impl;

namespace CarDealership.page.authorized
{
    public partial class GasolineCarPage 
    {
        private readonly ProductServiceImpl _productService;
        private readonly BuyServiceImpl _buyService;
        private readonly MigrationServiceImpl _migrationService;
        private readonly string _currentUserLogin;

        public ICommand BuyCommand { get; }

        public GasolineCarPage(string userLogin = null)
        {
            InitializeComponent();
            _currentUserLogin = userLogin;
    
            var productRepo = new ProductRepositoryImpl(new DealershipContext());
            var gasolineCarRepo = new GasolineCarRepository(new DealershipContext());
            var electroCarRepo = new ElectroCarRepositoryImpl(new DealershipContext());
    
            _productService = new ProductServiceImpl(productRepo);
            _migrationService = new MigrationServiceImpl(productRepo, gasolineCarRepo, electroCarRepo);
            var orderService = new OrderService(new OrderRepositoryImpl(new DealershipContext()));
            var clientRepo = new ClientRepository(new DealershipContext());
            _buyService = new BuyServiceImpl(productRepo, orderService, clientRepo);

            BuyCommand = new RelayCommand<ProductDto>(BuyCar);

            DataContext = this; // ← ОБОВ’ЯЗКОВО

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
            System.Diagnostics.Debug.WriteLine($"BuyCar called for product: {product?.Number}");
            try
            {
                // відкриваємо діалогове вікно
                var dialog = new BuyCarDialog
                {
                    Owner = Window.GetWindow(this) // щоб модальне вікно було поверх сторінки
                };

                if (dialog.ShowDialog() == true) // користувач натиснув "Купити"
                {
                    var clientId = GetClientIdFromUser(_currentUserLogin);
                    if (clientId == 0)
                    {
                        MessageBox.Show("Не знайдено клієнта для користувача");
                        return;
                    }

                    // дані з діалогу + дані з продукту
                    var dto = new BuyCarDto
                    {
                        Id = product.Id,
                        CarType = product.CarType,
                        CountryOfOrigin = product.CountryOfOrigin,
                        AvailableFrom = product.AvailableFrom,
                        ClientId = clientId,
                        PaymentType = dialog.BuyCarDto.PaymentType, // з діалогу
                        Delivery = dialog.BuyCarDto.Delivery        // з діалогу
                    };

                    _buyService.BuyCar(dto);

                    MessageBox.Show("Замовлення успішно створено!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка покупки: {ex.Message}");
            }
        }

        private int GetClientIdFromUser(string userLogin)
        {
            try
            {
                if (string.IsNullOrEmpty(userLogin))
                {
                    System.Diagnostics.Debug.WriteLine("User login is null or empty");
                    return 0;
                }

                using var context = new DealershipContext();
                
                // Find user by login
                var user = context.Users.FirstOrDefault(u => u.Login == userLogin);
                if (user == null)
                {
                    System.Diagnostics.Debug.WriteLine($"User not found with login: {userLogin}");
                    return 0;
                }

                // Find client by user ID
                var client = context.Clients.FirstOrDefault(c => c.User.Id == user.Id);
                if (client == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Client not found for user: {userLogin}");
                    return 0;
                }

                System.Diagnostics.Debug.WriteLine($"Found client ID: {client.Id} for user: {userLogin}");
                return client.Id;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting client ID: {ex.Message}");
                return 0;
            }
        }

        private int GetProductIdFromDatabase(int carId)
        {
            try
            {
                using var context = new DealershipContext();
                
                // Find product by gasoline car ID
                var product = context.Products.FirstOrDefault(p => p.GasolineCarId == carId);
                if (product == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Product not found for car ID: {carId}");
                    return 0;
                }

                System.Diagnostics.Debug.WriteLine($"Found product ID: {product.Id} for car ID: {carId}");
                return product.Id;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting product ID: {ex.Message}");
                return 0;
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
