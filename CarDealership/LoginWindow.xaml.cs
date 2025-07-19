using System.Windows;

namespace CarDealership.page
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (UsernameTextBox.Text == "admin" && PasswordBox.Password == "1234")
            {
                this.DialogResult = true; // достатньо
            }
            else
            {
                MessageBox.Show("Невірні дані для входу");
            }
        }

    }
}