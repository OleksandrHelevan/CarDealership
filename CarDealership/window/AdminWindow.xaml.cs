using System.Windows;
using CarDealership.config.decoder;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.exception;
using CarDealership.page.admin;
using CarDealership.repo.impl;
using CarDealership.service.impl;

namespace CarDealership.window;

public partial class AdminWindow : Window
{
    private readonly string _currentLogin;
    public AdminWindow(String login)
    {
        InitializeComponent();
        _currentLogin = login;
    }

    private void BtnAddOperator_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new AddOperatorPage());
    }
    
}