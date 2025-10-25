using System.Windows;

namespace CarDealership.page.common;

public partial class SimplePromptDialog : Window
{
    public string? ResultText { get; private set; }

    public SimplePromptDialog(string prompt)
    {
        InitializeComponent();
        PromptText.Text = prompt;
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        ResultText = InputBox.Text?.Trim();
        if (string.IsNullOrEmpty(ResultText))
        {
            MessageBox.Show("Please enter a value.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        DialogResult = true;
        Close();
    }
}

