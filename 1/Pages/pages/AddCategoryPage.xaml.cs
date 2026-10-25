using _1;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PaymentApp.Pages
{
    public partial class AddCategoryPage : Page
    {
        private Category _current;

        public AddCategoryPage(Category selected)
        {
            InitializeComponent();
            Tag = (selected == null) ? "Добавление категории" : "Редактирование категории";
            _current = selected ?? new Category();
            DataContext = _current;
        }

        private void ButtonSaveCategory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_current.Name))
            {
                MessageBox.Show("Введите название категории!");
                return;
            }
            try
            {
                if (_current.ID == 0)
                    Entities.GetContext().Categories
                        .Add(_current);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Категория сохранена!");
                NavigationService?.GoBack();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ButtonClean_Click(object sender, RoutedEventArgs e)
            => TBCategoryName.Text = "";
    }
}