using System.Windows;
using CarDealership.page.query;

namespace CarDealership.page.authorized;

public partial class RoureQueriesPage
{
    public RoureQueriesPage()
    {
        InitializeComponent();
    }

    private void Btn1_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new GetPriceByBrandPage());
    }
}

