
using System.Windows;
using CarDealership.config;
using CarDealership.config.decoder;
using CarDealership.entity;
using MessageBox = System.Windows.MessageBox;

namespace CarDealership.window
{
    public partial class RegisterWindow : Window
    {
        private readonly DealershipContext _context;

        public RegisterWindow()
        {
            InitializeComponent();
            _context = new DealershipContext();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameBox.Text.Trim();
            string lastName = LastNameBox.Text.Trim();
            string passportId = PassportIdBox.Text.Trim();
            string passportData = PassportDataBox.Text.Trim();
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!");
                return;
            }

            if (_context.Users.Any(k => k.Login == login))
            {
                MessageBox.Show("This login is already taken.");
                return;
            }

            // save passport data
            var passport = new PassportData
            {
                FirstName = firstName,
                LastName = lastName,
                PassportNumber = passportId
            };
            _context.PassportData.Add(passport);
            _context.SaveChanges();

            // save keys
            var key = new User
            {
                Login = login,
                Password = DealershipPasswordEncoder.Encode(password)
            };
            _context.Users.Add(key);
            _context.SaveChanges();

            // create client
            var client = new Client
            {
                UserId = key.Id,            // FK to keys
                PassportDataId = passport.Id    // FK to passportData
            };
            _context.Clients.Add(client);
            _context.SaveChanges();

            MessageBox.Show("Registration successful!");
            this.Close();
        }
        
    }
}
