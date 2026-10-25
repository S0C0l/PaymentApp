using _1;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PaymentApp.Pages
{
    public partial class ChangePassPage : Page
    {
        public ChangePassPage()
        {
            InitializeComponent();
            Tag = "Смена пароля";
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TbLogin.Text) || string.IsNullOrEmpty(CurrentPasswordBox.Password) ||
                string.IsNullOrEmpty(NewPasswordBox.Password) || string.IsNullOrEmpty(ConfirmPasswordBox.Password))
            {
                MessageBox.Show("Все поля обязательны к заполнению!");
                return;
            }

            if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            if (NewPasswordBox.Password.Length < 6) { MessageBox.Show("Пароль от 6 символов!"); return; }

            string hashedOld = AuthPage.GetHash(CurrentPasswordBox.Password);
            using (var db = Entities.GetContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == TbLogin.Text && u.Password == hashedOld);
                if (user == null)
                {
                    MessageBox.Show("Текущий логин или пароль неверный!");
                    return;
                }

                user.Password = AuthPage.GetHash(NewPasswordBox.Password);
                db.SaveChanges();
            }

            MessageBox.Show("Пароль успешно изменен!");
            NavigationService?.Navigate(new AuthPage());
        }
    }
}