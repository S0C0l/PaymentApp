using _1;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PaymentApp.Pages
{
    public partial class UsersTabPage : Page
    {
        public UsersTabPage()
        {
            InitializeComponent();
            Tag = "Таблица пользователей";
            Reload();
        }

        private void Reload()
        {
            DataGridUser.ItemsSource = Entities.GetContext().Users.ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList()
                    .ForEach(x => x.Reload());
                Reload();
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
            => NavigationService?.Navigate(new AddUserPage(null));

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as FrameworkElement)?.DataContext as User;
            if (user != null)
                NavigationService?.Navigate(new AddUserPage(user));
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var forRemove = DataGridUser.SelectedItems.Cast<User>().ToList();
            if (forRemove.Count == 0) { MessageBox.Show("Ничего не выбрано"); return; }

            if (MessageBox.Show(
                $"Удалить {forRemove.Count} записей?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            try
            {
                Entities.GetContext().Users.RemoveRange((System.Collections.Generic.IEnumerable<User>)forRemove);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные удалены!");
                Reload();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}