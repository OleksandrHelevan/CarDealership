using CarDealership.service.impl;
using CarDealership.dto;
using System.Windows.Controls;

namespace CarDealership.page;

public partial class GasolineEnginePage : Page
{
    public GasolineEnginePage()
    {
        InitializeComponent();
        try
        {
            LoadData();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show("Error loading data: " + ex.Message);
        }
    }

    private void LoadData()
    {
        GasolineEngineServiceImpl service = new GasolineEngineServiceImpl();
        List<GasolineEngineDto> engines = service.GetAllGasolineEngines();
        GasolineEnginesList.ItemsSource = engines;
    }
}