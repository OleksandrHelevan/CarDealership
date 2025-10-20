using System;
using System.Windows;
using CarDealership.dto;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.page.@operator;

public partial class PutOnSaleDialog
{
    private readonly Vehicle _vehicle;
    public Product? CreatedProduct { get; private set; }

    public PutOnSaleDialog(Vehicle vehicle)
    {
        InitializeComponent();
        _vehicle = vehicle;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TbNumber.Text))
        {
            MessageBox.Show("Введіть номер продукту");
            return;
        }

        var product = new Product
        {
            ProductNumber = TbNumber.Text.Trim(),
            CountryOfOrigin = string.IsNullOrWhiteSpace(TbCountry.Text) ? "Ukraine" : TbCountry.Text.Trim(),
            InStock = CbInStock.IsChecked == true,
            AvailableFrom = DateTime.UtcNow
        };

        if (_vehicle is GasolineCarDto g)
        {
            product.CarId = g.Id;
            product.CarType = CarType.Gasoline;
        }
        else if (_vehicle is ElectroCarDto ecar)
        {
            product.CarId = ecar.Id;
            product.CarType = CarType.Electro;
        }
        else
        {
            MessageBox.Show("Невідомий тип авто");
            return;
        }

        CreatedProduct = product;
        DialogResult = true;
        Close();
    }
}


