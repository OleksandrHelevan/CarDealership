using CarDealership.config;
using CarDealership.entity;
using CarDealership.model;
using CarDealership.service.impl;

namespace CarDealership;

using System.Collections.Generic;
using System.Linq;
using System.Windows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        try
        {
            LoadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading data: " + ex.Message);
            // Можна залогувати виняток або в консолі
            Console.WriteLine(ex);
        }
    }

    private void LoadData()
    {
        GasolineEngineServiceImpl service = new GasolineEngineServiceImpl();
        List<GasolineEngineDto> engines = service.GetAllGasolineEngines();
        EnginesDataGrid.ItemsSource = engines;
    }
}
