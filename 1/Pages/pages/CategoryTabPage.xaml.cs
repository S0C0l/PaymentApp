using _1;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PaymentApp.Pages
{
    public partial class CategoryTabPage : Page
    {
        public CategoryTabPage()
        {
            InitializeComponent();
            Tag = "Таблица категорий";
            Reload();
        }

        private void Reload()
            => DataGridCategory.ItemsSource = Entities.GetContext().Categories.ToList();

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible) Reload();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
            => NavigationService?.Navigate(new AddCategoryPage(null));

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            var c = (sender as FrameworkElement)?.DataContext as Category;
            if (c != null) NavigationService?.Navigate(new AddCategoryPage(c));
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var forRemove = DataGridCategory.SelectedItems.Cast<Category>().ToList();
            if (forRemove.Count == 0) { MessageBox.Show("Ничего не выбрано"); return; }

            if (MessageBox.Show($"Удалить {forRemove.Count} записей?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

            try
            {
                Entities.GetContext().Categories.RemoveRange(forRemove);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные удалены!");
                Reload();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}