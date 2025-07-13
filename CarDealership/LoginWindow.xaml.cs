using System.Windows;

namespace CarDealership
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (username == "admin" && password == "1234")
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Невірні дані для входу");
            }
        }
    }
}