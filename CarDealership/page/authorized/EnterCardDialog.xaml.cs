using System.Linq;
using System.Windows;

namespace CarDealership.page.authorized;

public partial class EnterCardDialog : Window
{
    public string? CardNumber { get; private set; }

    public EnterCardDialog()
    {
        InitializeComponent();
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        var card = CardBox.Text?.Trim();
        var digits = new string((card ?? string.Empty).Where(char.IsDigit).ToArray());
        if (string.IsNullOrWhiteSpace(card) || digits.Length < 12)
        {
            MessageBox.Show("Введіть коректний номер картки.", "Перевірка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        CardNumber = card;
        DialogResult = true;
        Close();
    }
}

