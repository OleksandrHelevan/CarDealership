using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarDealership.config;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.page.@operator
{
    public partial class AddCarPage : Page
    {
        private readonly DealershipContext _context;

        public AddCarPage()
        {
            InitializeComponent();
            _context = new DealershipContext();

            LoadEnumCombos();
            UpdateEngineFieldsVisibility();
        }

        private void LoadEnumCombos()
        {
            EngineTypeCombo.ItemsSource = Enum.GetValues(typeof(EngineType))
                .Cast<EngineType>()
                .Select(t => new ComboBoxItem { Content = t.ToFriendlyString(), Tag = t })
                .ToList();
            EngineTypeCombo.SelectedIndex = 0;

            ColorCombo.ItemsSource = Enum.GetValues(typeof(Color)).Cast<Color>()
                .Select(c => new ComboBoxItem { Content = c.ToFriendlyString(), Tag = c }).ToList();
            DriveTypeCombo.ItemsSource = Enum.GetValues(typeof(DriveType)).Cast<DriveType>()
                .Select(d => new ComboBoxItem { Content = d.ToFriendlyString(), Tag = d }).ToList();
            TransmissionCombo.ItemsSource = Enum.GetValues(typeof(TransmissionType)).Cast<TransmissionType>()
                .Select(t => new ComboBoxItem { Content = t.ToFriendlyString(), Tag = t }).ToList();
            BodyTypeCombo.ItemsSource = Enum.GetValues(typeof(CarBodyType)).Cast<CarBodyType>()
                .Select(b => new ComboBoxItem { Content = b.ToFriendlyString(), Tag = b }).ToList();

            FuelTypeCombo.ItemsSource = Enum.GetValues(typeof(FuelType)).Cast<FuelType>()
                .Select(f => new ComboBoxItem { Content = f.ToFriendlyString(), Tag = f }).ToList();
            MotorTypeCombo.ItemsSource = Enum.GetValues(typeof(ElectroMotorType)).Cast<ElectroMotorType>()
                .Select(m => new ComboBoxItem { Content = m.ToFriendlyString(), Tag = m }).ToList();
        }

        private void EngineTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEngineFieldsVisibility();
        }

        private void UpdateEngineFieldsVisibility()
        {
            var selected = (EngineType)((EngineTypeCombo.SelectedItem as ComboBoxItem)?.Tag ?? EngineType.Gasoline);
            bool isElectro = selected == EngineType.Electro;

            GasFieldsPanel.Visibility = isElectro ? Visibility.Collapsed : Visibility.Visible;
            ElectroFieldsPanel.Visibility = isElectro ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TbBrand.Text) || string.IsNullOrWhiteSpace(TbModel.Text))
                {
                    MessageBox.Show("Заповніть Марку та Модель");
                    return;
                }

                if (!int.TryParse(TbYear.Text, out var year) || year < 1900)
                {
                    MessageBox.Show("Невірний рік");
                    return;
                }

                if (!double.TryParse(TbPrice.Text, out var price))
                {
                    MessageBox.Show("Невірна ціна");
                    return;
                }

                if (!int.TryParse(TbMileage.Text, out var mileage))
                {
                    MessageBox.Show("Невірний пробіг");
                    return;
                }

                if (!int.TryParse(TbWeight.Text, out var weight))
                {
                    MessageBox.Show("Невірна вага");
                    return;
                }

                if (!int.TryParse(TbDoors.Text, out var doors) || doors <= 0)
                {
                    MessageBox.Show("Невірна кількість дверей");
                    return;
                }

                if (EngineTypeCombo.SelectedItem is not ComboBoxItem engineItem)
                {
                    MessageBox.Show("Оберіть тип двигуна");
                    return;
                }
                var engineType = (EngineType)engineItem.Tag;

                if (ColorCombo.SelectedItem is not ComboBoxItem colorItem ||
                    DriveTypeCombo.SelectedItem is not ComboBoxItem driveItem ||
                    TransmissionCombo.SelectedItem is not ComboBoxItem transItem ||
                    BodyTypeCombo.SelectedItem is not ComboBoxItem bodyItem)
                {
                    MessageBox.Show("Заповніть параметри авто");
                    return;
                }

                if (!double.TryParse(TbPower.Text, out var power))
                {
                    MessageBox.Show("Невірна потужність");
                    return;
                }

                var engine = new Engine
                {
                    EngineType = engineType,
                    Power = power
                };

                if (engineType == EngineType.Gasoline)
                {
                    if (FuelTypeCombo.SelectedItem is ComboBoxItem fuelItem)
                        engine.FuelType = (FuelType)fuelItem.Tag;
                    if (float.TryParse(TbFuelConsumption.Text, out var fc))
                        engine.FuelConsumption = fc;
                }
                else
                {
                    if (double.TryParse(TbBatteryCapacity.Text, out var battery))
                        engine.BatteryCapacity = battery;
                    if (int.TryParse(TbRange.Text, out var range))
                        engine.Range = range;
                    if (MotorTypeCombo.SelectedItem is ComboBoxItem motorItem)
                        engine.MotorType = (ElectroMotorType)motorItem.Tag;
                }

                _context.Engines.Add(engine);
                _context.SaveChanges();

                var car = new Car
                {
                    CarType = engineType == EngineType.Electro ? CarType.Electro : CarType.Gasoline,
                    Brand = TbBrand.Text.Trim(),
                    ModelName = TbModel.Text.Trim(),
                    EngineId = engine.Id,
                    Color = (Color)colorItem.Tag,
                    Mileage = mileage,
                    Price = (decimal)price,
                    Weight = weight,
                    DriveType = (DriveType)driveItem.Tag,
                    Transmission = (TransmissionType)transItem.Tag,
                    Year = year,
                    NumberOfDoors = doors,
                    BodyType = (CarBodyType)bodyItem.Tag
                };

                _context.Cars.Add(car);
                _context.SaveChanges();

                MessageBox.Show("Автомобіль додано успішно", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            TbBrand.Clear();
            TbModel.Clear();
            TbYear.Clear();
            TbPrice.Clear();
            TbMileage.Clear();
            TbWeight.Clear();
            TbDoors.Clear();
            TbPower.Clear();
            TbFuelConsumption.Clear();
            TbBatteryCapacity.Clear();
            TbRange.Clear();
            EngineTypeCombo.SelectedIndex = 0;
            ColorCombo.SelectedIndex = -1;
            DriveTypeCombo.SelectedIndex = -1;
            TransmissionCombo.SelectedIndex = -1;
            BodyTypeCombo.SelectedIndex = -1;
            FuelTypeCombo.SelectedIndex = -1;
            MotorTypeCombo.SelectedIndex = -1;
        }
    }
}
