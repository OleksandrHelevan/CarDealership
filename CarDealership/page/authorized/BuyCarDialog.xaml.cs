using System.Windows;
using System.Windows.Controls;
using CarDealership.dto;
using CarDealership.enums;

namespace CarDealership.page.authorized
{
    public partial class BuyCarDialog : Window
    {
        public BuyCarDto BuyCarDto { get; private set; }

        public BuyCarDialog()
        {
            InitializeComponent();
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(ClientIdTextBox.Text, out int clientId))
                {
                    MessageBox.Show("Будь ласка, введіть правильний ID клієнта", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var paymentType = GetPaymentType();
                
                BuyCarDto = new BuyCarDto
                {
                    ClientId = clientId,
                    PaymentType = paymentType,
                    Delivery = DeliveryCheckBox.IsChecked ?? false,
                    CountryOfOrigin = CountryTextBox.Text.Trim()
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
