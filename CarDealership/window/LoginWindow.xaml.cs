using System.Windows;
using CarDealership.enums;
using CarDealership.dto;
using CarDealership.exception;
using CarDealership.service;
using CarDealership.service.impl;

namespace CarDealership.window
{
    public partial class LoginWindow
    {
        private readonly IUserService _userService;

        public LoginWindow()
        {
            InitializeComponent();
            _userService = new UserServiceImpl();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Логін та пароль обов'язкові.", "Валідація");
                return;
            }

            try
            {
                UserDto user = _userService.Login(login, password)!;

                MessageBox.Show(
                    $"Вітаємо, {user.Login} (права доступу - {user.AccessRight.ToFriendlyString()})!",
                    "Успішний вхід");

                if (user.AccessRight == AccessRight.Guest)
                {
                    GuestWindow guestWindow = new GuestWindow(user.Login);
                    guestWindow.Show();

                    DialogResult = true;
                    Close();
                }
                else if (user.AccessRight == AccessRight.Authorized)
                {
                    AuthorizedWindow authorizedWindow = new AuthorizedWindow(user.Login);
                    authorizedWindow.Show();

                    DialogResult = true;
                    Close();
                }
                else if (user.AccessRight == AccessRight.Operator)
                {
                    OperatorWindow operatorWindow = new OperatorWindow(user.Login);
                    operatorWindow.Show();

                    DialogResult = true;
                    Close();
                }
                else if (user.AccessRight == AccessRight.Admin)
                {
                    AdminWindow adminWindow = new AdminWindow(user.Login);
                    adminWindow.Show();

                    DialogResult = true;
                    Close();
                }
            }
            catch (UserNotFoundException)
            {
                MessageBox.Show("Користувача з таким логіном не знайдено.", "Помилка входу");
            }
            catch (InvalidPasswordException)
            {
                MessageBox.Show("Невірний пароль.", "Помилка входу");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непередбачена помилка: {ex.Message}", "Помилка входу");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(AccessRight.Guest);
            registerWindow.ShowDialog();
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            // Використати логін для пошуку email в БД
            var login = UsernameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Введіть логін.", "Відновлення пароля");
                return;
            }

            var user = _userService.LoadByUsername(login);
            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                MessageBox.Show("Користувача або email не знайдено.", "Відновлення пароля");
                return;
            }

            var email = user.Email;

            // Згенерувати код і відправити HTML-лист
            var code = util.RandomCodeGenerator.GenerateNumericCode(6);
            var templatePath = System.IO.Path.Combine(AppContext.BaseDirectory, "templates", "reset_code_email.html");
            var html = util.EmailTemplateRenderer.RenderResetCodeTemplate(templatePath, code);

            try
            {
                var emailService = new service.impl.EmailServiceImpl();
                emailService.SendHtmlEmail(email, "Код підтвердження — CarDealership", html);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося надіслати email: {ex.Message}", "Відновлення пароля");
                return;
            }

            // Запросити код та перевірити
            var codeDialog = new page.common.SimplePromptDialog("Введіть код з email:");
            codeDialog.Owner = this;
            var codeOk = codeDialog.ShowDialog();
            if (codeOk != true) return;

            var inputCode = codeDialog.ResultText!;
            if (!string.Equals(inputCode, code, StringComparison.Ordinal))
            {
                MessageBox.Show("Код не співпадає.", "Відновлення пароля");
                return;
            }

            // Відкрити ResetPasswordWindow
            var resetWindow = new ResetPasswordWindow(_userService, login);
            resetWindow.Owner = this;
            resetWindow.ShowDialog();
        }
    }
}
