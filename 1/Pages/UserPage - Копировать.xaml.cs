using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PaymentApp.Pages
{
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            Tag = "Пользователи";
            UpdateUsers();
        }

        private void fioFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => UpdateUsers();
        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateUsers();
        private void onlyAdminCheckBox_Checked(object sender, RoutedEventArgs e) => UpdateUsers();
        private void onlyAdminCheckBox_Unchecked(object sender, RoutedEventArgs e) => UpdateUsers();

        private void clearFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            fioFilterTextBox.Text = "";
            sortComboBox.SelectedIndex = 0;
            onlyAdminCheckBox.IsChecked = false;
        }

        private void UpdateUsers()
        {
            if (!IsInitialized) return;
            try
            {
                List<User> users = Entities.GetContext().User.ToList();

                if (!string.IsNullOrWhiteSpace(fioFilterTextBox.Text))
                {
                    string q = fioFilterTextBox.Text.ToLower();
                    users = users.Where(u => u.FIO.ToLower().Contains(q)).ToList();
                }

                if (onlyAdminCheckBox.IsChecked == true)
                    users = users.Where(u => u.Role == "Admin").ToList();

                ListUser.ItemsSource = (sortComboBox.SelectedIndex == 0)
                    ? users.OrderBy(u => u.FIO).ToList()
                    : users.OrderByDescending(u => u.FIO).ToList();
            }
            catch { /* игнорируем */ }
        }
    }
}