using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CarDealership.entity;
using CarDealership.enums;
using Microsoft.EntityFrameworkCore;
using Color = CarDealership.enums.Color;

namespace CarDealership.page.@operator
{
    public partial class EditProductDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public Product ProductToEdit { get; private set; }
        private readonly CarDealership.config.DealershipContext _ctx = new CarDealership.config.DealershipContext();

        public ElectroCar? ElectroCarView =>
            ProductToEdit?.CarType == CarType.Electro ? ProductToEdit.Car as ElectroCar : null;

        public GasolineCar? GasolineCarView =>
            ProductToEdit?.CarType == CarType.Gasoline ? ProductToEdit.Car as GasolineCar : null;

        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }

        public EditProductDialog(Product product)
        {
            InitializeComponent();
            ProductToEdit = product;
            DataContext = this;

            Log("=== EditProductDialog OPENED ===");
            Log($"ProductToEdit.CarType = {ProductToEdit?.CarType}");
            Log($"Car is null? {ProductToEdit?.Car == null}");

            EnsureCarWithEngineLoaded();
            LoadComboBoxData();

            CancelCommand = new CarDealership.dto.RelayCommand<object>(_ =>
            {
                DialogResult = false;
                Close();
            });

            SaveCommand = new CarDealership.dto.RelayCommand<object>(_ =>
            {
                Log("=== SAVE BUTTON CLICKED ===");
                ForceUpdateAllBindings(this);
                UpdateProductFromUI();
                ClearIrrelevantFields();
                DialogResult = true;
                Close();
            });
        }

        private void EnsureCarWithEngineLoaded()
        {
            Log("EnsureCarWithEngineLoaded()");
            if (ProductToEdit?.Car == null)
            {
                Log("ProductToEdit.Car == null, skip load.");
                return;
            }

            if (ProductToEdit.CarType == CarType.Electro)
            {
                var ecar = _ctx.Cars
                    .OfType<ElectroCar>()
                    .Include(c => c.Engine)
                    .FirstOrDefault(c => c.Id == ProductToEdit.Car.Id);
                if (ecar != null)
                {
                    ProductToEdit.Car = ecar;
                    Log($"Loaded ElectroCar with engine: {ecar.Engine?.MotorType}");
                }
                else Log("⚠️ ElectroCar not found in DB!");
            }
            else if (ProductToEdit.CarType == CarType.Gasoline)
            {
                var gcar = _ctx.Cars
                    .OfType<GasolineCar>()
                    .Include(c => c.Engine)
                    .FirstOrDefault(c => c.Id == ProductToEdit.Car.Id);
                if (gcar != null)
                {
                    ProductToEdit.Car = gcar;
                    Log($"Loaded GasolineCar with engine: {gcar.Engine?.FuelType}");
                }
                else Log("⚠️ GasolineCar not found in DB!");
            }

            // 🔄 Оновлюємо DataContext
            OnPropertyChanged(nameof(ProductToEdit));
            OnPropertyChanged(nameof(ElectroCarView));
            OnPropertyChanged(nameof(GasolineCarView));
        }

        private void LoadComboBoxData()
        {
            Log("LoadComboBoxData() started");

            try
            {
                var colorItems = Enum.GetValues(typeof(Color)).Cast<Color>()
                    .Select(c => new ComboBoxItem { Content = c.ToFriendlyString(), Tag = c }).ToList();
                var driveItems = Enum.GetValues(typeof(DriveType)).Cast<DriveType>()
                    .Select(d => new ComboBoxItem { Content = d.ToFriendlyString(), Tag = d }).ToList();
                var transItems = Enum.GetValues(typeof(TransmissionType)).Cast<TransmissionType>()
                    .Select(t => new ComboBoxItem { Content = t.ToFriendlyString(), Tag = t }).ToList();
                var bodyItems = Enum.GetValues(typeof(CarBodyType)).Cast<CarBodyType>()
                    .Select(b => new ComboBoxItem { Content = b.ToFriendlyString(), Tag = b }).ToList();

                var car = ProductToEdit.Car;
                Log($"Car = {(car == null ? "NULL" : "OK")}");

                SetCombo("CarColorComboBox", colorItems, car?.Color);
                SetCombo("CarDriveTypeComboBox", driveItems, car?.DriveType);
                SetCombo("CarTransmissionComboBox", transItems, car?.Transmission);
                SetCombo("CarBodyTypeComboBox", bodyItems, car?.BodyType);

                var motorCombo = this.FindName("ElectroMotorTypeComboBox") as ComboBox;
                if (motorCombo != null)
                {
                    var motorItems = Enum.GetValues(typeof(ElectroMotorType)).Cast<ElectroMotorType>()
                        .Select(m => new ComboBoxItem { Content = m.ToFriendlyString(), Tag = m }).ToList();
                    motorCombo.ItemsSource = motorItems;

                    var selected = ElectroCarView?.Engine?.MotorType;
                    motorCombo.SelectedItem = motorItems.FirstOrDefault(i => Equals(i.Tag, selected));
                    Log($"Motor combo set, selected: {selected}");
                }

                var fuelCombo = this.FindName("FuelTypeComboBox") as ComboBox;
                if (fuelCombo != null)
                {
                    var fuelItems = Enum.GetValues(typeof(FuelType)).Cast<FuelType>()
                        .Select(f => new ComboBoxItem { Content = f.ToFriendlyString(), Tag = f }).ToList();
                    fuelCombo.ItemsSource = fuelItems;

                    var selectedFuel = GasolineCarView?.Engine?.FuelType;
                    fuelCombo.SelectedItem = fuelItems.FirstOrDefault(i => Equals(i.Tag, selectedFuel));
                    Log($"Fuel combo set, selected: {selectedFuel}");
                }
            }
            catch (Exception ex)
            {
                Log($"❌ ERROR in LoadComboBoxData: {ex}");
            }
        }

        private void SetCombo(string name, System.Collections.IEnumerable items, object? selectedValue)
        {
            if (this.FindName(name) is not ComboBox combo)
            {
                Log($"⚠️ ComboBox {name} not found!");
                return;
            }

            combo.ItemsSource = items;
            var selectedItem = items.Cast<ComboBoxItem>().FirstOrDefault(i => Equals(i.Tag, selectedValue));
            combo.SelectedItem = selectedItem;
            Log($"{name} set: Selected = {selectedItem?.Content ?? "null"}");
        }

        private void UpdateProductFromUI()
        {
            Log("UpdateProductFromUI()");
            var car = ProductToEdit.Car;
            if (car == null)
            {
                Log("⚠️ ProductToEdit.Car == null, skip Update.");
                return;
            }

            TryUpdateCombo("CarColorComboBox", val => car.Color = (Color)val);
            TryUpdateCombo("CarDriveTypeComboBox", val => car.DriveType = (DriveType)val);
            TryUpdateCombo("CarTransmissionComboBox", val => car.Transmission = (TransmissionType)val);
            TryUpdateCombo("CarBodyTypeComboBox", val => car.BodyType = (CarBodyType)val);

            if (ProductToEdit.CarType == CarType.Electro && ElectroCarView?.Engine != null)
                TryUpdateCombo("ElectroMotorTypeComboBox", val => ElectroCarView.Engine.MotorType = (ElectroMotorType)val);
            else if (ProductToEdit.CarType == CarType.Gasoline && GasolineCarView?.Engine != null)
                TryUpdateCombo("FuelTypeComboBox", val => GasolineCarView.Engine.FuelType = (FuelType)val);
        }

        private void TryUpdateCombo(string name, Action<object> setter)
        {
            if (this.FindName(name) is ComboBox combo && combo.SelectedItem is ComboBoxItem item)
            {
                setter(item.Tag);
                Log($"Updated {name}: {item.Content}");
            }
            else Log($"⚠️ {name} is null or no selected item");
        }

        private void ClearIrrelevantFields()
        {
            if (ProductToEdit.CarType == CarType.Electro && GasolineCarView?.Engine != null)
            {
                Log("Clearing Gasoline engine fields (Electro active)");
                GasolineCarView.Engine.Power = 0;
                GasolineCarView.Engine.FuelConsumption = 0;
                GasolineCarView.Engine.FuelType = default;
            }
            else if (ProductToEdit.CarType == CarType.Gasoline && ElectroCarView?.Engine != null)
            {
                Log("Clearing Electro engine fields (Gasoline active)");
                ElectroCarView.Engine.BatteryCapacity = 0;
                ElectroCarView.Engine.Power = 0;
                ElectroCarView.Engine.Range = 0;
                ElectroCarView.Engine.MotorType = default;
            }
        }

        private static void ForceUpdateAllBindings(DependencyObject root)
        {
            foreach (var tb in FindVisualChildren<TextBox>(root))
                tb.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

            foreach (var cb in FindVisualChildren<ComboBox>(root))
                cb.GetBindingExpression(ComboBox.SelectedItemProperty)?.UpdateSource();
        }

        private static System.Collections.Generic.IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T t) yield return t;
                foreach (var childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }

        private static void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
