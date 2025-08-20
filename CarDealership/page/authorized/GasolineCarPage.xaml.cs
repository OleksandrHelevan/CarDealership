using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.repo.impl;
using CarDealership.service.impl;

namespace CarDealership.page.authorized
{
    public partial class GasolineCarPage : Page
    {
        private readonly GasolineCarServiceImpl _service;
        private List<GasolineCarDto> _allCars;

        public ICommand BuyCommand { get; }

        public GasolineCarPage()
        {
            InitializeComponent();
            _service = new GasolineCarServiceImpl(new GasolineCarRepository(new DealershipContext()));

            BuyCommand = new RelayCommand<GasolineCarDto>(BuyCar);

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                _allCars = _service.GetAll().ToList();
                GasolineCarsList.ItemsSource = _allCars;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchBox.Text.Trim().ToLower();
            string selectedFilter = (FilterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Всі";

            var filtered = _allCars.Where(c =>
                    (string.IsNullOrEmpty(searchText) ||
                     c.Brand?.ToLower().Contains(searchText) == true ||
                     c.ModelName?.ToLower().Contains(searchText) == true) &&
                    (selectedFilter == "КПП" || 
                     c.TransmissionString == selectedFilter)
            ).ToList();

            GasolineCarsList.ItemsSource = filtered;
        }



        private void BuyCar(GasolineCarDto car)
        {
            MessageBox.Show($"Ви купили {car.Brand} {car.ModelName} за ${car.Price}!", "Успіх");
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is T t) _execute(t);
        }

        public event EventHandler CanExecuteChanged;
    }
}
