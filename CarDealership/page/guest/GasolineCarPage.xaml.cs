
using System.Windows.Forms;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.repo.impl;
using CarDealership.service.impl;

namespace CarDealership.page.guest
{
    public partial class GasolineCarPage 
    {
        private readonly GasolineCarServiceImpl _service;

        public GasolineCarPage()
        {
            InitializeComponent();
            _service = new GasolineCarServiceImpl(new GasolineCarRepository(new DealershipContext()));
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<GasolineCarDto> cars = _service.GetAll().ToList();
                GasolineCarsList.ItemsSource = cars;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}