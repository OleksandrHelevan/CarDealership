using System.Windows;
using CarDealership.enums;

namespace CarDealership.page.@operator;

public partial class OrderDecisionDialog : Window
{
    public bool IsApproved { get; private set; }
    public string? Reason { get; private set; }
    public string? CardNumber { get; private set; }

    private readonly PaymentType _paymentType;

    public OrderDecisionDialog(PaymentType paymentType)
    {
        InitializeComponent();
        _paymentType = paymentType;

        RejectRadio.Checked += (_, __) => ReasonBox.Visibility = Visibility.Visible;
        RejectRadio.Unchecked += (_, __) => ReasonBox.Visibility = Visibility.Collapsed;

        bool needsCard = _paymentType == PaymentType.Card;
        CardLabel.Visibility = needsCard ? Visibility.Visible : Visibility.Collapsed;
        CardBox.Visibility = needsCard ? Visibility.Visible : Visibility.Collapsed;
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        IsApproved = ApproveRadio.IsChecked == true;
        if (!IsApproved)
        {
            Reason = ReasonBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(Reason))
            {
                MessageBox.Show("Введіть причину відхилення.", "Перевірка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        if (_paymentType == PaymentType.Card)
        {
            CardNumber = CardBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(CardNumber))
            {
                MessageBox.Show("Введіть номер картки.", "Перевірка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        DialogResult = true;
        Close();
    }
}
