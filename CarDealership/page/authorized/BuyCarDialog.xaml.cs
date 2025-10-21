using System.Windows;
using System.Windows.Controls;
using CarDealership.config;
using CarDealership.dto;
using CarDealership.enums;

namespace CarDealership.page.authorized
{
    public partial class BuyCarDialog : Window
    {
        
        private DealershipContext _context;
        
        public BuyCarDto BuyCarDto { get; private set; }

        public BuyCarDialog()
        {
            InitializeComponent();
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var paymentType = GetPaymentType();

                BuyCarDto = new BuyCarDto
                {
                    PaymentType = paymentType,
                    Delivery = DeliveryCheckBox.IsChecked ?? false
                };

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private PaymentType GetPaymentType()
        {
            var selectedItem = PaymentTypeComboBox.SelectedItem as ComboBoxItem;
            return selectedItem?.Content.ToString() switch
            {
                "Готівка" => PaymentType.Cash,
                "Кредитна карта" => PaymentType.Card,
                "Банківський переказ" => PaymentType.Credit,
                _ => PaymentType.Cash
            };
        }
    }
}
