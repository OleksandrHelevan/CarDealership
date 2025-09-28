using System;
using System.Linq;
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
    public partial class ElectroCarPage : Page
    {
        private readonly ProductServiceImpl _productService;
        private readonly BuyServiceImpl _buyService;
        private readonly MigrationServiceImpl _migrationService;
        private readonly string _currentUserLogin;

        public ICommand BuyCommand { get; }

        public ElectroCarPage(string userLogin = null)
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
                var electricProducts = allProducts.Where(p => p.CarType == CarType.Electro).ToList();
                ElectroCarsList.ItemsSource = electricProducts;
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
                var filter = new ElectroCarFilterDto
                {
                    SearchText = SearchBox.Text,
                    TransmissionType = GetSelectedTransmissionType(),
                    BodyType = GetSelectedBodyType(),
                    Color = GetSelectedColor(),
                    DriveType = GetSelectedDriveType(),
                    MotorType = GetSelectedMotorType(),
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

                // Get filtered cars from repository through service
                var electroCarService = new ElectroCarServiceImpl(new ElectroCarRepositoryImpl(new DealershipContext()));
                var filteredCars = electroCarService.GetFilteredCars(filter);
                
                // Convert to product DTOs for display
                var allProducts = _productService.GetAll();
                var filteredProducts = allProducts
                    .Where(p => p.CarType == CarType.Electro)
                    .Where(p => filteredCars.Any(c => c.Id == ((ElectroCarDto)p.Vehicle).Id))
                    .ToList();

                ElectroCarsList.ItemsSource = filteredProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при застосуванні фільтра: {ex.Message}");
            }
        }
        
        private TransmissionType? GetSelectedTransmissionType()
        {
            return FilterTransmission.SelectedIndex switch
            {
                1 => TransmissionType.Manual,
                2 => TransmissionType.Automatic,
                3 => TransmissionType.CVT,
                4 => TransmissionType.SemiAutomatic,
                _ => null
            };
        }

        private CarBodyType? GetSelectedBodyType()
        {
            return FilterBodyType.SelectedIndex switch
            {
                1 => CarBodyType.Micro,
                2 => CarBodyType.Hatchback,
                3 => CarBodyType.Sedan,
                4 => CarBodyType.Crossover,
                5 => CarBodyType.Coupe,
                6 => CarBodyType.CoupeSuv,
                7 => CarBodyType.Hyper,
                8 => CarBodyType.Suv,
                9 => CarBodyType.OffRoader,
                10 => CarBodyType.PickUp,
                11 => CarBodyType.Cabriolet,
                12 => CarBodyType.Sport,
                13 => CarBodyType.Muv,
                14 => CarBodyType.Wagon,
                15 => CarBodyType.Roadster,
                16 => CarBodyType.Limousine,
                17 => CarBodyType.Formula1,
                18 => CarBodyType.Muscle,
                19 => CarBodyType.Van,
                _ => null
            };
        }

        private Color? GetSelectedColor()
        {
            return FilterColor.SelectedIndex switch
            {
                1 => Color.Red,
                2 => Color.Blue,
                3 => Color.Green,
                4 => Color.Yellow,
                5 => Color.White,
                6 => Color.Black,
                7 => Color.Orange,
                8 => Color.Purple,
                9 => Color.Brown,
                10 => Color.Pink,
                11 => Color.Grey,
                12 => Color.LightBlue,
                _ => null
            };
        }

        private DriveType? GetSelectedDriveType()
        {
            return FilterDriveType.SelectedIndex switch
            {
                1 => DriveType.FWD,
                2 => DriveType.RWD,
                3 => DriveType.AWD,
                4 => DriveType.FourWD,
                _ => null
            };
        }

        private ElectroMotorType? GetSelectedMotorType()
        {
            return FilterMotorType.SelectedIndex switch
            {
                1 => ElectroMotorType.Asynchronous,
                2 => ElectroMotorType.DirectCurrent,
                3 => ElectroMotorType.Synchronous,
                4 => ElectroMotorType.PermanentMagnet,
                5 => ElectroMotorType.Induction,
                _ => null
            };
        }

        private int? GetIntValue(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            return int.TryParse(text, out int value) ? value : null;
        }

        private double? GetDoubleValue(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            return double.TryParse(text, out double value) ? value : null;
        }

        private float? GetFloatValue(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            return float.TryParse(text, out float value) ? value : null;
        }

        private void BuyCar(ProductDto product)
        {
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
                        Delivery = dialog.BuyCarDto.Delivery
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
            using var context = new DealershipContext();
            var user = context.Users.FirstOrDefault(u => u.Login == userLogin);
            var client = user != null ? context.Clients.FirstOrDefault(c => c.User.Id == user.Id) : null;
            return client?.Id ?? 0;
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        public RelayCommand(Action<T> execute) => _execute = execute;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) { if (parameter is T t) _execute(t); }
        public event EventHandler CanExecuteChanged;
    }
}
