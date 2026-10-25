using _1;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PaymentApp.Pages
{
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
            Tag = "Регистрация";
            comboBxRole.SelectedIndex = 0;
        }

        private void txtbxLog_TextChanged(object sender, TextChangedEventArgs e) { }

        public static string GetHash(string password)
        {
            using (var sha = SHA1.Create())
            {
                return string.Concat(sha.ComputeHash(Encoding.UTF8.GetBytes(password))
                    .Select(x => x.ToString("X2")));
            }
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbxLog.Text) ||
                string.IsNullOrWhiteSpace(txtbxFIO.Text) ||
                string.IsNullOrWhiteSpace(passBxFrst.Password) ||
                string.IsNullOrWhiteSpace(passBxScnd.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            using (var db = new Entities())
            {
               
                var existing = db.Users.AsNoTracking()
                    .FirstOrDefault(u => u.Login == txtbxLog.Text);
                if (existing != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!");
                    return;
                }

                
                if (passBxFrst.Password.Length < 6)
                {
                    MessageBox.Show("Пароль должен содержать 6 или более символов!");
                    return;
                }

                bool en = true, number = false;
                foreach (char c in passBxFrst.Password)
                {
                    if (c >= '0' && c <= '9') number = true;
                    else if (!((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')))
                        en = false;
                }

                if (!en) { MessageBox.Show("Пароль должен содержать только латиницу!"); return; }
                if (!number) { MessageBox.Show("Пароль должен содержать хотя бы одну цифру!"); return; }
                if (passBxFrst.Password != passBxScnd.Password)
                {
                    MessageBox.Show("Пароли не совпадают!"); return;
                }

                var user = new User
                {
                    Login = txtbxLog.Text,
                    Password = GetHash(passBxFrst.Password),
                    Role = comboBxRole.Text,
                    FIO = txtbxFIO.Text,
                    Photo = null
                };

                db.Users.Add(user);
                db.SaveChanges();
            }

            MessageBox.Show("Пользователь успешно зарегистрирован!");
            NavigationService?.Navigate(new AuthPage());
        }
    }
}