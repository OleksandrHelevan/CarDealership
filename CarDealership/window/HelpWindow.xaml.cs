using System;
using System.IO;
using System.Windows;
using CarDealership.enums;

namespace CarDealership.window
{
    public partial class HelpWindow : Window
    {
        private readonly AccessRight _accessRight;

        public HelpWindow(AccessRight accessRight)
        {
            InitializeComponent();
            _accessRight = accessRight;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TitleBlock.Text = $"Довідка — { _accessRight.ToFriendlyString() }";

            try
            {
                var helpDir = Path.Combine(AppContext.BaseDirectory, "templates\\help");
                var fileName = _accessRight switch
                {
                    AccessRight.Admin => "admin.txt",
                    AccessRight.Operator => "operator.txt",
                    AccessRight.Authorized => "authorized.txt",
                    _ => "guest.txt"
                };            
                var path = Path.Combine(helpDir, fileName);

                if (File.Exists(path))
                {
                    HelpTextBox.Text = File.ReadAllText(path);
                }
                else
                {
                    HelpTextBox.Text = $"Файл довідки не знайдено: {path}";
                }
            }
            catch (Exception ex)
            {
                HelpTextBox.Text = $"Помилка завантаження довідки: {ex.Message}";
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
