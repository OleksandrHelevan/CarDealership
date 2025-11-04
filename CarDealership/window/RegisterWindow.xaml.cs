using System.Windows;
using System.Linq;
using System.Text.RegularExpressions;
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
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || !firstName.All(char.IsLetter))
            {
                MessageBox.Show("Ім'я має містити щонайменше 2 літери та тільки літери.");
                return;
            }

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || !lastName.All(char.IsLetter))
            {
                MessageBox.Show("Прізвище має містити щонайменше 2 літери та тільки літери.");
                return;
            }

            if (string.IsNullOrWhiteSpace(passportNumber) || !Regex.IsMatch(passportNumber, "^\\d{13}$"))
            {
                MessageBox.Show("Номер паспорта має складатися з 13 цифр.");
                return;
            }

            if (string.IsNullOrWhiteSpace(login) || !Regex.IsMatch(login, "^\\d{13}$"))
            {
                MessageBox.Show("Логін має бути номером паспорта з 13 цифр.");
                return;
            }

            if (!string.Equals(login, passportNumber))
            {
                MessageBox.Show("Логін має збігатися з номером паспорта (13 цифр).");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Підтвердження пароля не збігається з паролем.");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Email є обов'язковим.");
                return;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Введіть коректну email-адресу.");
                return;
            }

            bool hasLetter = password.Any(char.IsLetter);
            bool hasDigit = password.Any(char.IsDigit);
            if (password.Length <= 8 || !hasLetter || !hasDigit)
            {
                MessageBox.Show("Пароль має бути довшим за 8 символів та містити літери й цифри.");
                return;
            }

            if (_context.Users.Any(k => k.Login == login))
            {
                MessageBox.Show("Такий логін уже зайнятий.");
                return;
            }

            if (_context.Users.Any(k => k.Email == email))
            {
                MessageBox.Show("Такий email уже зареєстровано.");
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
                Email = email,
                Password = DealershipPasswordEncoder.Encode(password),
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