using _1;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PaymentApp.Pages
{
    public partial class AddUserPage : Page
    {
        private User _currentUser;

        public AddUserPage(User selectedUser)
        {
            InitializeComponent();
            Tag = (selectedUser == null) ? "Добавление пользователя" : "Редактирование пользователя";

            _currentUser = selectedUser ?? new User();
            DataContext = _currentUser;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentUser.Login)) errors.AppendLine("Укажите логин!");
            if (string.IsNullOrWhiteSpace(_currentUser.Password)) errors.AppendLine("Укажите пароль!");
            if (string.IsNullOrWhiteSpace(_currentUser.Role)) errors.AppendLine("Выберите роль!");
            if (string.IsNullOrWhiteSpace(_currentUser.FIO)) errors.AppendLine("Укажите ФИО!");

            if (errors.Length > 0) { MessageBox.Show(errors.ToString()); return; }

            try
            {
                if (_currentUser.ID == 0)
                    Entities.GetContext().Users.Add(_currentUser);

                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены!");
                NavigationService?.GoBack();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            TBLogin.Text = "";
            TBPass.Text = "";
            cmbRole.SelectedIndex = -1;
            TBFio.Text = "";
            TBPhoto.Text = "";
        }
    }
}