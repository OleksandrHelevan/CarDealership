
using System.Windows;
using CarDealership.config;
using CarDealership.config.decoder;
using CarDealership.entity;
using CarDealership.enums;
using MessageBox = System.Windows.MessageBox;

namespace CarDealership.window
{
    public partial class RegisterWindow : Window
    {
        private readonly DealershipContext _context;

        private AccessRight _accessRight;
        public RegisterWindow(AccessRight accessRight)
        {
            InitializeComponent();
            _context = new DealershipContext();
            _accessRight = accessRight;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameBox.Text.Trim();
            string lastName = LastNameBox.Text.Trim();
            string passportNumber = PassportNumberBox.Text.Trim();
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

            var passport = new PassportData
            {
                FirstName = firstName,
                LastName = lastName,
                PassportNumber = passportNumber
            };
            _context.PassportData.Add(passport);
            _context.SaveChanges();

            var key = new User
            {
                Login = login,
                PasswordHash = DealershipPasswordEncoder.Encode(password),
                AccessRight = _accessRight
            };
            _context.Users.Add(key);
            _context.SaveChanges();

            var client = new Client
            {
                UserId = key.Id,
                PassportDataId = passport.Id
            };
            
            _context.Clients.Add(client);
            _context.SaveChanges();

            MessageBox.Show("Registration successful!");
            Close();
        }
        
    }
}
