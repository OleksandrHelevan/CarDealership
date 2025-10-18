
using System.Windows;
using System.Windows.Controls;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.page.@operator
{
    public partial class EditProductDialog : Window
    {
        public Product ProductToEdit { get; private set; }

        public EditProductDialog(Product product)
        {
            InitializeComponent();
            ProductToEdit = product;
            DataContext = this;
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            if (ProductToEdit.ElectroCar != null)
            {
                var electroColorItems = Enum.GetValues(typeof(Color)).Cast<Color>()
                    .Select(c => new ComboBoxItem { Content = c.ToFriendlyString(), Tag = c }).ToList();
                ElectroCarColorComboBox.ItemsSource = electroColorItems;
                ElectroCarColorComboBox.SelectedItem = electroColorItems.FirstOrDefault(item => item.Tag.Equals(ProductToEdit.ElectroCar.Color));

                var electroDriveItems = Enum.GetValues(typeof(DriveType)).Cast<DriveType>()
                    .Select(d => new ComboBoxItem { Content = d.ToFriendlyString(), Tag = d }).ToList();
                ElectroCarDriveTypeComboBox.ItemsSource = electroDriveItems;
                ElectroCarDriveTypeComboBox.SelectedItem = electroDriveItems.FirstOrDefault(item => item.Tag.Equals(ProductToEdit.ElectroCar.DriveType));
                
                var electroTransItems = Enum.GetValues(typeof(TransmissionType)).Cast<TransmissionType>()
                    .Select(t => new ComboBoxItem { Content = t.ToFriendlyString(), Tag = t }).ToList();
                ElectroCarTransmissionComboBox.ItemsSource = electroTransItems;
                ElectroCarTransmissionComboBox.SelectedItem = electroTransItems.FirstOrDefault(item => item.Tag.Equals(ProductToEdit.ElectroCar.Transmission));
                
                var electroBodyItems = Enum.GetValues(typeof(CarBodyType)).Cast<CarBodyType>()
                    .Select(b => new ComboBoxItem { Content = b.ToFriendlyString(), Tag = b }).ToList();
                ElectroCarBodyTypeComboBox.ItemsSource = electroBodyItems;
                ElectroCarBodyTypeComboBox.SelectedItem = electroBodyItems.FirstOrDefault(item => item.Tag.Equals(ProductToEdit.ElectroCar.BodyType));
                
                var motorItems = Enum.GetValues(typeof(ElectroMotorType)).Cast<ElectroMotorType>()
                    .Select(m => new ComboBoxItem { Content = m.ToFriendlyString(), Tag = m }).ToList();
                ElectroMotorTypeComboBox.ItemsSource = motorItems;
                ElectroMotorTypeComboBox.SelectedItem = motorItems.FirstOrDefault(item => item.Tag.Equals(ProductToEdit.ElectroCar.Engine.MotorType));
            }
            
            if (ProductToEdit.GasolineCar != null)
            {
                var gasolineColorItems = Enum.GetValues(typeof(Color)).Cast<Color>()
                    .Select(c => new ComboBoxItem { Content = c.ToFriendlyString(), Tag = c }).ToList();
                GasolineCarColorComboBox.ItemsSource = gasolineColorItems;
                GasolineCarColorComboBox.SelectedItem = gasolineColorItems.FirstOrDefault(item => item.Tag.Equals(ProductToEdit.GasolineCar.Color));

                var gasolineDriveItems = Enum.GetValues(typeof(DriveType)).Cast<DriveType>()
                    .Select(d => new ComboBoxItem { Content = d.ToFriendlyString(), Tag = d }).ToList();
                GasolineCarDriveTypeComboBox.ItemsSource = gasolineDriveItems;
                GasolineCarDriveTypeComboBox.SelectedItem = gasolineDriveItems.FirstOrDefault(item => item.Tag.Equals(ProductToEdit.GasolineCar.DriveType));
                
                var gasolineTransItems = Enum.GetValues(typeof(TransmissionType)).Cast<TransmissionType>()
                    .Select(t => new ComboBoxItem { Content = t.ToFriendlyString(), Tag = t }).ToList();
                GasolineCarTransmissionComboBox.ItemsSource = gasolineTransItems;
                GasolineCarTransmissionComboBox.SelectedItem = gasolineTransItems.FirstOrDefault(item => item.Tag.Equals(ProductToEdit.GasolineCar.Transmission));
                
                var gasolineBodyItems = Enum.GetValues(typeof(CarBodyType)).Cast<CarBodyType>()
                    .Select(b => new ComboBoxItem { Content = b.ToFriendlyString(), Tag = b }).ToList();
                GasolineCarBodyTypeComboBox.ItemsSource = gasolineBodyItems;
                GasolineCarBodyTypeComboBox.SelectedItem = gasolineBodyItems.FirstOrDefault(item => item.Tag.Equals(ProductToEdit.GasolineCar.BodyType));
                
                var fuelItems = Enum.GetValues(typeof(FuelType)).Cast<FuelType>()
                    .Select(f => new ComboBoxItem { Content = f.ToFriendlyString(), Tag = f }).ToList();
                FuelTypeComboBox.ItemsSource = fuelItems;
                FuelTypeComboBox.SelectedItem = fuelItems.FirstOrDefault(item => item.Tag.Equals(ProductToEdit.GasolineCar.Engine.FuelType));
            }
        }
        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            UpdateProductFromUI();
            DialogResult = true;
            Close();
        }
        
        private void UpdateProductFromUI()
        {
            if (ProductToEdit.ElectroCar != null)
            {
                if (ElectroCarColorComboBox.SelectedItem is ComboBoxItem colorItem) ProductToEdit.ElectroCar.Color = (Color)colorItem.Tag;
                if (ElectroCarDriveTypeComboBox.SelectedItem is ComboBoxItem driveItem) ProductToEdit.ElectroCar.DriveType = (DriveType)driveItem.Tag;
                if (ElectroCarTransmissionComboBox.SelectedItem is ComboBoxItem transItem) ProductToEdit.ElectroCar.Transmission = (TransmissionType)transItem.Tag;
                if (ElectroCarBodyTypeComboBox.SelectedItem is ComboBoxItem bodyItem) ProductToEdit.ElectroCar.BodyType = (CarBodyType)bodyItem.Tag;
                if (ElectroMotorTypeComboBox.SelectedItem is ComboBoxItem motorItem) ProductToEdit.ElectroCar.Engine.MotorType = (ElectroMotorType)motorItem.Tag;
            }
            
            if (ProductToEdit.GasolineCar != null)
            {
                if (GasolineCarColorComboBox.SelectedItem is ComboBoxItem colorItem) ProductToEdit.GasolineCar.Color = (Color)colorItem.Tag;
                if (GasolineCarDriveTypeComboBox.SelectedItem is ComboBoxItem driveItem) ProductToEdit.GasolineCar.DriveType = (DriveType)driveItem.Tag;
                if (GasolineCarTransmissionComboBox.SelectedItem is ComboBoxItem transItem) ProductToEdit.GasolineCar.Transmission = (TransmissionType)transItem.Tag;
                if (GasolineCarBodyTypeComboBox.SelectedItem is ComboBoxItem bodyItem) ProductToEdit.GasolineCar.BodyType = (CarBodyType)bodyItem.Tag;
                if (FuelTypeComboBox.SelectedItem is ComboBoxItem fuelItem) ProductToEdit.GasolineCar.Engine.FuelType = (FuelType)fuelItem.Tag;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
