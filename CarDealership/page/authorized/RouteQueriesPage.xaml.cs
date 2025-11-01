using System.Windows;
using CarDealership.page.query;

namespace CarDealership.page.authorized;

public partial class RouteQueriesPage
{
    private readonly string _login;

    public RouteQueriesPage(string login = null)
    {
        InitializeComponent();
        _login = login;
    }

    private void Btn1_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new GetPriceByBrandPage());
    }

    private void Btn2_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new ProductsPage(_login));
    }

    private void Btn3_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new BrandsDelayPage());
    }
    private void BtnMinStock_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new MinStockCarsPage());
    }
}
