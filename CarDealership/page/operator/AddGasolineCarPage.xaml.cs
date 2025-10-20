using System;
using System.Linq;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using CarDealership.config;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.repo.impl;
using CarDealership.service.impl;
using Npgsql;

namespace CarDealership.page.@operator
{
    public partial class AddGasolineCarPage : Page
    {
        private readonly GasolineCarRepositoryImpl _repo;
        private readonly GasolineEngineServiceImpl _engineService;
        private readonly string _connectionString = "Host=localhost;Port=5432;Database=car_dealership;Username=postgres;Password=1234qwer";

        public AddGasolineCarPage()
        {
            InitializeComponent();

            var context = new DealershipContext();
            _repo = new GasolineCarRepositoryImpl(context);
            _engineService = new GasolineEngineServiceImpl();

            ColorComboBox.ItemsSource = Enum.GetValues(typeof(Color))
                .Cast<Color>()
                .Select(c => new ComboBoxItem { Content = c.ToFriendlyString(), Tag = c });

            DriveTypeComboBox.ItemsSource = Enum.GetValues(typeof(DriveType))
                .Cast<DriveType>()
                .Select(d => new ComboBoxItem { Content = d.ToFriendlyString(), Tag = d });

            TransmissionTypeComboBox.ItemsSource = Enum.GetValues(typeof(TransmissionType))
                .Cast<TransmissionType>()
                .Select(t => new ComboBoxItem { Content = t.ToFriendlyString(), Tag = t });

            CarBodyTypeComboBox.ItemsSource = Enum.GetValues(typeof(CarBodyType))
                .Cast<CarBodyType>()
                .Select(b => new ComboBoxItem { Content = b.ToFriendlyString(), Tag = b });

            LoadEngines();
        }

        private void LoadEngines()
        {
            var engines = _engineService.GetAllGasolineEngines()
                .Select(dto => new
                {
                    dto.Id,
                    Display = $"{dto.Power} л.с., {dto.FuelType.ToFriendlyString()}, {dto.FuelConsumption} л/100км"
                })
                .ToList();

            Console.WriteLine("Завантажені двигуни:");
            foreach (var e in engines)
                Console.WriteLine($"Id={e.Id}, Display={e.Display}");

            EnginesComboBox.ItemsSource = engines;
            EnginesComboBox.DisplayMemberPath = "Display";
            EnginesComboBox.SelectedValuePath = "Id";
        }


        private void AddEngine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!decimal.TryParse(EnginePowerTextBox.Text, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal power) ||
                    EngineFuelTypeComboBox.SelectedItem is not ComboBoxItem fuelItem ||
                    !decimal.TryParse(EngineConsumptionTextBox.Text, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal consumption))
                {
                    MessageBox.Show("Некоректні значення для двигуна!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var engine = new GasolineEngine
                {
                    Power = power,
                    FuelType = (EngineFuelTypeComboBox.SelectedItem is ComboBoxItem fi ? (FuelType)fi.Tag : FuelType.Petrol),
                    FuelConsumption = consumption
                };
                _engineService.AddGasolineEngine(engine);
                Console.WriteLine($"Додано двигун: Power={engine.Power}, Fuel={engine.FuelType}, Consumption={engine.FuelConsumption}, Id={engine.Id}");

                LoadEngines();

                EnginesComboBox.SelectedValue = engine.Id;

                MessageBox.Show("Двигун додано успішно!", "✅", MessageBoxButton.OK, MessageBoxImage.Information);

                EnginePowerTextBox.Clear();
                EngineFuelTypeComboBox.SelectedIndex = -1;
                EngineConsumptionTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при додаванні двигуна: {ex.Message}", "❌", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SyncGasolineCarsSequence()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                string sql = @"
                    SELECT setval(
                        'cars_id_seq',
                        COALESCE((SELECT MAX(id) FROM cars), 0) + 1,
                        false
                    );
                ";

                using var cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Sequence cars_id_seq synchronized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error syncing sequence: {ex.Message}");
            }
        }

        private void AddGasolineCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(BrandTextBox.Text) || string.IsNullOrWhiteSpace(ModelTextBox.Text))
                {
                    MessageBox.Show("Заповніть обов’язкові поля (Марка та Модель)!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(YearTextBox.Text, out int year) ||
                    !decimal.TryParse(PriceTextBox.Text, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal price) ||
                    !int.TryParse(MileageTextBox.Text, out int mileage) ||
                    !int.TryParse(WeightTextBox.Text, out int weight) ||
                    !int.TryParse(NumberOfDoorsTextBox.Text, out int doors))
                {
                    MessageBox.Show("Некоректні числові значення!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (ColorComboBox.SelectedItem is not ComboBoxItem colorItem ||
                    DriveTypeComboBox.SelectedItem is not ComboBoxItem driveItem ||
                    TransmissionTypeComboBox.SelectedItem is not ComboBoxItem transItem ||
                    CarBodyTypeComboBox.SelectedItem is not ComboBoxItem bodyItem ||
                    EnginesComboBox.SelectedValue is null)
                {
                    MessageBox.Show("Оберіть всі параметри зі списків!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int engineId = (int)EnginesComboBox.SelectedValue;
                Console.WriteLine($"Вибраний EngineId для авто: {engineId}");

                SyncGasolineCarsSequence();

                var car = new GasolineCar
                {
                    Brand = BrandTextBox.Text,
                    ModelName = ModelTextBox.Text,
                    Year = year,
                    Price = price,
                    Mileage = mileage,
                    Weight = weight,
                    NumberOfDoors = doors,
                    Color = (Color)colorItem.Tag,
                    DriveType = (DriveType)driveItem.Tag,
                    Transmission = (TransmissionType)transItem.Tag,
                    BodyType = (CarBodyType)bodyItem.Tag,
                    EngineId = engineId
                };

                _repo.Add(car);
                MessageBox.Show("Бензинове авто додано успішно!", "✅", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при додаванні авто: {ex.InnerException?.Message ?? ex.Message}", "❌", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}


