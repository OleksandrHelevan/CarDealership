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
    public partial class ProductsPage
    {
        private readonly ProductServiceImpl _productService;
        private readonly BuyServiceImpl _buyService;
        private readonly MigrationService _migrationService;
        private readonly string _currentUserLogin;

        public ICommand BuyCommand { get; }

        public ProductsPage(string userLogin = null)
        {
            InitializeComponent();
            _currentUserLogin = userLogin;

            var productRepo = new ProductRepositoryImpl(new DealershipContext());
            _productService = new ProductServiceImpl(productRepo);
            _migrationService = new MigrationService(new DealershipContext());
            var orderService = new OrderService(new OrderRepositoryImpl(new DealershipContext()));
            var clientRepo = new ClientRepository(new DealershipContext());
            _buyService = new BuyServiceImpl(productRepo, orderService, clientRepo);

            BuyCommand = new RelayCommand<ProductDto>(BuyCar);

            DataContext = this;

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
                Log($"❌ Migration error: {ex.Message}");
            }
        }

        private void LoadData()
        {
            try
            {
                Log("=== Loading Products from DB ===");

                var allProducts = _productService.GetAll()?.ToList() ?? new List<ProductDto>();
                GasolineCarsList.ItemsSource = allProducts;

                Log($"✅ Loaded {allProducts.Count} products total.");

                foreach (var p in allProducts)
                {
                    Log(
                        $"→ Product ID: {p.Id}, CarType: {p.CarType}, Car: {p.CountryOfOrigin} {p.Id}, Price: {p.CarType}");
                }

                if (allProducts.Count == 0)
                    Log("⚠️ No products found in DB.");
            }
            catch (Exception ex)
            {
                Log($"❌ Error loading products: {ex.Message}");
                MessageBox.Show(ex.Message);
            }
        }

        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log("=== Applying filter ===");

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

                Log(
                    $"Filter → Search: '{filter.SearchText}', Color: {filter.Color}, Year: {filter.YearFrom}-{filter.YearTo}, Price: {filter.PriceFrom}-{filter.PriceTo}");

                var gasolineCarService = new GasolineCarServiceImpl(new GasolineCarRepositoryImpl(new DealershipContext()));
                var filteredCars = gasolineCarService.GetFiltered(filter);

                Log($"Found {filteredCars.Count()} matching cars from DB.");

                var productRepo = new ProductRepositoryImpl(new DealershipContext());
                var filteredProducts = productRepo.GetByVehicleIds(
                    filteredCars.Select(c => c.Id).ToList(),
                    CarType.Gasoline
                ).Select(ProductMapper.ToDto).ToList();

                Log($"Filtered {filteredProducts.Count} products matched the filter.");
                GasolineCarsList.ItemsSource = filteredProducts;

                foreach (var fp in filteredProducts)
                    Log($"→ {fp.CarType} {fp.Id}, {fp.CountryOfOrigin} ₴");
            }
            catch (Exception ex)
            {
                Log($"❌ Filter error: {ex.Message}");
                MessageBox.Show($"Помилка при застосуванні фільтра: {ex.Message}");
            }
        }

        private void BuyCar(ProductDto product)
        {
            Log($"🛒 BuyCar() called for product: {product?.Number ?? "null"}");
            try
            {
                var dialog = new BuyCarDialog
                {
                    Owner = Window.GetWindow(this)
                };

                if (dialog.ShowDialog() == true)
                {
                    var clientId = GetClientIdFromUser(_currentUserLogin);
                    if (clientId == 0)
                    {
                        MessageBox.Show("Не знайдено клієнта для користувача");
                        Log("⚠️ Client not found for user.");
                        return;
                    }

                    var dto = new BuyCarDto
                    {
                        Id = product.Id,
                        CarType = product.CarType,
                        CountryOfOrigin = product.CountryOfOrigin,
                        AvailableFrom = product.AvailableFrom,
                        ClientId = clientId,
                        PaymentType = dialog.BuyCarDto.PaymentType,
                        DeliveryRequired = dialog.BuyCarDto.DeliveryRequired
                    };

                    var success = _buyService.BuyCar(dto);
                    if (success)
                    {
                        MessageBox.Show("✅ Автомобіль успішно придбано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("❌ Не вдалося здійснити покупку. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                    Log($"✅ Purchase complete for Product ID {product.Id} ({product.Id} {product.CarType})");
                }
                else
                {
                    Log("❌ BuyCarDialog cancelled by user.");
                }
            }
            catch (Exception ex)
            {
                Log($"❌ Error during purchase: {ex.Message}");
                MessageBox.Show($"Помилка покупки: {ex.Message}");
            }
        }

        private int GetClientIdFromUser(string userLogin)
        {
            try
            {
                if (string.IsNullOrEmpty(userLogin))
                {
                    Log("⚠️ User login is null or empty.");
                    return 0;
                }

                using var context = new DealershipContext();

                var user = context.Users.FirstOrDefault(u => u.Login == userLogin);
                if (user == null)
                {
                    Log($"⚠️ User not found with login: {userLogin}");
                    return 0;
                }

                var client = context.Clients.FirstOrDefault(c => c.UserId == user.Id);
                if (client == null)
                {
                    Log($"⚠️ Client not found for user: {userLogin}");
                    return 0;
                }

                Log($"✅ Found client ID {client.Id} for user {userLogin}");
                return client.Id;
            }
            catch (Exception ex)
            {
                Log($"❌ Error getting client ID: {ex.Message}");
                return 0;
            }
        }

        private int GetProductIdFromDatabase(int carId)
        {
            try
            {
                using var context = new DealershipContext();

                var product = context.Products.FirstOrDefault(p => p.CarId == carId && p.CarType == CarType.Gasoline);
                if (product == null)
                {
                    Log($"⚠️ Product not found for car ID {carId}");
                    return 0;
                }

                Log($"✅ Found product ID {product.Id} for car ID {carId}");
                return product.Id;
            }
            catch (Exception ex)
            {
                Log($"❌ Error getting product ID: {ex.Message}");
                return 0;
            }
        }

        private void Log(string message)
        {
            var time = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{time}] {message}");
            System.Diagnostics.Debug.WriteLine($"[{time}] {message}");
        }

        #region Helper Methods (unchanged)

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

        private int? GetIntValue(string text) => int.TryParse(text, out var v) ? v : null;
        private double? GetDoubleValue(string text) => double.TryParse(text, out var v) ? v : null;
        private float? GetFloatValue(string text) => float.TryParse(text, out var v) ? v : null;

        #endregion
    }
}
