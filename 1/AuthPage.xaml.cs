using _1;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PaymentApp.Pages
{
    public partial class AuthPage : Page
    {
        private int failedAttempts = 0;

        public AuthPage()
        {
            InitializeComponent();
            Tag = "Авторизация";

            // Плейсхолдеры
            TextBoxLogin.TextChanged += (s, e) =>
                lblHintLogin.Visibility = TextBoxLogin.Text.Length > 0
                    ? Visibility.Hidden : Visibility.Visible;
            PasswordBox.PasswordChanged += (s, e) =>
                lblHintPass.Visibility = PasswordBox.Password.Length > 0
                    ? Visibility.Hidden : Visibility.Visible;

            lblHintLogin.MouseLeftButtonUp += (s, e) => TextBoxLogin.Focus();
            lblHintPass.MouseLeftButtonUp += (s, e) => PasswordBox.Focus();
        }

        // ------- Хэширование -------
        public static string GetHash(string password)
        {
            using (var sha = SHA1.Create())
            {
                return string.Concat(sha.ComputeHash(Encoding.UTF8.GetBytes(password))
                    .Select(x => x.ToString("X2")));
            }
        }

        private void ButtonEnter_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxLogin.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка");
                return;
            }

            if (captcha.Visibility == Visibility.Visible &&
                captchaInput.Text != captcha.Text)
            {
                MessageBox.Show("Неверно введена капча", "Ошибка");
                CaptchaChange();
                return;
            }

            string hashed = GetHash(PasswordBox.Password);

            using (var db = new Entities())
            {
                var user = db.Users.AsNoTracking().FirstOrDefault(u =>
                    u.Login == TextBoxLogin.Text && u.Password == hashed);

                if (user == null)
                {
                    failedAttempts++;
                    MessageBox.Show("Пользователь с такими данными не найден", "Ошибка");

                    if (failedAttempts >= 3)
                    {
                        CaptchaSwitch(true);
                        CaptchaChange();
                    }
                    return;
                }

                MessageBox.Show("Пользователь успешно найден!", "Успех");
                failedAttempts = 0;
                CaptchaSwitch(false);

                switch (user.Role)
                {
                    case "Admin":
                        NavigationService?.Navigate(new AdminPage());
                        break;
                    default:
                        NavigationService?.Navigate(new UserPage());
                        break;
                }
            }
        }

        private void ButtonReg_Click(object sender, RoutedEventArgs e)
            => NavigationService?.Navigate(new RegPage());

        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
            => NavigationService?.Navigate(new ChangePassPage());

        public void CaptchaSwitch(bool show)
        {
            var vis = show ? Visibility.Visible : Visibility.Hidden;
            var inv = show ? Visibility.Hidden : Visibility.Visible;

            captcha.Visibility = vis;
            captchaInput.Visibility = vis;
            labelCaptcha.Visibility = vis;
            submitCaptcha.Visibility = vis;

            TextBoxLogin.Visibility = inv;
            PasswordBox.Visibility = inv;
            ButtonEnter.Visibility = inv;
            ButtonReg.Visibility = inv;
            ButtonChangePassword.Visibility = inv;
            lblHintLogin.Visibility = inv;
            lblHintPass.Visibility = inv;
        }

        public void CaptchaChange()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
            var rnd = new Random();
            captcha.Text = new string(Enumerable.Range(0, 6)
                .Select(_ => chars[rnd.Next(chars.Length)]).ToArray());
        }

        private void submitCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (captchaInput.Text != captcha.Text)
            {
                MessageBox.Show("Неверно введена капча", "Ошибка");
                CaptchaChange();
                return;
            }
            MessageBox.Show("Капча верна. Введите логин и пароль снова.");
            CaptchaSwitch(false);
        }
    }
}