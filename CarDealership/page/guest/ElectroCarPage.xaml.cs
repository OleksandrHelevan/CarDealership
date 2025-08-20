using System.Windows.Controls;
using System.Windows.Forms;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.repo.impl;
using CarDealership.service.impl;

namespace CarDealership.page.guest
{
    public partial class ElectroCarPage : Page
    {
        private readonly ElectroCarServiceImpl _service;

        public ElectroCarPage()
        {
            InitializeComponent();
            _service = new ElectroCarServiceImpl(new ElectroCarRepository(new DealershipContext()));
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<ElectroCarDto> cars = _service.GetAllCars().ToList();
                foreach (ElectroCarDto car in cars)
                    ElectroCarsList.ItemsSource = cars;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}