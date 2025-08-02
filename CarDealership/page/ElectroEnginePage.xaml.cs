using System.Collections.Generic;
using System.Windows.Controls;
using CarDealership.dto;
using CarDealership.service.impl;

namespace CarDealership.page
{
    public partial class ElectroEnginePage : Page
    {
        public ElectroEnginePage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            ElectroEngineServiceImpl service = new ElectroEngineServiceImpl();
            List<ElectroEngineDto> engines = service.GetAllElectroEngines();
            ElectroEnginesList.ItemsSource = engines;
        }
    }
}