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
        private readonly MigrationServiceImpl _migrationService;
        private readonly string _currentUserLogin;

        public ICommand BuyCommand { get; }

        public ProductsPage(string userLogin = null)
        {
            InitializeComponent();
            _currentUserLogin = userLogin;
    
            var productRepo = new ProductRepositoryImpl(new DealershipContext());

            _productService = new ProductServiceImpl(productRepo);
            _migrationService = new MigrationServiceImpl(productRepo);
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
                System.Diagnostics.Debug.WriteLine($"Migration error: {ex.Message}");
            }
        }

        private void LoadData()
        {
            try
            {
                var allProducts = _productService.GetAll();
                GasolineCarsList.ItemsSource = allProducts.ToList();
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
                // TODO: Implement unified Car filtering; for now, reload all
                var productRepo = new ProductRepositoryImpl(new DealershipContext());
                var allProducts = productRepo.GetAll().Select(ProductMapper.ToDto).ToList();

                // stock filter removed from ProductsPage

                GasolineCarsList.ItemsSource = allProducts;
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
                        Delivery = dialog.BuyCarDto.Delivery,
                        Address = dialog.BuyCarDto.Address,
                        PhoneNumber = dialog.BuyCarDto.PhoneNumber
                    };

                    var ok = _buyService.BuyCar(dto);
                    if (ok)
                    {
                        MessageBox.Show("Замовлення успішно створено!");
                    }
                    else
                    {
                        // Try to provide a more specific reason
                        try
                        {
                            using var ctx = new DealershipContext();
                            var pr = new ProductRepositoryImpl(ctx).GetById(product.Id);
                            if (pr == null)
                            {
                                MessageBox.Show($"Продукт не знайдено: ID={product.Id}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else if (!pr.InStock)
                            {
                                MessageBox.Show($"Продукт №{pr.Number} відсутній на складі.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Не вдалося створити замовлення", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        catch (Exception innerEx)
                        {
                            MessageBox.Show($"Не вдалося створити замовлення: {innerEx.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
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
                
                var user = context.Users.FirstOrDefault(u => u.Login == userLogin);
                if (user == null)
                {
                    System.Diagnostics.Debug.WriteLine($"User not found with login: {userLogin}");
                    return 0;
                }

                var client = context.Clients.FirstOrDefault(c => c.UserId == user.Id);
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
                
                var product = context.Products.FirstOrDefault(p => p.CarId == carId);
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
