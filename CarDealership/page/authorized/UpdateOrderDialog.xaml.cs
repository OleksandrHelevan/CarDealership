
using System.Windows;
using CarDealership.config;
using CarDealership.enums;
using CarDealership.entity;

namespace CarDealership.window
{
    public partial class UpdateOrderDialog : Window
    {
        private readonly DealershipContext _context;
        private readonly int _productId;
        private Order? _order;

        public UpdateOrderDialog(int productId)
        {
            InitializeComponent();
            _context = new DealershipContext();
            _productId = productId;

            LoadOrder();
        }

        private void LoadOrder()
        {
            _order = _context.Orders.FirstOrDefault(o => o.ProductId == _productId);
            if (_order == null)
            {
                MessageBox.Show("Замовлення не знайдено!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            PaymentTypeComboBox.SelectedIndex = (int)_order.PaymentType;
            DeliveryCheckBox.IsChecked = _order.DeliveryRequired;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_order == null) return;

            var selectedItem = PaymentTypeComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem;
            if (selectedItem?.Tag is string tagStr && int.TryParse(tagStr, out int tag))
                _order.PaymentType = (PaymentType)tag;
            else if (selectedItem?.Tag is int tagInt)
                _order.PaymentType = (PaymentType)tagInt;

            _order.DeliveryRequired = DeliveryCheckBox.IsChecked ?? false;
            _order.OrderDate = DateTime.UtcNow; 

            _context.SaveChanges();
            MessageBox.Show("Замовлення оновлено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
