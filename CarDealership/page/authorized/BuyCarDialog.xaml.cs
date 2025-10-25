using System.Linq;
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
                var paymentType = GetPaymentType();

                var phone = PhoneBox.Text?.Trim();
                if (string.IsNullOrWhiteSpace(phone) || !IsValidPhone(phone))
                    throw new System.Exception("Введіть коректний номер телефону (цифри, +, пробіли, (), -).");

                var delivery = DeliveryCheckBox.IsChecked ?? false;
                string? address = null;
                if (delivery)
                {
                    address = AddressBox.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(address))
                        throw new System.Exception("Адреса обов’язкова, якщо вибрано доставку.");
                }

                BuyCarDto = new BuyCarDto
                {
                    PaymentType = paymentType,
                    Delivery = delivery,
                    Address = address,
                    PhoneNumber = phone!
                };

                DialogResult = true;
                Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Помилка покупки: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                "Картка" => PaymentType.Card,
                "Кредит" => PaymentType.Credit,
                _ => PaymentType.Cash
            };
        }

        private bool IsValidPhone(string value)
        {
            var cleaned = new string(value.Where(char.IsDigit).ToArray());
            return cleaned.Length >= 7 && cleaned.Length <= 20;
        }
    }
}
