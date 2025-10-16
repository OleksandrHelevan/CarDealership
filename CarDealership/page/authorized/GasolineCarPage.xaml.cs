using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.enums;
using CarDealership.mapper;
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
                // Load all products (both gasoline and electro)
                var allProducts = _productService.GetAll();
                GasolineCarsList.ItemsSource = allProducts;
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
                // Create filter object from UI inputs
                var filter = new GasolineCarFilterDto
                {
                    SearchText = SearchBox.Text,
                    TransmissionType = GetSelectedTransmissionType(),
                    BodyType = GetSelectedBodyType(),
                    Color = GetSelectedColor(),
                    DriveType = GetSelectedDriveType(),
                    FuelType = GetSelectedFuelType(),
                    YearFrom = GetIntValue(FilterYearFrom.Text),
                    YearTo = GetIntValue(FilterYearTo.Text),
                    PriceFrom = GetDoubleValue(FilterPriceFrom.Text),
                    PriceTo = GetDoubleValue(FilterPriceTo.Text),
                    WeightFrom = GetFloatValue(FilterWeightFrom.Text),
                    WeightTo = GetFloatValue(FilterWeightTo.Text),
                    MileageFrom = GetIntValue(FilterMileageFrom.Text),
                    MileageTo = GetIntValue(FilterMileageTo.Text),
                    DoorsFrom = GetIntValue(FilterDoorsFrom.Text),
                    DoorsTo = GetIntValue(FilterDoorsTo.Text)
                };

                // Get filtered cars directly from database through service
                var gasolineCarService = new GasolineCarServiceImpl(new GasolineCarRepository(new DealershipContext()));
                var filteredCars = gasolineCarService.GetFiltered(filter);
                
                // Convert to product DTOs for display
                var productRepo = new ProductRepositoryImpl(new DealershipContext());
                var filteredProducts = productRepo.GetByVehicleIds(
                    filteredCars.Select(c => c.Id).ToList(), 
                    CarType.Gasoline
                ).Select(ProductMapper.ToDto).ToList();

                GasolineCarsList.ItemsSource = filteredProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при застосуванні фільтра: {ex.Message}");
            }
        }
        
        private TransmissionType? GetSelectedTransmissionType()
        {
            var selectedItem = FilterTransmission.SelectedItem as ComboBoxItem;
            if (selectedItem == null || selectedItem.Content.ToString() == "КПП")
                return null;
                
            return FilterHelper.GetTransmissionType(selectedItem.Content.ToString());
        }
        
        private CarBodyType? GetSelectedBodyType()
        {
            var selectedItem = FilterBodyType.SelectedItem as ComboBoxItem;
            if (selectedItem == null || selectedItem.Content.ToString() == "Тип кузова")
                return null;
                
            return FilterHelper.GetBodyType(selectedItem.Content.ToString());
        }
        
        private Color? GetSelectedColor()
        {
            var selectedItem = FilterColor.SelectedItem as ComboBoxItem;
            if (selectedItem == null || selectedItem.Content.ToString() == "Колір")
                return null;
                
            return FilterHelper.GetColor(selectedItem.Content.ToString());
        }
        
        private DriveType? GetSelectedDriveType()
        {
            var selectedItem = FilterDriveType.SelectedItem as ComboBoxItem;
            if (selectedItem == null || selectedItem.Content.ToString() == "Привід")
                return null;
                
            return FilterHelper.GetDriveType(selectedItem.Content.ToString());
        }
        
        private FuelType? GetSelectedFuelType()
        {
            var selectedItem = FilterFuelType.SelectedItem as ComboBoxItem;
            if (selectedItem == null || selectedItem.Content.ToString() == "Тип пального")
                return null;
                
            return FilterHelper.GetFuelType(selectedItem.Content.ToString());
        }
        
        private int? GetIntValue(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            return int.TryParse(text, out int value) ? value : null;
        }
        
        private double? GetDoubleValue(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            return double.TryParse(text, out double value) ? value : null;
        }
        
        private float? GetFloatValue(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            return float.TryParse(text, out float value) ? value : null;
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

  
}
