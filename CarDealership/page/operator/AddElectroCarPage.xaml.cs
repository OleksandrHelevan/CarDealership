using System;
using System.Linq;
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
    public partial class AddElectroCarPage : Page
    {
        private readonly DealershipContext _context;
        private readonly ElectroCarRepositoryImpl _repo;
        private readonly ElectroEngineServiceImpl _engineService;
        private readonly string _connectionString = "Host=localhost;Port=5432;Database=car_dealership;Username=postgres;Password=1234qwer";

        public AddElectroCarPage()
        {
            InitializeComponent();

            _context = new DealershipContext();
            _repo = new ElectroCarRepositoryImpl(_context);
            _engineService = new ElectroEngineServiceImpl();

            LoadEnums();
            LoadEngines();
        }

        private void LoadEnums()
        {
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

            MotorTypeComboBox.ItemsSource = Enum.GetValues(typeof(ElectroMotorType))
                .Cast<ElectroMotorType>()
                .Select(m => new ComboBoxItem { Content = m.ToFriendlyString(), Tag = m });
        }

        private void LoadEngines()
        {
            var engines = _engineService.GetAllElectroEngines()
                .Select(dto => new
                {
                    dto.Id,
                    Display = $"{dto.Power} кВт, {dto.BatteryCapacity} кВт⋅год, {dto.Range} км, {dto.MotorType.ToFriendlyString()}"
                })
                .ToList();

            // Логування всіх двигунів
            Console.WriteLine("=== Завантажені двигуни для dropdown ===");
            foreach (var e in engines)
            {
                Console.WriteLine($"Engine Id={e.Id}, Display={e.Display}");
            }

            EnginesComboBox.ItemsSource = engines;
            EnginesComboBox.DisplayMemberPath = "Display";
            EnginesComboBox.SelectedValuePath = "Id";
        }

        private void AddEngine_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(EnginePowerTextBox.Text, out double power) ||
                !double.TryParse(BatteryCapacityTextBox.Text, out double battery) ||
                !int.TryParse(RangeTextBox.Text, out int range) ||
                MotorTypeComboBox.SelectedItem is not ComboBoxItem motorItem)
            {
                MessageBox.Show("Некоректні значення для електродвигуна!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var engine = new ElectroEngine
            {
                Power = power,
                BatteryCapacity = battery,
                Range = range,
                MotorType = (ElectroMotorType)motorItem.Tag
            };

            _engineService.AddElectroEngine(engine);

            LoadEngines();

            EnginesComboBox.SelectedValue = engine.Id;

            MessageBox.Show($"Електродвигун додано успішно! Id={engine.Id}", "✅", MessageBoxButton.OK, MessageBoxImage.Information);

            EnginePowerTextBox.Clear();
            BatteryCapacityTextBox.Clear();
            RangeTextBox.Clear();
            MotorTypeComboBox.SelectedIndex = -1;
        }

        private void SyncElectroCarsSequence()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string sql = @"
                SELECT setval(
                    'electro_cars_id_seq',
                    COALESCE((SELECT MAX(id) FROM electro_cars), 0) + 1,
                    false
                );
            ";

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }

        private void AddElectroCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(BrandTextBox.Text) || string.IsNullOrWhiteSpace(ModelTextBox.Text))
                {
                    MessageBox.Show("Заповніть обов’язкові поля (Марка та Модель)!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(YearTextBox.Text, out int year) ||
                    !double.TryParse(PriceTextBox.Text, out double price) ||
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

                // Логування обраного двигуна
                Console.WriteLine($"Вибрано EngineId={engineId} для нового авто");

                // Перевіряємо наявність двигуна
                if (!_engineService.GetAllElectroEngines().Any(e => e.Id == engineId))
                {
                    MessageBox.Show("Обраний двигун не існує в базі!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                SyncElectroCarsSequence();

                var car = new ElectroCar
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
                MessageBox.Show("Електромобіль додано успішно!", "✅", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при додаванні авто: {ex.InnerException?.Message ?? ex.Message}", "❌", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
