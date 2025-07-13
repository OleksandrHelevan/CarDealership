using CarDealership.config;
using CarDealership.entity;

namespace CarDealership;

using System.Collections.Generic;
using System.Linq;
using System.Windows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        using var context = new DealershipContext();
        List<GasolineEngineEntity> engines = context.GasolineEnginesEntity.ToList();
        EnginesDataGrid.ItemsSource = engines;
    }
}
