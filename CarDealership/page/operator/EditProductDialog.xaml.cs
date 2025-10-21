using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.page.@operator
{
    public partial class EditProductDialog : Window
    {
        public Product ProductToEdit { get; }

        public EditProductDialog(Product product)
        {
            InitializeComponent();
            ProductToEdit = product;

            LoadEnums();
            LoadDefaults();
        }

        private void LoadEnums()
        {
            ColorCombo.ItemsSource = Enum.GetValues(typeof(Color)).Cast<Color>()
                .Select(c => new ComboBoxItem { Content = c.ToFriendlyString(), Tag = c }).ToList();
            DriveTypeCombo.ItemsSource = Enum.GetValues(typeof(DriveType)).Cast<DriveType>()
                .Select(d => new ComboBoxItem { Content = d.ToFriendlyString(), Tag = d }).ToList();
            TransmissionCombo.ItemsSource = Enum.GetValues(typeof(TransmissionType)).Cast<TransmissionType>()
                .Select(t => new ComboBoxItem { Content = t.ToFriendlyString(), Tag = t }).ToList();
            BodyTypeCombo.ItemsSource = Enum.GetValues(typeof(CarBodyType)).Cast<CarBodyType>()
                .Select(b => new ComboBoxItem { Content = b.ToFriendlyString(), Tag = b }).ToList();

            EngineTypeCombo.ItemsSource = Enum.GetValues(typeof(EngineType)).Cast<EngineType>().ToList();
            FuelTypeCombo.ItemsSource = Enum.GetValues(typeof(FuelType)).Cast<FuelType>()
                .Select(f => new ComboBoxItem { Content = f.ToFriendlyString(), Tag = f }).ToList();
            MotorTypeCombo.ItemsSource = Enum.GetValues(typeof(ElectroMotorType)).Cast<ElectroMotorType>()
                .Select(m => new ComboBoxItem { Content = m.ToFriendlyString(), Tag = m }).ToList();
        }

        private void LoadDefaults()
        {
            // Product fields
            TbNumber.Text = ProductToEdit.Number;
            TbCountry.Text = ProductToEdit.CountryOfOrigin;
            CbInStock.IsChecked = ProductToEdit.InStock;
            DpAvailableFrom.SelectedDate = ProductToEdit.AvailableFrom;

            // Car fields
            var car = ProductToEdit.Car;
            if (car == null) return;

            TbBrand.Text = car.Brand;
            TbModel.Text = car.ModelName;
            TbYear.Text = car.Year.ToString();
            TbPrice.Text = car.Price.ToString();
            TbMileage.Text = car.Mileage.ToString();
            TbWeight.Text = car.Weight.ToString();
            TbDoors.Text = car.NumberOfDoors.ToString();

            ColorCombo.SelectedItem = ((System.Collections.IEnumerable)ColorCombo.Items)
                .Cast<ComboBoxItem>().FirstOrDefault(i => (Color)i.Tag == car.Color);
            DriveTypeCombo.SelectedItem = ((System.Collections.IEnumerable)DriveTypeCombo.Items)
                .Cast<ComboBoxItem>().FirstOrDefault(i => (DriveType)i.Tag == car.DriveType);
            TransmissionCombo.SelectedItem = ((System.Collections.IEnumerable)TransmissionCombo.Items)
                .Cast<ComboBoxItem>().FirstOrDefault(i => (TransmissionType)i.Tag == car.Transmission);
            BodyTypeCombo.SelectedItem = ((System.Collections.IEnumerable)BodyTypeCombo.Items)
                .Cast<ComboBoxItem>().FirstOrDefault(i => (CarBodyType)i.Tag == car.BodyType);

            // Engine
            var engine = car.Engine;
            if (engine == null) return;

            EngineTypeCombo.SelectedItem = engine.EngineType;
            TbPower.Text = engine.Power.ToString();

            ToggleEnginePanels();

            FuelTypeCombo.SelectedItem = ((System.Collections.IEnumerable)FuelTypeCombo.Items)
                .Cast<ComboBoxItem>().FirstOrDefault(i => (FuelType)i.Tag == engine.FuelType);
            TbFuelConsumption.Text = engine.FuelConsumption?.ToString() ?? string.Empty;

            TbBatteryCapacity.Text = engine.BatteryCapacity?.ToString() ?? string.Empty;
            TbRange.Text = engine.Range?.ToString() ?? string.Empty;
            MotorTypeCombo.SelectedItem = ((System.Collections.IEnumerable)MotorTypeCombo.Items)
                .Cast<ComboBoxItem>().FirstOrDefault(i => (ElectroMotorType)i.Tag == engine.MotorType);
        }

        private void ToggleEnginePanels()
        {
            var et = EngineTypeCombo.SelectedItem is EngineType t ? t : EngineType.Gasoline;
            bool isElectro = et == EngineType.Electro;
            GasFieldsPanel.Visibility = isElectro ? Visibility.Collapsed : Visibility.Visible;
            ElectroFieldsPanel.Visibility = isElectro ? Visibility.Visible : Visibility.Collapsed;
        }

        private void EngineTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToggleEnginePanels();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // product
                ProductToEdit.Number = TbNumber.Text.Trim();
                ProductToEdit.CountryOfOrigin = TbCountry.Text.Trim();
                ProductToEdit.InStock = CbInStock.IsChecked == true;
                ProductToEdit.AvailableFrom = DpAvailableFrom.SelectedDate.HasValue
                    ? DateTime.SpecifyKind(DpAvailableFrom.SelectedDate.Value, DateTimeKind.Utc)
                    : null;

                var car = ProductToEdit.Car;
                if (car == null) { DialogResult = false; return; }

                car.Brand = TbBrand.Text.Trim();
                car.ModelName = TbModel.Text.Trim();
                car.Year = int.TryParse(TbYear.Text, out var year) ? year : car.Year;
                if (decimal.TryParse(TbPrice.Text, out var price)) car.Price = price;
                if (int.TryParse(TbMileage.Text, out var mileage)) car.Mileage = mileage;
                if (int.TryParse(TbWeight.Text, out var weight)) car.Weight = weight;
                if (int.TryParse(TbDoors.Text, out var doors)) car.NumberOfDoors = doors;

                if (ColorCombo.SelectedItem is ComboBoxItem colorItem) car.Color = (Color)colorItem.Tag;
                if (DriveTypeCombo.SelectedItem is ComboBoxItem driveItem) car.DriveType = (DriveType)driveItem.Tag;
                if (TransmissionCombo.SelectedItem is ComboBoxItem transItem) car.Transmission = (TransmissionType)transItem.Tag;
                if (BodyTypeCombo.SelectedItem is ComboBoxItem bodyItem) car.BodyType = (CarBodyType)bodyItem.Tag;

                // Engine
                var engine = car.Engine ?? new Engine();
                car.Engine = engine;

                engine.EngineType = EngineTypeCombo.SelectedItem is EngineType et ? et : engine.EngineType;
                if (double.TryParse(TbPower.Text, out var power)) engine.Power = power;

                if (engine.EngineType == EngineType.Gasoline)
                {
                    engine.BatteryCapacity = null;
                    engine.Range = null;
                    engine.MotorType = null;

                    if (FuelTypeCombo.SelectedItem is ComboBoxItem fuelItem) engine.FuelType = (FuelType)fuelItem.Tag;
                    if (float.TryParse(TbFuelConsumption.Text, out var fc)) engine.FuelConsumption = fc; else engine.FuelConsumption = null;
                }
                else
                {
                    engine.FuelType = null;
                    engine.FuelConsumption = null;

                    if (double.TryParse(TbBatteryCapacity.Text, out var bat)) engine.BatteryCapacity = bat; else engine.BatteryCapacity = null;
                    if (int.TryParse(TbRange.Text, out var range)) engine.Range = range; else engine.Range = null;
                    if (MotorTypeCombo.SelectedItem is ComboBoxItem motorItem) engine.MotorType = (ElectroMotorType)motorItem.Tag; else engine.MotorType = null;
                }

                car.CarType = engine.EngineType == EngineType.Electro ? CarType.Electro : CarType.Gasoline;

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
